using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Spine.Unity;
public class UIEvolveHero : BaseUIView
{
    public Animator Anim;
    public SkeletonGraphic sg1, sg2;
    public TextMeshProUGUI txtHeroName1, txtHeroName2;
    public TextMeshProUGUI txtDamage1, txtDamage2;
    public TextMeshProUGUI txtFireRate1, txtFireRate2;
    public TextMeshProUGUI GoldCost;
    public Transform ItemContain;
    public UIButton BtnEvolve, BtnClose;
    public UiItem pfUIItem;
    GameDataWeapon data1;
    Weapons dataBase;
    Weapons dataBase2;
    public Elemental heroElemental;
    int Gold = 0;
    [SerializeField]
    bool canEvolve = true;
    List<Item> listItem;
    SkeletonDataAsset SkeletonDataAsset;
    public bool IsHero
    {
        get
        {
            return MenuController.Instance.UIPanelHeroAlliance.isHero;
        }
    }
    private void Start()
    {
        BtnClose.SetUpEvent(OnBtnCloseClick);
        BtnEvolve.SetUpEvent(OnBtnEvolveClick);
    }

    void OnBtnEvolveClick()
    {
        if (canEvolve)
        {
            if (IsHero)
                DataController.Instance.AddWeaponTier(heroElemental);
            else
                DataController.Instance.AddAllianceTier(heroElemental);

            DataController.Instance.Gold -= Gold;
            for (int i = 0; i < listItem.Count; i++)
            {
                if (listItem[i].Type != ITEM_TYPE.None)
                {
                    DataController.Instance.AddItemQuality(listItem[i].Type, -listItem[i].Quality);
                }
            }
            MenuController.Instance.UIPanelHeroAlliance.SetupUIHero(heroElemental);
            DataController.Instance.Save();
            OnBtnCloseClick();
            // SetUpData(heroElemental);
        }
    }

    void OnBtnCloseClick()
    {
        OnHide();
    }
    public void SetUpData(Elemental elemental)
    {
        heroElemental = elemental;
        OnShow();
        canEvolve = true;
        Gold = 0;
        if (IsHero)
        {
            data1 = DataController.Instance.GetGameDataWeapon(elemental);
            dataBase = DataController.Instance.GetDataBaseWeapons(elemental, data1.WeaponTierLevel.Tier);
            dataBase2 = DataController.Instance.GetDataBaseWeapons(elemental, data1.WeaponTierLevel.Tier + 1);
            txtHeroName1.text = Language.GetKey("Name_" + elemental.ToString()) + "." + ToolHelper.ToRoman(data1.WeaponTierLevel.Tier);
            txtHeroName2.text = Language.GetKey("Name_" + elemental.ToString()) + "." + ToolHelper.ToRoman(data1.WeaponTierLevel.Tier + 1);
            SkeletonDataAsset = DataController.Instance.DefaultData.WeaponsUISkeletonDataAsset[(int)elemental - 1];
        }
        else
        {
            data1 = DataController.Instance.GetGameAlliance(elemental);
            dataBase = DataController.Instance.GetAllianceDataBases(elemental, data1.WeaponTierLevel.Tier).weapons;
            dataBase2 = DataController.Instance.GetAllianceDataBases(elemental, data1.WeaponTierLevel.Tier + 1).weapons;
            txtHeroName1.text = Language.GetKey("Name_Alliance_" + elemental.ToString()) + "." + ToolHelper.ToRoman(data1.WeaponTierLevel.Tier);
            txtHeroName2.text = Language.GetKey("Name_Alliance_" + elemental.ToString()) + "." + ToolHelper.ToRoman(data1.WeaponTierLevel.Tier + 1);
            SkeletonDataAsset = DataController.Instance.DefaultData.AllianceUISkeletonDataAsset[(int)elemental - 1];
        }
        int Level = data1.WeaponTierLevel.Level > 0 ? data1.WeaponTierLevel.Level : 1;
        txtDamage1.text = dataBase.ATK[Level - 1].ToString();
        txtFireRate1.text = dataBase.ATKspeed[Level - 1].ToString();
        txtDamage2.text = dataBase2.ATK[0] > dataBase.ATK[Level - 1] ? string.Format("<color=#65FF00FF>{0}</color>", dataBase2.ATK[0]) : dataBase2.ATK[0].ToString();
        txtFireRate2.text = dataBase2.ATKspeed[0] > dataBase.ATKspeed[Level - 1] ? string.Format("<color=#65FF00FF>{0}</color>", dataBase2.ATKspeed[0]) : dataBase2.ATKspeed[0].ToString();

        /// init Item
        foreach (Transform child in ItemContain)
        {
            Destroy(child.gameObject);
        }
        listItem = dataBase.CostEvolution;
        for (int i = 0; i < listItem.Count; i++)
        {
            var it = Instantiate(pfUIItem.gameObject, Vector3.zero, Quaternion.identity);
            it.SetActive(true);
            it.transform.SetParent(ItemContain);
            it.transform.SetDefaultTransform();
            var itInInventory = DataController.Instance.GetGameItemData(listItem[i].Type);
            it.GetComponent<UiItem>().SetUpData(listItem[i], itInInventory.Quality, 2);
            var itDataBase = DataController.Instance.GetItemDataBase(listItem[i].Type);
            Gold += listItem[i].Quality * itDataBase.UseGoldCost;
            if (listItem[i].Quality > itInInventory.Quality)
                canEvolve = false;
        }
        if (Gold > DataController.Instance.Gold)
            canEvolve = false;
        GoldCost.text = Gold.ToString();
        sg1.skeletonDataAsset = SkeletonDataAsset;
        sg2.skeletonDataAsset = SkeletonDataAsset;
        sg1.Initialize(true);
        sg2.Initialize(true);
        sg1.Skeleton.SetSkin("tier" + (data1.WeaponTierLevel.Tier));
        sg2.Skeleton.SetSkin("tier" + (data1.WeaponTierLevel.Tier + 1));
        sg1.AnimationState.SetAnimation(0, "idle", true);
        sg2.AnimationState.SetAnimation(0, "idle", true);
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
