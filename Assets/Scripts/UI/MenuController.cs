using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using InviGiant.Tools;
public class MenuController : Singleton<MenuController>
{
    public UIPanelHeroAlliance UIPanelHeroAlliance;
    public UiSelectLevel UiSelectLevel;
    // Start is called before the first frame update
    IBaseUI CurrentUI
    {
        get
        {
            if (UIPanelHeroAlliance.gameObject.activeSelf)
                return UIPanelHeroAlliance;
            return null;
        }
    }
    void Start()
    {
        BottomBarController.Instance.BotUIButton[2].SetUpEvent(OnBtnMainPlayClick);
        BottomBarController.Instance.BotUIButton[3].SetUpEvent(OnBtnHeroClick);
    }

    public void OnBtnPlayClick()
    {
        DataController.Instance.LoadIngameStage();
        SceneManager.LoadScene(2);
    }

    public void OnBtnHeroClick()
    {
        if (CurrentUI != null)
            CurrentUI.OnHide();
        UIPanelHeroAlliance.SetUpData();
    }

    public void OnBtnMainPlayClick()
    {
        if (CurrentUI != null)
            CurrentUI.OnHide();
        UiSelectLevel.SetUpData();
    }
}
