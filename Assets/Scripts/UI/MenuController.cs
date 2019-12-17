using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using InviGiant.Tools;
public class MenuController : Singleton<MenuController>
{
    public UIPanelHeroAlliance UIPanelHeroAlliance;
    // Start is called before the first frame update
    void Start()
    {
        BottomBarController.Instance.BotUIButton[3].SetUpEvent(OnBtnHeroClick);
    }

    public void OnBtnPlayClick()
    {
        DataController.Instance.CurrentSelected = 1;
        DataController.Instance.LoadIngameStage();
        SceneManager.LoadScene(2);
    }

    public void OnBtnHeroClick()
    {
        UIPanelHeroAlliance.SetUpData();
    }

}
