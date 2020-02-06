using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using InviGiant.Tools;
using UnityEngine.Events;
using TMPro;
public class MenuController : Singleton<MenuController>
{
    public UIPanelHeroAlliance UIPanelHeroAlliance;
    public UiSelectLevel UiSelectLevel;
    public UIPanelForstress UIPanelForstress;
    public UIPanelAchievement UIPanelInventory;
    public UIPanelShop UIPanelShop;
    public Stack<UITYPE> UIType = new Stack<UITYPE>();
    public UIDailyQuest UIDailyQuest;
    public UIUnlockWeaponAlliance UIUnlockWeaponAlliance;
    public UIGacha UIGacha;
    public UIButton BtnGacha;
    public UIButton BtnQuest;
    public UIReward UIReward;
    public TextMeshProUGUI txtGold, txtGem;
    bool _GachafailUpgrade;
    public bool GachafailUpgrade
    {
        set
        {
            _GachafailUpgrade = value;
            if (value)
                CheckEnableGacha();
        }
        get
        {
            return _GachafailUpgrade;
        }
    }
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
        BtnQuest.SetUpEvent(OnBtnQuestClick);
        CheckUNLOCK_UI();
        ResetTextGem();
        ResetTextGold();
        BtnGacha.SetUpEvent(OnBtnGachaClick);
        CheckEnableGacha();
    }

    public void CheckEnableGacha()
    {
        StopAllCoroutines();
        bool enable = true;
        enable &= DataController.Instance.HighestLevelMode1 >= 5;
        float ti = DataController.Instance.CheckLastPlayGacha();
        if (enable)
        {
            if (ti >= 0)
            {
                BtnGacha.gameObject.SetActive(true);
                return;
            }
            else
            {
                StartCoroutine(RepeatCheckBtnGacha(1 - ti));
            }
            if (DataController.Instance.ShowGachaFail)
            {
                BtnGacha.gameObject.SetActive(true);
                return;
            }
            if (DataController.Instance.WinGachaCount >= 2)
            {
                BtnGacha.gameObject.SetActive(true);
                return;
            }
        }
    }
    IEnumerator RepeatCheckBtnGacha(float time)
    {
        yield return new WaitForSeconds(time);
        CheckEnableGacha();
    }

    public void HideBtnGacha()
    {
        BtnGacha.gameObject.SetActive(false);
        CheckEnableGacha();
    }

    void CheckUNLOCK_UI()
    {
        var str = DataController.Instance.GetStringUNLOCK_UI();
        if (str != null && str != "NONE")
        {
            UIUnlockWeaponAlliance.SetUpData(str);
        }
    }
    public BaseUIView GetBaseUIView(UITYPE uITYPE)
    {
        switch (uITYPE)
        {
            case UITYPE.none:
                break;
            case UITYPE.shop:
                return UIPanelShop;
            case UITYPE.achievement:
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

    public void OnShowReward(List<Item> items)
    {
        UIReward.SetUpData(items);
    }

    public void OnBtnGachaClick()
    {
        UIGacha.OnShow();
    }

    public void OnBtnPlayClick()
    {
        if (DataController.Instance.Energy >= 1)
        {
            DataController.Instance.Energy--;
            DataController.Instance.LoadIngameStage();
            SceneManager.LoadScene(2);
        }
        else {
            Debug.Log("Not Enough energy");
        }
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
    public void OnBtnQuestClick()
    {
        UIDailyQuest.SetUpData();
    }
    public void ResetTextGold()
    {
        txtGold.text = DataController.Instance.Gold.ToString();
    }
    public void ResetTextGem()
    {
        txtGem.text = DataController.Instance.Gem.ToString();
    }
}
