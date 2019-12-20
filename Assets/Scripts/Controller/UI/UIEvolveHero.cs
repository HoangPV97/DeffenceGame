using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIEvolveHero : MonoBehaviour
{
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
    }
    void OnBtnCloseClick()
    {
        gameObject.SetActive(false);
    }
    public void SetUpData(Elemental elemental)
    {
        heroElemental = elemental;
        gameObject.SetActive(true);
        if (IsHero)
        {
            data1 = DataController.Instance.GetGameDataWeapon(elemental);
            dataBase = DataController.Instance.GetDataBaseWeapons(elemental, data1.WeaponTierLevel.Tier);
            dataBase2 = DataController.Instance.GetDataBaseWeapons(elemental, data1.WeaponTierLevel.Tier + 1);
            txtHeroName1.text = Language.GetKey("Name_" + elemental.ToString()) + "." + ToolHelper.ToRoman(data1.WeaponTierLevel.Tier);
            txtHeroName2.text = Language.GetKey("Name_" + elemental.ToString()) + "." + ToolHelper.ToRoman(data1.WeaponTierLevel.Tier + 1);
        }
        else
        {
            data1 = DataController.Instance.GetGameAlliance(elemental);
            dataBase = DataController.Instance.GetAllianceDataBases(elemental, data1.WeaponTierLevel.Tier).weapons;
            dataBase2 = DataController.Instance.GetAllianceDataBases(elemental, data1.WeaponTierLevel.Tier + 1).weapons;
            txtHeroName1.text = Language.GetKey("Name_Alliance_" + elemental.ToString()) + "." + ToolHelper.ToRoman(data1.WeaponTierLevel.Tier);
            txtHeroName2.text = Language.GetKey("Name_Alliance_" + elemental.ToString()) + "." + ToolHelper.ToRoman(data1.WeaponTierLevel.Tier + 1);
        }
        int Level = data1.WeaponTierLevel.Level > 0 ? data1.WeaponTierLevel.Level : 1;
        txtDamage1.text = dataBase.ATK[Level - 1].ToString();
        txtFireRate1.text = dataBase.ATKspeed[Level - 1].ToString();
        txtDamage2.text = dataBase2.ATK[0] > dataBase.ATK[Level - 1] ? string.Format("<color=#65FF00FF>{0}</color>", dataBase2.ATK[0]) : dataBase2.ATK[0].ToString();
        txtFireRate2.text = dataBase2.ATKspeed[0] > dataBase.ATKspeed[Level - 1] ? string.Format("<color=#65FF00FF>{0}</color>", dataBase2.ATKspeed[0]) : dataBase2.ATKspeed[0].ToString();
    }
}
