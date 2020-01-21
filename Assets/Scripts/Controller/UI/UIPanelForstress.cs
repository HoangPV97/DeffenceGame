using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.UI;
using System;

public class UIPanelForstress : BaseUIView
{
    public UISkillItem[] UISkillItems;
    public Image Avatar;
    public UISkillItem currentSelectedUISkillItems = null;
    public int TabIndex = 0;
    public TabController TabController;
    public TextMeshProUGUI Lb1, Lb2, lb3;
    public TextMeshProUGUI txtValue1, txtValue2, txtValue3;
    public Image icon1, icon2, icon3;
    public Image PBValue1, PBValue2, PBValue3;
    public TextMeshProUGUI txtLevel, txtUpgradeGoldCost;
    public int Value1, Value2, Value3, Value1Max, Value2Max, Value3Max;
    public UiUpgradeSkill UiUpgradeSkill;
    public UIButton BtnUpgrade, BtnEvolve;
    public int GoldUpgradeCost;
    public UIEvolveFortress UIEvolveFortress;
    List<string> Skills;
    // Start is called before the first frame update
    void Start()
    {
        UISkillItems = GetComponentsInChildren<UISkillItem>();
        BtnUpgrade.SetUpEvent(OnBtnUpgradeClick);
        BtnEvolve.SetUpEvent(OnBtnEvolveClick);
    }

    private void OnBtnEvolveClick()
    {
        UIEvolveFortress.SetUpData();
    }

    private void OnBtnUpgradeClick()
    {
        switch (TabIndex)
        {
            case 1:
                if (GoldUpgradeCost <= DataController.Instance.Gold)
                {
                    DataController.Instance.Gold -= GoldUpgradeCost;
                    DataController.Instance.AddArcheryLevel();
                    OnTabArcheryClick();
                    DataController.Instance.Save();
                }
                //Check Gold
                break;
            case 2:
                if (GoldUpgradeCost <= DataController.Instance.Gold)
                {
                    DataController.Instance.Gold -= GoldUpgradeCost;
                    DataController.Instance.AddTempleLevel();
                    OnTabTemplClick();
                    DataController.Instance.Save();
                }
                //Check Gold
                break;
            case 3:
                if (GoldUpgradeCost <= DataController.Instance.Gold)
                {
                    DataController.Instance.Gold -= GoldUpgradeCost;
                    DataController.Instance.AddFortressLevel();
                    OnTabFortressClick();
                    DataController.Instance.Save();
                }
                //Check Gold
                break;
        }
    }

    public void SetUpData(int TabIndex)
    {
        this.TabIndex = TabIndex;
        if (UISkillItems.Length == 0)
        {
            UISkillItems = GetComponentsInChildren<UISkillItem>();
        }

    }

    public override void OnShowFromRight(UnityAction unityAction = null)
    {
        base.OnShowFromRight(unityAction);
        TabController.TabItems[0].OnTabClick();
    }

    public void OnSelectSkill(UISkillItem uISkillItem)
    {
        if (currentSelectedUISkillItems != null && currentSelectedUISkillItems != uISkillItem)
            currentSelectedUISkillItems.OnUnSelect();
        if (currentSelectedUISkillItems != uISkillItem)
            currentSelectedUISkillItems = uISkillItem;
    }

