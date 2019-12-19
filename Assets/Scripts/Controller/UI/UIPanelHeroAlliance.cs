using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.UI;
public class UIPanelHeroAlliance : MonoBehaviour, IBaseUI
{
    [Header("Hero")]
    public GameObject PanelHero;
    public TextMeshProUGUI txtHeroName, txtLevel;
    public TextMeshProUGUI txtDamage, txtFireRate, txtEXP;
    public Image PBDamage, PBFireRate, PBEXP;
    public Image ElementalIcon;
    public UIHeroItem[] UIHeroItems;
    public UISkillItem[] UISkillItems;
    UIHeroItem currentSelectedHero = null;
    public UIButton BtnEquip, BtnUpgrade;
    public UIUpgradehero UIUpgradehero;
    public bool isHero = true;
    #region Animation
    public void OnHide()
    {
        gameObject.SetActive(false);
    }

    public void OnHideLeft(UnityAction unityAction = null)
    {
        throw new System.NotImplementedException();
    }

    public void OnHideRight(UnityAction unityAction = null)
    {
        throw new System.NotImplementedException();
    }

    public void OnShow()
    {
        gameObject.SetActive(true);
    }

    public void OnShowFromLeft(UnityAction unityAction = null)
    {
        throw new System.NotImplementedException();
    }

    public void OnShowFromRight(UnityAction unityAction = null)
    {
        throw new System.NotImplementedException();
    }
    #endregion
    void Awake()
    {
        UIHeroItems = GetComponentsInChildren<UIHeroItem>();
        UISkillItems = GetComponentsInChildren<UISkillItem>();
        BtnUpgrade.SetUpEvent(Upgradehero);
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
        OnShow();
        currentSelectedHero = null;
        this.isHero = isHero;
        bool showAliiance = false;
        for (int i = 0; i < UIHeroItems.Length; i++)
        {
            UIHeroItems[i].SetupData();
            if (isHero)
            {
                if (UIHeroItems[i].elemental == DataController.Instance.CurrentSelectedWeapon)
                {
                    UIHeroItems[i].OnSelected();
                }
            }
            else
            {
                if (DataController.Instance.ElementalSlot1 != Elemental.None && UIHeroItems[i].elemental == DataController.Instance.ElementalSlot1)
                {
                    UIHeroItems[i].OnSelected();
                    showAliiance = true;
                    break;
                }
                else if (DataController.Instance.ElementalSlot2 != Elemental.None && UIHeroItems[i].elemental == DataController.Instance.ElementalSlot2)
                {
                    UIHeroItems[i].OnSelected();
                    showAliiance = true;
                    break;
                }

            }
        }
        if (!isHero && !showAliiance)
            UIHeroItems[0].OnSelected();
    }
    public void OnSelectHero(UIHeroItem UIHeroItems)
    {
        if (currentSelectedHero != null && currentSelectedHero != UIHeroItems)
            currentSelectedHero.OnUnSelect();
        if (currentSelectedHero != UIHeroItems)
            SetupUIHero(UIHeroItems.elemental);
        currentSelectedHero = UIHeroItems;
    }

    public void SetupUIHero(Elemental elemental)
    {
        if (isHero)
            txtHeroName.text = Language.GetKey("Name_" + elemental.ToString());
        else
            txtHeroName.text = Language.GetKey("Name_Alliance_" + elemental.ToString());
        GameDataWeapon data1;
        if (isHero)
            data1 = DataController.Instance.GetGameDataWeapon(elemental);
        else
            data1 = DataController.Instance.GetGameAlliance(elemental);

        Weapons dataBase;
        if (isHero)
            dataBase = DataController.Instance.GetDataBaseWeapons(elemental, data1.WeaponTierLevel.Tier);
        else
            // dataBase = DataController.Instance.GetAllianceDataBases(elemental, data1.WeaponTierLevel.Tier);
            dataBase = DataController.Instance.GetAllianceDataBases(elemental, data1.WeaponTierLevel.Tier).weapons;

        BtnEquip.gameObject.SetActive(data1.WeaponTierLevel.Level >= 1 && data1.Type != DataController.Instance.CurrentSelectedWeapon);
        BtnUpgrade.gameObject.SetActive(data1.WeaponTierLevel.Level >= 1);
        txtLevel.gameObject.SetActive(data1.WeaponTierLevel.Level >= 1);
        int Level = data1.WeaponTierLevel.Level > 0 ? data1.WeaponTierLevel.Level : 1;
        txtLevel.text = Language.GetKey("Level") + " " + data1.WeaponTierLevel.Level + "/" + dataBase.ATKspeed.Count;
        txtDamage.text = dataBase.ATK[Level - 1].ToString();
        PBDamage.fillAmount = dataBase.ATK[Level - 1] * 1f / dataBase.ATK[dataBase.ATK.Count - 1];
        txtFireRate.text = dataBase.ATKspeed[Level - 1].ToString();
        PBFireRate.fillAmount = dataBase.ATKspeed[Level - 1] * 1f / dataBase.ATKspeed[dataBase.ATKspeed.Count - 1];
        txtEXP.text = data1.EXP.ToString();
        PBEXP.fillAmount = data1.EXP * 1f / dataBase.Cost[dataBase.Cost.Count - 1];

        /// Set up Skill

    }

}
