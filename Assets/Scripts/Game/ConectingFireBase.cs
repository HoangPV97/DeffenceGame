using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.RemoteConfig;
using System.Threading.Tasks;
using Firebase.Extensions;
using InviGiant.Tools;
public class ConectingFireBase : Singleton<ConectingFireBase>
{
    // Start is called before the first frame update
    public ConfigInfo ConfigInfo;
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
        DataController.Instance.DefaultData = Resources.Load<DefaultData>("Data/DefaultData");
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
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
        ConfigInfo = FirebaseRemoteConfig.Info;

        switch (ConfigInfo.LastFetchStatus)
        {
            case LastFetchStatus.Success:
                FirebaseRemoteConfig.ActivateFetched();
                string txt = FirebaseRemoteConfig.GetValue("Weapons").StringValue;
                // DataController.Instance.WeaponsDatas = JsonUtility.FromJson<WeaponDatabase>(stop);
                Debug.Log("Value: " + (string.IsNullOrEmpty(txt) ? "NA" : txt));
                break;
            case LastFetchStatus.Failure:
                switch (ConfigInfo.LastFetchFailureReason)
                {
                    case FetchFailureReason.Error:
                        Debug.Log("Fetch failed for unknown reason");
                        break;
                    case FetchFailureReason.Throttled:
                        Debug.Log("Fetch throttled until " + ConfigInfo.ThrottledEndTime);
                        break;
                }
                break;
            case LastFetchStatus.Pending:
                Debug.Log("Latest Fetch call still pending.");
                break;
        }
        DataController.Instance.SetUpData();

    }

    public string GetTextAllianceDatabase()
    {
        if (ConfigInfo.LastFetchStatus == LastFetchStatus.Success)
            return FirebaseRemoteConfig.GetValue("AllianceDataBase").StringValue;
        else
        {
            return DataController.Instance.DefaultData.AllianceDataBases.text;
        }
    }

    public string GetTextWeaponDatabase()
    {
        if (ConfigInfo.LastFetchStatus == LastFetchStatus.Success)
            return FirebaseRemoteConfig.GetValue("Weapons").StringValue;
        else
        {
            return DataController.Instance.DefaultData.Weapons.text;
        }
    }

    public string GetTextGameEnemyDataBase()
    {
        if (ConfigInfo.LastFetchStatus == LastFetchStatus.Success)
            return FirebaseRemoteConfig.GetValue("GameEnemyDataBase").StringValue;
        else
        {
            return DataController.Instance.DefaultData.GameEnemyDataBase.text;
        }
    }

    public string GetTextGameStageDataBase()
    {
        if (ConfigInfo.LastFetchStatus == LastFetchStatus.Success)
            return FirebaseRemoteConfig.GetValue("GameStageDataBase").StringValue;
        else
        {
            return DataController.Instance.DefaultData.GameStageDataBase.text;
        }
    }

    public string GetTextMonsterDataBases()
    {
        if (ConfigInfo.LastFetchStatus == LastFetchStatus.Success)
            return FirebaseRemoteConfig.GetValue("MonsterDataBases").StringValue;
        else
        {
            return DataController.Instance.DefaultData.MonsterDataBases.text;
        }
    }

    public string GetTextWeaponSkill(string SkillID)
    {
        if (ConfigInfo.LastFetchStatus == LastFetchStatus.Success)
            return FirebaseRemoteConfig.GetValue(SkillID).StringValue;
        else
        {
            return DataController.Instance.DefaultData.GetTextWeaponSkill(SkillID);
        }
    }

    public string GetTextBaseDataBases()
    {
        if (ConfigInfo.LastFetchStatus == LastFetchStatus.Success)
            return FirebaseRemoteConfig.GetValue("BaseDatabases").StringValue;
        else
        {
            return DataController.Instance.DefaultData.BaseDatabases.text;
        }
    }

    public string GetTextItemDataBase()
    {
        if (ConfigInfo.LastFetchStatus == LastFetchStatus.Success)
            return FirebaseRemoteConfig.GetValue("ItemDataBase").StringValue;
        else
        {
            return DataController.Instance.DefaultData.ItemDataBase.text;
        }
    }
    public string GetTexSpawnEnemyBoss(int level)
    {
        return DataController.Instance.DefaultData.SpawnEnemyBoss[level].text;
    }
}
