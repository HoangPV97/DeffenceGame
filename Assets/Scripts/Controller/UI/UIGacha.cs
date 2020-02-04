using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Spine.Unity;
using Spine;

public class UIGacha : BaseUIView
{
    public Animator Anim;
    public UIButton btnClose;
    public FortuneWheelManager FortuneWheelManager;
    public SkeletonGraphic SkeletonGraphic;
    bool canGo;
    // Start is called before the first frame update
    void Start()
    {
        btnClose.SetUpEvent(OnHide);
        SkeletonGraphic.AnimationState.Complete += OnComplete;
        FortuneWheelManager.Complete += OnCompleteGacha;
    }

    private void OnCompleteGacha(int Result)
    {
        var item = new Item();
        switch (Result)
        {
            case 0:
                item.Type = ITEM_TYPE.Evolve_Spell;
                item.Quality = 1;
                break;
            case 1:
                item.Type = ITEM_TYPE.coin;
                item.Quality = 500;
                break;
            case 2:
                item.Type = ITEM_TYPE.Evolve_Weapon;
                item.Quality = 1;
                break;
            case 3:
                item.Type = ITEM_TYPE.coin;
                item.Quality = 2000;
                break;
            case 4:
                ITEM_TYPE[] iTEM_s = new ITEM_TYPE[] { ITEM_TYPE.FireObs_2, ITEM_TYPE.IceObs_2, ITEM_TYPE.EarthObs_2, ITEM_TYPE.WindObs_2 };
                item.Type = iTEM_s[UnityEngine.Random.Range(0, 4)];
                item.Quality = 1;
                break;
            case 5:
                item.Type = ITEM_TYPE.gem;
                item.Quality = 10;
                break;
            case 6:
                item.Type = ITEM_TYPE.coin;
                item.Quality = 1000;
                break;
            case 7:
                item.Type = ITEM_TYPE.gem;
                item.Quality = 2;
                break;
        }
        DataController.Instance.AddItemQuality(item);
        Debug.Log("OnCompleteGacha: " + Result + "  " + item.ToString());
        DG.Tweening.DOVirtual.DelayedCall(0.2f, () =>
        {
            MenuController.Instance.OnShowReward(new List<Item>() { item });
        }, false);
        DG.Tweening.DOVirtual.DelayedCall(0.2f, () =>
        {
            SkeletonGraphic.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }, false);
        DataController.Instance.SaveLastPlayGacha();
        DataController.Instance.Save();
        MenuController.Instance.HideBtnGacha();
    }

    public void OnClickGo()
    {
        if (canGo)
        {
            canGo = false;
            FortuneWheelManager.TurnWheel();
        }
    }


    private void OnComplete(TrackEntry trackEntry)
    {
        if (trackEntry.Animation.Name == "start")
            SkeletonGraphic.AnimationState.SetAnimation(0, "idle", true);
    }

    public override void OnHide()
    {
        Debug.Log(FortuneWheelManager._isStarted);
        if (!FortuneWheelManager._isStarted)
        {
            base.OnHide();
            Anim.SetTrigger("HidePanelSetting");
            SkeletonGraphic.AnimationState.SetAnimation(0, "start", false);
            DG.Tweening.DOVirtual.DelayedCall(0.3f, () =>
            {
                SkeletonGraphic.gameObject.SetActive(false);
                gameObject.SetActive(false);
            }, false);
        }
    }
    public override void OnShow()
    {
        gameObject.SetActive(true);
        SkeletonGraphic.AnimationState.SetAnimation(0, "start", false);
        FortuneWheelManager._isStarted = false;
        Anim.SetTrigger("ShowPanelSetting");
        SkeletonGraphic.gameObject.SetActive(true);
        canGo = true;
        DataController.Instance.WinGachaCount = 0;
        DataController.Instance.ShowGachaFail = false;
    }
}
