using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.UI;
using System;
public class UIPanelAchievement : BaseUIView
{
    public List<UIItemAchievement> uIItemAchievements;
    public RectTransform contain;

    public override void OnShowFromLeft(UnityAction unityAction = null)
    {
        base.OnShowFromLeft();
        SetUpData();
    }

    public override void OnShowFromRight(UnityAction unityAction = null)
    {
        base.OnShowFromRight();
        SetUpData();
    }
    public void SetUpData()
    {
        if (uIItemAchievements == null || uIItemAchievements.Count == 0)
        {
            uIItemAchievements.AddRange(GetComponentsInChildren<UIItemAchievement>());
        }
        for (int i = 0; i < uIItemAchievements.Count; i++)
        {
            uIItemAchievements[i].SetUpData();
        }
        contain.anchoredPosition = Vector2.zero;
    }
}
