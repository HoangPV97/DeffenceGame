using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.UI;
using System;
using Spine.Unity;

public class UIPanelHeroAlliance : BaseUIView
{
    [Header("Hero")]
    public GameObject PanelHero;
    public TabController TabController;
    public TextMeshProUGUI txtHeroName, txtLevel;
    public TextMeshProUGUI txtDamage, txtFireRate, txtEXP;
    public Image PBDamage, PBFireRate, PBEXP;
    public Image ElementalIcon;
    public UIHeroItem[] UIHeroItems;
    public UISkillItem[] UISkillItems;
    public UISkillItem currentSelectedUISkillItems = null;
    UIHeroItem currentSelectedHero = null;
    public UIButton BtnEquip, BtnUpgrade, BtnUnEquip, BtnEvolve;
    public UIUpgradehero UIUpgradehero;
    public bool isHero = true;
    public UIEvolveHero UIEvolveHero;
    public UiUpgradeSkill UiUpgradeSkill;
    public UIEvolveSkill UIEvolveSkill;
    public SkeletonGraphic sg1;
    SkeletonDataAsset SkeletonDataAsset;
    public GameObject[] Star;
    public Elemental SelectedElemental
    {
        get
        {
            return currentSelectedHero.elemental;
        }
    }

    #region Animation

    #endregion
    void Awake()
    {

        BtnUpgrade.SetUpEvent(Upgradehero);
        BtnEquip.SetUpEvent(OnEquip);
        BtnUnEquip.SetUpEvent(OnUnEquip);
        BtnEvolve.SetUpEvent(OnEvolve);
    }

    private void OnEquip()
    {
        if (isHero)
        {
            DataController.Instance.CurrentSelectedWeapon = currentSelectedHero.elemental;
            SetupUIHero(currentSelectedHero.elemental);
        }
        else
        {
            if (DataController.Instance.ElementalSlot1 == Elemental.None)
                DataController.Instance.ElementalSlot1 = currentSelectedHero.elemental;
            else if (DataController.Instance.ElementalSlot2 == Elemental.None)
                DataController.Instance.ElementalSlot2 = currentSelectedHero.elemental;
            SetupUIHero(currentSelectedHero.elemental);
        }
    }

    private void OnUnEquip()
    {
        if (!isHero)
        {
            if (DataController.Instance.ElementalSlot1 == currentSelectedHero.elemental)
                DataController.Instance.ElementalSlot1 = Elemental.None;
            else if (DataController.Instance.ElementalSlot2 == currentSelectedHero.elemental)
                DataController.Instance.ElementalSlot2 = Elemental.None;
            SetupUIHero(currentSelectedHero.elemental);
        }
    }

    private void OnEvolve()
    {
        UIEvolveHero.SetUpData(currentSelectedHero.elemental);
    }

    void Upgradehero()
    {
        UIUpgradehero.SetUpData(currentSelectedHero.elemental);
    }

    public void ReSetUI()
    {
        throw new System.NotImplementedException();
    }

    public void UpDateData()
    {
        throw new System.NotImplementedException();
    }

    public void SetUpData(bool isHero = true)
    {
        this.isHero = isHero;

        //  if (currentSelectedUISkillItems != null)
        //      currentSelectedUISkillItems.OnUnSelect();
        currentSelectedUISkillItems = null;

        //   if (currentSelectedHero != null)
        //      currentSelectedHero.OnUnSelect();
        currentSelectedHero = null;

        if (UISkillItems.Length == 0)
        {
            UISkillItems = GetComponentsInChildren<UISkillItem>();
        }
        if (UIHeroItems.Length == 0)
            UIHeroItems = GetComponentsInChildren<UIHeroItem>();

        for (int i = 0; i < UIHeroItems.Length; i++)
        {
            UIHeroItems[i].SetupData();
        }

        DG.Tweening.DOVirtual.DelayedCall(0.02f, () =>
        {
            Debug.Log("?????????????????");
            UIHeroItems[0].OnSelected();
        });

    }
    public void OnSelectSkill(UISkillItem uISkillItem)
    {
        if (currentSelectedUISkillItems != null && currentSelectedUISkillItems != uISkillItem)
            currentSelectedUISkillItems.OnUnSelect();
        if (currentSelectedUISkillItems != uISkillItem)
            currentSelectedUISkillItems = uISkillItem;
    }

    public void OnSelectHero(UIHeroItem UIHeroItems)
    {
        if (currentSelectedHero != null && currentSelectedHero != UIHeroItems)
            currentSelectedHero.OnUnSelect();
        if (currentSelectedHero != UIHeroItems)
            SetupUIHero(UIHeroItems.elemental);
        currentSelectedHero = UIHeroItems;
    }
    public void SetupUIHero()
    {
        SetupUIHero(currentSelectedHero.elemental);
    }

