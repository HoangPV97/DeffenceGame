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
            BtnEvolve.gameObject.SetActive(sgd.Tier < 3);
            BtnUpgrade.gameObject.SetActive(false);
        }
        else
        {
            BtnEvolve.gameObject.SetActive(false);
            GoldUpgradeCost = baseArchery.UpgradeLevelCost[sgd.Level - 1];
            txtUpgradeGoldCost.text = baseArchery.UpgradeLevelCost[sgd.Level - 1].ToString();
        }

        Value1Max = (int)baseArchery.GetAttributeValue("Damage", baseArchery.MaxLevel);
        Value2Max = Value3Max = 100;
        //SetUp Skill
        var Skills = DataController.Instance.GetArcherySkillID();
        if (UISkillItems.Length == 0)
        {
            UISkillItems = GetComponentsInChildren<UISkillItem>();
        }
        for (int i = 0; i < UISkillItems.Length; i++)
        {
            Debug.Log(Skills[i]);
            UISkillItems[i].SetUpdata(Skills[i], true);
        }
        SetTextValue();
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
    }
    public void OnTabFortressClick()
    {
        TabIndex = 3;
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
    }
}
