using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UIDailyQuest : BaseUIView
{
    public Animator Anim;
    public UIItemQuest[] uIItemQuests;
    public TextMeshProUGUI txtTimer;
    public UIButton btnClose;

    private void Start()
    {
        btnClose.SetUpEvent(OnHide);
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