    public void OnTabArcheryClick()
    {
        TabIndex = 1;
        Lb1.text = Language.GetKey("Damage");
        Lb2.text = Language.GetKey("Critical");
        lb3.text = Language.GetKey("KnockBack");
        // SetUp info
        var sgd = DataController.Instance.GetArcheryGameData();
        var baseArchery = DataController.Instance.BaseDatabases.GetBaseArcheryData(sgd.Tier);
        Value1 = (int)baseArchery.GetAttributeValue("Damage", sgd.Level);
        txtLevel.text = "Lv." + sgd.Level;
        BtnUpgrade.gameObject.SetActive(true);

        if (sgd.Level == baseArchery.MaxLevel)
        {
            BtnEvolve.gameObject.SetActive(sgd.Tier < DataController.Instance.BaseDatabases.MaxTierArchery);
            BtnUpgrade.gameObject.SetActive(false);
        }
        else
        {
            BtnEvolve.gameObject.SetActive(false);
            GoldUpgradeCost = baseArchery.UpgradeLevelCost[sgd.Level - 1];
            txtUpgradeGoldCost.text = baseArchery.UpgradeLevelCost[sgd.Level - 1].ToString();
            LayoutRebuilder.ForceRebuildLayoutImmediate(txtUpgradeGoldCost.transform.parent.gameObject.GetComponent<RectTransform>());
        }

        Value1Max = (int)baseArchery.GetAttributeValue("Damage", baseArchery.MaxLevel);
        Value2Max = Value3Max = 100;
        //SetUp Skill
        Skills = DataController.Instance.GetArcherySkillID();
        if (UISkillItems.Length == 0)
        {
            UISkillItems = GetComponentsInChildren<UISkillItem>();
        }
        for (int i = 0; i < UISkillItems.Length; i++)
        {
            Debug.Log(Skills[i]);
            UISkillItems[i].SetUpdata(Skills[i], true);
        }
        //critical
        var sgd1 = DataController.Instance.GetGameSkillData(Skills[0]);
        var skillData1 = ConectingFireBase.Instance.GetSkillData(Skills[0]);
        if (sgd1.Level > 0)
        {
            Value2 = (int)skillData1.GetSkillAttributes("CriticalChance", sgd1.Tier, sgd1.Level);
        }
        else
        {
            Value2 = 0;
        }
        //KnockBack
        var sgd2 = DataController.Instance.GetGameSkillData(Skills[1]);
        var skillData2 = ConectingFireBase.Instance.GetSkillData(Skills[1]);
        if (sgd2.Level > 0)
        {
            Value3 = (int)skillData2.GetSkillAttributes("KnockBackChance", sgd2.Tier, sgd2.Level);
        }
        else
        {
            Value3 = 0;
        }
        SetTextValue();
        Avatar.sprite = DataController.Instance.DefaultData.LoadSprite("ICON_Archery_" + sgd.Tier);
        Avatar.SetNativeSize();
    }


    public void SetTextValue()
    {
        txtValue1.text = Value1.ToString();
        txtValue2.text = Value2.ToString();
        txtValue3.text = Value3.ToString();
        PBValue1.fillAmount = Value1 * 1.0f / Value1Max;
        PBValue2.fillAmount = Value2 * 1.0f / Value2Max;
        PBValue3.fillAmount = Value3 * 1.0f / Value3Max;
    }

