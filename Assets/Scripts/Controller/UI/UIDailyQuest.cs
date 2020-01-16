using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class UIDailyQuest : BaseUIView
{
    public Animator Anim;
    public UIItemQuest[] uIItemQuests;
    public TextMeshProUGUI txtTimer;
    public UIButton btnClose;
    double secondsUntilMidnight;
    private void Start()
    {
        btnClose.SetUpEvent(OnHide);
    }
    public void SetUpData()
    {
        DataController.Instance.CheckDailyQuest();
        var ddq = DataController.Instance.GetGameDataQuests();
        for (int i = 0; i < uIItemQuests.Length; i++)
        {
            uIItemQuests[i].SetUpData(ddq[i]);
        }
        OnShow();
        DateTime current = DateTime.Now; // current time
        DateTime tomorrow = current.AddDays(1).Date; // this is the "next" midnight
        secondsUntilMidnight = (tomorrow - current).TotalSeconds;
        StartCoroutine(UpdateTimer());
    }
    private IEnumerator UpdateTimer()
    {
        secondsUntilMidnight--;
        TimeSpan timeSpan = TimeSpan.FromSeconds(secondsUntilMidnight);
        txtTimer.text = string.Format("{0:D2}:{1:D2}:{2:D2}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
        yield return new WaitForSeconds(1);
        StartCoroutine(UpdateTimer());
    }
    public override void OnHide()
    {
        base.OnHide();
        Anim.SetTrigger("HidePanelSetting");
        DG.Tweening.DOVirtual.DelayedCall(0.3f, () =>
        {
            gameObject.SetActive(false);
        }, false);
    }
    public override void OnShow()
    {
        gameObject.SetActive(true);
        Anim.SetTrigger("ShowPanelSetting");
    }
}
