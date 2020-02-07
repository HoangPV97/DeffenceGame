using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
public class EnergyController : MonoBehaviour
{
    private const int ENERGY_REPLENISH_TIME = 180, MAX_ENERGY = 5;
    [SerializeField] private TextMeshProUGUI energyCount, timeCountDown;
    [SerializeField] private GameObject energyPropPrefab, unlimitedEnergyPropPrefab;
    [SerializeField] private Transform energyIcon, cameraCanvas;
    [SerializeField] private Button buyEnergyBtn;
    [SerializeField] private GameObject unlimitedIcon;
    private int currentEnergy;
    private void Start()
    {
        DataController.Instance.Energy += CalculateGainEnergy();
        energyCount.text = DataController.Instance.Energy + "/" + MAX_ENERGY;
        currentEnergy = DataController.Instance.Energy;
        StartCoroutine(ReplenishEnergy());
    }
    private IEnumerator ReplenishEnergy()
    {
        var delay = new WaitForSecondsRealtime(1);
        var saveTimeStamp = DataController.Instance.EnergyTimeStamp;
        double deltaTime;
        int remainTime;
        while (true)
        {

            deltaTime = (DateTime.Now - saveTimeStamp).TotalSeconds;
            if (deltaTime < -ENERGY_REPLENISH_TIME)
            {
                DataController.Instance.EnergyTimeStamp = DateTime.Now;//user hack time
                deltaTime = 0;
            }
            if (deltaTime > ENERGY_REPLENISH_TIME)
            {
                saveTimeStamp = DateTime.Now;
                DataController.Instance.Energy++;//add a energy
                currentEnergy = DataController.Instance.Energy;

            }
            if (DataController.Instance.Energy < MAX_ENERGY)//calculate the remain time and convert to minute and second
            {
                remainTime = (int)(ENERGY_REPLENISH_TIME - deltaTime);
                if (remainTime < 0) remainTime = 0;
            }
            else
            {
            }
            yield return delay;
        }
    }
    private int CalculateGainEnergy()//this function calculate the offline energy of player
    {
        double deltaTime = (DateTime.Now - DataController.Instance.EnergyTimeStamp).TotalSeconds;
        Debug.Log(deltaTime);
        return Mathf.Min((int)(deltaTime / ENERGY_REPLENISH_TIME), MAX_ENERGY);
    }
    public void OpenBuyEnergyPanel()
    {
        //buyEnergyPanelPrefab.Spawn(cameraCanvas, PlayIncreaseEnergyEffect);
    }
    public void PlayIncreaseEnergyEffect()
    {
        /* energyCount.text = DataController.Instance.Energy + "/" + MAX_ENERGY;
         int amount = DataController.Instance.Energy - currentEnergy;
         currentEnergy = DataController.Instance.Energy;
         energyCount.text = DataController.Instance.Energy + "/" + MAX_ENERGY;
         buyEnergyBtn.gameObject.SetActive(DataController.Instance.Energy == 0);
         for (int i = 0; i < amount; i++)
         {
             GameObject energyProp = Instantiate(energyPropPrefab, cameraCanvas.position + new Vector3(0, 0, -0.1f), Quaternion.identity, cameraCanvas);
             Vector3 target = energyProp.transform.position + new Vector3(UnityEngine.Random.Range(-1.5f, 1.5f), UnityEngine.Random.Range(-1.5f, 1.5f), 0);//generate a random pos
             System.Action<ITween<Vector3>> updatePropPos = (t) =>
                 {
                     energyProp.transform.position = t.CurrentValue;
                 };
             System.Action<ITween<Vector3>> completedPropMovement = (t) =>
             {
                 Destroy(energyProp);
             };
             TweenFactory.Tween("energy" + i + Time.time, energyProp.transform.position, target, 0.5f + i * 0.06f, TweenScaleFunctions.QuinticEaseOut, updatePropPos)
             .ContinueWith(new Vector3Tween().Setup(target, energyIcon.position, 0.25f + i * 0.06f, TweenScaleFunctions.QuadraticEaseIn, updatePropPos, completedPropMovement));
         }*/
    }
    public void PlayUnlimitedEnergyEffect()
    {
        /* GameObject energyProp = Instantiate(unlimitedEnergyPropPrefab, cameraCanvas.position + new Vector3(0, 0, -1f), Quaternion.identity, cameraCanvas);
         Vector3 target = energyProp.transform.position + new Vector3(UnityEngine.Random.Range(-1.5f, 1.5f), UnityEngine.Random.Range(-1.5f, 1.5f), 0);//generate a random pos
         System.Action<ITween<Vector3>> updatePropPos = (t) =>
             {
                 energyProp.transform.position = t.CurrentValue;
             };
         System.Action<ITween<Vector3>> completedPropMovement = (t) =>
         {
             DataController.Instance.UnlimitedEnergyDuration -= CalculateTimePassUnlimitedEnergy();
             remainUnlimitedTime = DataController.Instance.UnlimitedEnergyDuration;
             unlimitedIcon.SetActive(remainUnlimitedTime > 0);
             energyCount.gameObject.SetActive(remainUnlimitedTime == 0);
             Destroy(energyProp);
         };
         TweenFactory.Tween("energy" + Time.time, energyProp.transform.position, target, 0.5f, TweenScaleFunctions.QuinticEaseOut, updatePropPos)
         .ContinueWith(new Vector3Tween().Setup(target, unlimitedIcon.transform.position, 0.5f, TweenScaleFunctions.QuadraticEaseIn, updatePropPos, completedPropMovement));
    */
    }
}
