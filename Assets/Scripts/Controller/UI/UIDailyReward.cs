using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;


public class UIDailyReward : BaseUIView
{
    public Animator Anim;
    public UIItemDailyReward[] uIItemDailyRewards;
    public UIButton[] btnClose;
    public UIButton btnClaim;
    private void Awake()
    {
        uIItemDailyRewards = GetComponentsInChildren<UIItemDailyReward>();
        for (int i = 0; i < btnClose.Length; i++)
        {
            btnClose[i].SetUpEvent(OnHide);
        }
    }
    public void SetUpData()
    {
        OnShow();
        for (int i = 0; i < uIItemDailyRewards.Length; i++)
            uIItemDailyRewards[i].SetUpData();
        if (DataController.Instance.GetDailyLoginNumberDone() != DataController.Instance.GetDailyLoginNumber())
        {
            btnClaim.gameObject.SetActive(true);
            btnClose[0].gameObject.SetActive(false);
        }
        else
        {
            btnClaim.gameObject.SetActive(false);
            btnClose[0].gameObject.SetActive(true);
        }
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
