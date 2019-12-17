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
    public TextMeshProUGUI txtHeroName;
    public TextMeshProUGUI txtDamage, txtFireRate, txtEXP;
    public Image PBDamage, PBFireRate, PBEXP;
    public Image ElementalIcon;
    public UIHeroItem[] UIHeroItems;
    [Header("Alliance")]
    public GameObject PanelAlliance;

    #region Animation
    public void OnHide()
    {
        throw new System.NotImplementedException();
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
            if (UIHeroItems[i].elemental == DataController.Instance.GameData.CurrentSelectedWeapon)
            {

            }
        }
    }
}
