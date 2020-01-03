using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using InviGiant.Tools;
using UnityEngine.Events;

public class MenuController : Singleton<MenuController>
{
    public UIPanelHeroAlliance UIPanelHeroAlliance;
    public UiSelectLevel UiSelectLevel;
    public UIPanelForstress UIPanelForstress;
    public UIPanelInventory UIPanelInventory;
    public UIPanelShop UIPanelShop;
    public Stack<UITYPE> UIType = new Stack<UITYPE>();
    public UITYPE CurrentUITYPE
    {
        get
        {
            if (UIType.Count == 0)
            {
                UIType.Push(UITYPE.selectLevel);
            }
            return UIType.Peek();

        }
    }
    public BaseUIView CurrentBaseUIView
    {
        get
        {
            return GetBaseUIView(CurrentUITYPE);
        }
    }

    // Start is called before the first frame update
    IBaseUI CurrentUI
    {
        get
        {
            if (UIPanelHeroAlliance.gameObject.activeSelf)
                return UIPanelHeroAlliance;
            if (UiSelectLevel.gameObject.activeSelf)
                return UiSelectLevel;
            return null;
        }
    }
    void Start()
    {
        BottomBarController.Instance.BotUIButton[2].SetUpEvent(OnBtnMainPlayClick);
        BottomBarController.Instance.BotUIButton[3].SetUpEvent(OnBtnHeroClick);
    }
    public BaseUIView GetBaseUIView(UITYPE uITYPE)
    {
        switch (uITYPE)
        {
            case UITYPE.none:
                break;
            case UITYPE.shop:
                return UIPanelShop;
            case UITYPE.inventory:
                return UIPanelInventory;
            case UITYPE.selectLevel:
                return UiSelectLevel;
            case UITYPE.heroAlliance:
                return UIPanelHeroAlliance;
            case UITYPE.fortress:
                return UIPanelForstress;
        }
        return null;
    }
    public void OnBtnPlayClick()
    {
        DataController.Instance.LoadIngameStage();
        SceneManager.LoadScene(2);
    }

    public void OnBtnHeroClick()
    {
        UIPanelHeroAlliance.TabController.TabItems[0].OnTabClick();
    }

    public void OnBtnMainPlayClick()
    {
        UiSelectLevel.SetUpData();
    }
    public void OnHideLeftCurrentUI(UnityAction callBack = null)
    {
        CurrentBaseUIView.OnHideLeft();
        if (callBack != null)
            callBack();
        UIType.Pop();
    }
    public void OnShowFromRight(UITYPE uITYPE, UnityAction callBack = null)
    {
        UIType.Push(uITYPE);
        GetBaseUIView(uITYPE).OnShowFromRight();
        if (callBack != null)
            callBack();
    }

    public void OnShowFromLeft(UITYPE uITYPE, UnityAction callBack = null)
    {
        UIType.Push(uITYPE);
        GetBaseUIView(uITYPE).OnShowFromLeft();
        if (callBack != null)
            callBack();
    }
    public void OnHideRightCurrentUI(UnityAction callBack = null)
    {
        CurrentBaseUIView.OnHideRight();
        if (callBack != null)
            callBack();
        UIType.Pop();
    }

}
