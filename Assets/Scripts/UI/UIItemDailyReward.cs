using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class UIItemDailyReward : MonoBehaviour
{
    public TextMeshProUGUI txtDay, txtReward;
    public CanvasGroup CanvasGroup;
    public GameObject goHg, goDone;
    public bool isLastDay = false;
    public Sprite ldSprt1, ldSprt2;
    public int Day;
    // Start is called before the first frame update
    public void SetUpData()
    {
        Day = int.Parse(gameObject.name);
        txtDay.text = "Day " + gameObject.name;
        ResetUI();
    }
    public void ResetUI()
    {
        int DailyLoginNumber = DataController.Instance.GetDailyLoginNumber();
        int DailyLoginNumberDone = DataController.Instance.GetDailyLoginNumberDone();
        goHg.SetActive(Day == DailyLoginNumber);
        goDone.SetActive(Day <= DailyLoginNumberDone);

        if (Day == DailyLoginNumberDone)
            goHg.SetActive(false);
        if (Day <= DailyLoginNumberDone)
        {
            CanvasGroup.alpha = 0.5f;
        }
        else if (Day > DailyLoginNumberDone)
        {
            CanvasGroup.alpha = 0.75f;
        }
        if (Day == DailyLoginNumber && Day > DailyLoginNumberDone)
            CanvasGroup.alpha = 1f;
    }
}