    public void SetupUIHero(Elemental elemental)
    {
        GameDataWeapon data1;
        Weapons dataBase;

        if (isHero)
        {
            txtHeroName.text = Language.GetKey("Name_" + elemental.ToString());
            data1 = DataController.Instance.GetGameDataWeapon(elemental);
            dataBase = DataController.Instance.GetDataBaseWeapons(elemental, data1.WeaponTierLevel.Tier);
            BtnEquip.gameObject.SetActive(data1.WeaponTierLevel.Level >= 1 && data1.Type != DataController.Instance.CurrentSelectedWeapon);
            BtnUnEquip.gameObject.SetActive(false);
            SkeletonDataAsset = DataController.Instance.DefaultData.WeaponsUISkeletonDataAsset[(int)elemental - 1];
        }
        else
        {
            txtHeroName.text = Language.GetKey("Name_Alliance_" + elemental.ToString());
            data1 = DataController.Instance.GetGameAlliance(elemental);
            dataBase = DataController.Instance.GetAllianceDataBases(elemental, data1.WeaponTierLevel.Tier).weapons;
            if (data1.Type == DataController.Instance.ElementalSlot1 || data1.Type == DataController.Instance.ElementalSlot2)
            {
                BtnUnEquip.gameObject.SetActive(true);
                BtnEquip.gameObject.SetActive(false);
            }
            else
            {
                BtnUnEquip.gameObject.SetActive(false);
                BtnEquip.gameObject.SetActive(DataController.Instance.CanEquipAlliance && data1.WeaponTierLevel.Level >= 1);
            }
            SkeletonDataAsset = DataController.Instance.DefaultData.AllianceUISkeletonDataAsset[(int)elemental - 1];
        }

        BtnUpgrade.gameObject.SetActive(data1.WeaponTierLevel.Level >= 1);
        if (data1.WeaponTierLevel.Level == dataBase.MaxLevel && data1.EXP == dataBase.MaxEXP)
        {
            BtnEvolve.gameObject.SetActive(data1.WeaponTierLevel.Tier < 3);
            BtnUpgrade.gameObject.SetActive(false);
        }
        else
        {
            BtnEvolve.gameObject.SetActive(false);
        }

        txtLevel.gameObject.SetActive(data1.WeaponTierLevel.Level >= 1);
        int Level = data1.WeaponTierLevel.Level > 0 ? data1.WeaponTierLevel.Level : 1;
        txtLevel.text = Language.GetKey("Level") + " " + data1.WeaponTierLevel.Level + "/" + dataBase.ATKspeed.Count;
        txtDamage.text = dataBase.GetATK(Level).ToString();
        PBDamage.fillAmount = dataBase.GetATK(Level) * 1f / dataBase.GetATK(dataBase.ATK.Count);
        txtFireRate.text = dataBase.GetATKspeed(Level).ToString();
        PBFireRate.fillAmount = dataBase.GetATKspeed(Level) * 1f / dataBase.GetATKspeed(dataBase.ATKspeed.Count);
        txtEXP.text = data1.EXP.ToString();
        if (data1.WeaponTierLevel.Level > 0)
            PBEXP.fillAmount = data1.EXP * 1f / dataBase.Cost[data1.WeaponTierLevel.Level - 1];
        else
            PBEXP.fillAmount = 0;
        if (UISkillItems.Length == 0)
            UISkillItems = GetComponentsInChildren<UISkillItem>();
        /// Set up Skill
        if (isHero)
        {
            var SkillList = DataController.Instance.GetWeaponSkillID(elemental);
            for (int i = 0; i < UISkillItems.Length; i++)
            {
                UISkillItems[i].SetUpdata(SkillList[i]);
            }
        }
        else
        {
            var SkillList = DataController.Instance.GetAllianceSkillID(elemental);
            for (int i = 0; i < UISkillItems.Length; i++)
            {
                UISkillItems[i].SetUpdata(SkillList[i]);
            }
        }

        UISkillItems[0].OnBtnSelectClick();
        sg1.skeletonDataAsset = SkeletonDataAsset;
        sg1.Initialize(true);
        sg1.Skeleton.SetSkin("tier" + (data1.WeaponTierLevel.Tier));
        sg1.AnimationState.SetAnimation(0, "idle", true);
        for (int i = 0; i < Star.Length; i++)
        {
            Star[i].SetActive(i < data1.WeaponTierLevel.Tier);
        }
    }

    UISkillItem GetUISkillItem(string SkillID)
    {
        for (int i = 0; i < UISkillItems.Length; i++)
        {
            if (UISkillItems[i].skillData.SkillID == SkillID)
                return UISkillItems[i];
        }
        return null;
    }

    public void ReSetUISkill(string SkillID)
    {
        GetUISkillItem(SkillID).ReSetUISkill();
    }

    public void OnUpgradeSkill(SkillData skillData, SaveGameTierLevel saveGameTierLevel)
    {
        UiUpgradeSkill.SetUpData(skillData, saveGameTierLevel);
    }

    public void OnEvolveSkill(SkillData skillData, SaveGameTierLevel saveGameTierLevel)
    {
        UIEvolveSkill.SetUpData(skillData, saveGameTierLevel);
    }
}