    public void OnTabTemplClick()
    {
        TabIndex = 2;
        Lb1.text = Language.GetKey("Alliance_Damage");
        Lb2.text = Language.GetKey("Mana");
        lb3.text = Language.GetKey("ManaRegen");
        // SetUp info
        var sgd = DataController.Instance.GetTempleGameData();
        var baseTemple = DataController.Instance.BaseDatabases.GetBaseTempleData(sgd.Tier);
        Value1 = (int)baseTemple.GetAttributeValue("AllianceDamage", sgd.Level);
        Value2 = (int)baseTemple.GetAttributeValue("Mana", sgd.Level);
        Value3 = (int)baseTemple.GetAttributeValue("ManaRegen", sgd.Level);
        txtLevel.text = "Lv." + sgd.Level;
        BtnUpgrade.gameObject.SetActive(true);

        if (sgd.Level == baseTemple.MaxLevel)
        {
            BtnEvolve.gameObject.SetActive(sgd.Tier < DataController.Instance.BaseDatabases.MaxTierTemple);
            BtnUpgrade.gameObject.SetActive(false);
        }
        else
        {
            BtnEvolve.gameObject.SetActive(false);
            GoldUpgradeCost = baseTemple.UpgradeLevelCost[sgd.Level - 1];
            txtUpgradeGoldCost.text = baseTemple.UpgradeLevelCost[sgd.Level - 1].ToString();
            LayoutRebuilder.ForceRebuildLayoutImmediate(txtUpgradeGoldCost.transform.parent.gameObject.GetComponent<RectTransform>());
        }

        Value1Max = (int)baseTemple.GetAttributeValue("AllianceDamage", baseTemple.MaxLevel);
        Value3Max = (int)baseTemple.GetAttributeValue("ManaRegen", baseTemple.MaxLevel);

        //SetUp Skill
        Skills = DataController.Instance.GetTempleSkillID();
        if (UISkillItems.Length == 0)
        {
            UISkillItems = GetComponentsInChildren<UISkillItem>();
        }
        for (int i = 0; i < UISkillItems.Length; i++)
        {
            UISkillItems[i].SetUpdata(Skills[i], true);
        }
        //critical
        var sgd1 = DataController.Instance.GetGameSkillData(Skills[0]);
        var skillData1 = ConectingFireBase.Instance.GetSkillData(Skills[0]);
        if (sgd1.Level > 0)
        {
            Value2 += (int)skillData1.GetSkillAttributes("AddedMana", sgd1.Tier, sgd1.Level);
            Value2Max = (int)skillData1.GetSkillAttributes("AddedMana", sgd1.Tier, skillData1.baseSkills[0].MaxLevel);
        }
        //KnockBack
        var sgd2 = DataController.Instance.GetGameSkillData(Skills[1]);
        var skillData2 = ConectingFireBase.Instance.GetSkillData(Skills[1]);
        if (sgd2.Level > 0)
        {
            Value3 += (int)skillData2.GetSkillAttributes("AddedManaRegen", sgd2.Tier, sgd2.Level);
            Value3Max += (int)skillData2.GetSkillAttributes("AddedManaRegen", sgd2.Tier, sgd2.Level);
        }
        SetTextValue();
        Avatar.sprite = DataController.Instance.DefaultData.LoadSprite("ICON_Temple_" + sgd.Tier);
        Avatar.SetNativeSize();
    }
    public void OnTabFortressClick()
    {
        TabIndex = 3;
        Lb1.text = Language.GetKey("HP");
        Lb2.text = Language.GetKey("Shield");
        lb3.text = Language.GetKey("Repair");
        // SetUp info
        var sgd = DataController.Instance.GetFortressGameData();
        var baseTemple = DataController.Instance.BaseDatabases.GetBaseFortressData(sgd.Tier);
        Value1 = (int)baseTemple.GetAttributeValue("HP", sgd.Level);
        Value2 = (int)baseTemple.GetAttributeValue("Shield", sgd.Level);
        Value3 = (int)baseTemple.GetAttributeValue("HPRegen", sgd.Level);
        txtLevel.text = "Lv." + sgd.Level;
        BtnUpgrade.gameObject.SetActive(true);

        if (sgd.Level == baseTemple.MaxLevel)
        {
            BtnEvolve.gameObject.SetActive(sgd.Tier < DataController.Instance.BaseDatabases.MaxTierFortress);
            BtnUpgrade.gameObject.SetActive(false);
        }
        else
        {
            BtnEvolve.gameObject.SetActive(false);
            GoldUpgradeCost = baseTemple.UpgradeLevelCost[sgd.Level - 1];
            txtUpgradeGoldCost.text = baseTemple.UpgradeLevelCost[sgd.Level - 1].ToString();
            LayoutRebuilder.ForceRebuildLayoutImmediate(txtUpgradeGoldCost.transform.parent.gameObject.GetComponent<RectTransform>());
        }

        Value1Max = (int)baseTemple.GetAttributeValue("HP", baseTemple.MaxLevel);
        Value2Max = (int)baseTemple.GetAttributeValue("Shield", baseTemple.MaxLevel);
        Value3Max = (int)baseTemple.GetAttributeValue("HPRegen", baseTemple.MaxLevel);

        //SetUp Skill
        Skills = DataController.Instance.GetFortressSkillID();
        if (UISkillItems.Length == 0)
        {
            UISkillItems = GetComponentsInChildren<UISkillItem>();
        }
        for (int i = 0; i < UISkillItems.Length; i++)
        {
            Debug.Log(Skills[i]);
            UISkillItems[i].SetUpdata(Skills[i], true);
        }

        var sgd1 = DataController.Instance.GetGameSkillData(Skills[0]);
        var skillData1 = ConectingFireBase.Instance.GetSkillData(Skills[0]);
        if (sgd1.Level > 0)
        {
            Value2 += (int)skillData1.GetSkillAttributes("AddedShield", sgd1.Tier, sgd1.Level);
            Value2Max += (int)skillData1.GetSkillAttributes("AddedShield", sgd1.Tier, sgd1.Level);
        }

        var sgd2 = DataController.Instance.GetGameSkillData(Skills[1]);
        var skillData2 = ConectingFireBase.Instance.GetSkillData(Skills[1]);
        if (sgd2.Level > 0)
        {
            Value3 += (int)skillData2.GetSkillAttributes("AddedHPRegen", sgd2.Tier, sgd2.Level);
            Value3Max += (int)skillData2.GetSkillAttributes("AddedHPRegen", sgd2.Tier, sgd2.Level);
        }

        var sgd3 = DataController.Instance.GetGameSkillData(Skills[3]);
        var skillData3 = ConectingFireBase.Instance.GetSkillData(Skills[3]);
        if (sgd3.Level > 0)
        {
            Value1 += (int)skillData3.GetSkillAttributes("AddedHP", sgd3.Tier, sgd3.Level);
            Value1Max += (int)skillData3.GetSkillAttributes("AddedHP", sgd3.Tier, sgd3.Level);

            Value2 += (int)skillData3.GetSkillAttributes("AddedShield", sgd3.Tier, sgd3.Level);
            Value2Max += (int)skillData3.GetSkillAttributes("AddedShield", sgd3.Tier, sgd3.Level);
        }
        SetTextValue();
        Avatar.sprite = DataController.Instance.DefaultData.LoadSprite("ICON_Fortress_" + sgd.Tier);
        Avatar.SetNativeSize();
    }
    public void OnUpgradeSkill(SkillData skillData, SaveGameTierLevel saveGameTierLevel)
    {
        UiUpgradeSkill.SetUpData(skillData, saveGameTierLevel);
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
        if (SkillID == "BASE_ARCHERY_SKILL_1")
        {
            var sgd1 = DataController.Instance.GetGameSkillData(SkillID);
            var skillData1 = ConectingFireBase.Instance.GetSkillData(SkillID);
            Value2 = (int)skillData1.GetSkillAttributes("CriticalChance", sgd1.Tier, sgd1.Level);
        }
        else if (SkillID == "BASE_ARCHERY_SKILL_2")
        {
            var sgd2 = DataController.Instance.GetGameSkillData(SkillID);
            var skillData2 = ConectingFireBase.Instance.GetSkillData(SkillID);
            Value3 = (int)skillData2.GetSkillAttributes("KnockBackChance", sgd2.Tier, sgd2.Level);
        }
        else if (SkillID == "BASE_TEMPLE_SKILL_1")
        {
            var sgd2 = DataController.Instance.GetGameSkillData(SkillID);
            var skillData2 = ConectingFireBase.Instance.GetSkillData(SkillID);
            int a = (int)skillData2.GetSkillAttributes("AddedMana", sgd2.Tier, sgd2.Level) - (int)skillData2.GetSkillAttributes("AddedMana", sgd2.Tier, sgd2.Level - 1);
            Value2 += a;
        }
        else if (SkillID == "BASE_TEMPLE_SKILL_2")
        {
            var sgd2 = DataController.Instance.GetGameSkillData(SkillID);
            var skillData2 = ConectingFireBase.Instance.GetSkillData(SkillID);
            int a = (int)skillData2.GetSkillAttributes("AddedManaRegen", sgd2.Tier, sgd2.Level) - (int)skillData2.GetSkillAttributes("AddedMana", sgd2.Tier, sgd2.Level - 1);
            Value3 += a;
            Value3Max += a;
        }
        else if (SkillID == "BASE_FORTRESS_SKILL_1")
        {
            var sgd2 = DataController.Instance.GetGameSkillData(SkillID);
            var skillData2 = ConectingFireBase.Instance.GetSkillData(SkillID);
            int a = (int)skillData2.GetSkillAttributes("AddedShield", sgd2.Tier, sgd2.Level) - (int)skillData2.GetSkillAttributes("AddedShield", sgd2.Tier, sgd2.Level - 1);
            Value2 += a;
            Value2Max += a;
        }
        else if (SkillID == "BASE_FORTRESS_SKILL_2")
        {
            var sgd2 = DataController.Instance.GetGameSkillData(SkillID);
            var skillData2 = ConectingFireBase.Instance.GetSkillData(SkillID);
            int a = (int)skillData2.GetSkillAttributes("AddedHPRegen", sgd2.Tier, sgd2.Level) - (int)skillData2.GetSkillAttributes("AddedHPRegen", sgd2.Tier, sgd2.Level - 1);
            Value3 += a;
            Value3Max += a;
        }

        SetTextValue();
    }
}
