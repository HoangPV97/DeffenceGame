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
    public TextMeshProUGUI txtLevel;
    public int Value1, Value2, Value3;

    public UiUpgradeSkill UiUpgradeSkill;
    // Start is called before the first frame update
    void Start()
    {
        UISkillItems = GetComponentsInChildren<UISkillItem>();
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
        // SetUp info
        var sgd = DataController.Instance.GetArcheryData();
        var baseArchery = DataController.Instance.BaseDatabases.GetBaseArcheryData(sgd.Tier);
        Value1 = (int)baseArchery.GetAttributeValue("Damage", sgd.Level);
        Lb1.text = Language.GetKey("Damage");
        Lb2.text = Language.GetKey("Critical");
        lb3.text = Language.GetKey("KnockBack");
        txtLevel.text = "Lv." + sgd.Level;
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
    }

    public void OnTabTemplClick()
    {

    }
    public void OnTabFortressClick()
    {

    }
    public void OnUpgradeSkill(SkillData skillData, SaveGameTierLevel saveGameTierLevel)
    {
        UiUpgradeSkill.SetUpData(skillData, saveGameTierLevel);
    }

}
