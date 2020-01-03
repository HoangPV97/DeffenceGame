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


}
