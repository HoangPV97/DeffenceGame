using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Spine.Unity;
public class UIUnlockWeaponAlliance : BaseUIView
{
    public Animator Anim;
    public TextMeshProUGUI txtDes;
    public Sprite[] SprIcons;
    public Image imgIcon;
    public UIButton btnClose;
    public SkeletonGraphic SkeletonGraphic;
    SkeletonDataAsset SkeletonDataAsset;
    public void Start()
    {
        btnClose.SetUpEvent(OnHide);
    }
    public void SetUpData(string str)
    {
        OnShow();
        var strSplit = str.Split('_');
        Elemental elemental = (Elemental)System.Enum.Parse(typeof(Elemental), strSplit[1]);
        if (strSplit[0] == "Weapon")
        {
            SkeletonDataAsset = DataController.Instance.DefaultData.WeaponsUISkeletonDataAsset[(int)elemental - 1];
        }
        else
        {
            SkeletonDataAsset = DataController.Instance.DefaultData.AllianceUISkeletonDataAsset[(int)elemental - 1];
        }
        SkeletonGraphic.skeletonDataAsset = SkeletonDataAsset;
        SkeletonGraphic.Initialize(true);
        SkeletonGraphic.Skeleton.SetSkin("tier1");
        SkeletonGraphic.AnimationState.SetAnimation(0, "idle", true);
        imgIcon.sprite = SprIcons[(int)elemental - 1];
        txtDes.text = Language.GetKey("Des_Unlock_" + str);
        DataController.Instance.SetStringUNLOCK_UI("NONE");
        DataController.Instance.Save();
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
