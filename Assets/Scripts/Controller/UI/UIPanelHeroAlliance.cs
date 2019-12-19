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
    [Header("Alliance")]
    public GameObject PanelAlliance;

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

    public void SetUpData()
    {
        OnShow();
        for (int i = 0; i < UIHeroItems.Length; i++)
        {
            UIHeroItems[i].SetupData();
            if (UIHeroItems[i].elemental == DataController.Instance.CurrentSelectedWeapon)
            {
                UIHeroItems[i].OnSelected();
            }
        }
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
        txtHeroName.text = Language.GetKey("Name_" + elemental.ToString());
        var data1 = DataController.Instance.GetGameDataWeapon(elemental);
        var dataBase = DataController.Instance.GetDataBaseWeapons(elemental, data1.WeaponTierLevel.Tier);
        BtnEquip.gameObject.SetActive(data1.WeaponTierLevel.Level >= 1 && data1.Type != DataController.Instance.CurrentSelectedWeapon);
        BtnUpgrade.gameObject.SetActive(data1.WeaponTierLevel.Level >= 1);
        txtLevel.gameObject.SetActive(data1.WeaponTierLevel.Level >= 1);
        int Level = data1.WeaponTierLevel.Level > 0 ? data1.WeaponTierLevel.Level : 1;
        txtLevel.text = Language.GetKey("Level") + " " + data1.WeaponTierLevel.Level + "/" + dataBase.ATKspeed.Count;
        txtDamage.text = dataBase.ATK[Level - 1].ToString();
        PBDamage.fillAmount = dataBase.ATK[Level - 1] * 1f / dataBase.ATK[dataBase.ATK.Count - 1];
        txtFireRate.text = dataBase.ATKspeed[Level - 1].ToString();
        PBFireRate.fillAmount = dataBase.ATKspeed[Level - 1] * 1f / dataBase.ATK[dataBase.ATKspeed.Count - 1];
        txtFireRate.text = dataBase.ATKspeed[Level - 1].ToString();
        PBFireRate.fillAmount = dataBase.ATKspeed[Level - 1] * 1f / dataBase.ATK[dataBase.ATKspeed.Count - 1];
        txtEXP.text = data1.EXP.ToString();
        PBEXP.fillAmount = data1.EXP * 1f / dataBase.Cost[dataBase.Cost.Count - 1];

        /// Set up Skill

    }

}
