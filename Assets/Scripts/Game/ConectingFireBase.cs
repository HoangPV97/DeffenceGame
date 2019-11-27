using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.RemoteConfig;
using System.Threading.Tasks;
using Firebase.Extensions;

public class ConectingFireBase : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Awake()
    {
        Firebase.DependencyStatus dependencyStatus = Firebase.DependencyStatus.UnavailableOther;
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                InitFirebase();
            }
        });
    }
    public void InitFirebase()
    {
        Task fetchTask = FirebaseRemoteConfig.FetchAsync(System.TimeSpan.Zero);
        fetchTask.ContinueWithOnMainThread(OnFetchCompleted);
    }
    private void OnFetchCompleted(Task fetchTask)
    {
        if (fetchTask.IsFaulted)
        {
        }
        else if (fetchTask.IsCompleted)
        {
            Debug.Log("Fetch completed successfully!");
        }
        var info = FirebaseRemoteConfig.Info;

        switch (info.LastFetchStatus)
        {
            case LastFetchStatus.Success:
                FirebaseRemoteConfig.ActivateFetched();
                string stop = FirebaseRemoteConfig.GetValue("Weapons").StringValue;

                WeaponsData.Instance.Weapons = JsonUtility.FromJson<TestWeapon>(stop);
                Debug.Log("Value: " + (string.IsNullOrEmpty(stop) ? "NA" : stop));
                break;
            case LastFetchStatus.Failure:
                switch (info.LastFetchFailureReason)
                {
                    case FetchFailureReason.Error:
                        Debug.Log("Fetch failed for unknown reason");
                        break;
                    case FetchFailureReason.Throttled:
                        Debug.Log("Fetch throttled until " + info.ThrottledEndTime);
                        break;
                }
                break;
            case LastFetchStatus.Pending:
                Debug.Log("Latest Fetch call still pending.");
                break;
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
}
