using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
public class UiSelectLevel : BaseUIView
{
    public UISelectLevelItem[] uiSelectLevelItems;
    public Animator anim;
    [Header("Level Detail")]
    public GameObject LevelDetail;
    public TextMeshProUGUI txtLevel;
    public GameObject[] Star, HardMode;
    public UIButton BtnPlay;
    public UiItem pfUIItem;
    public UIButton BtnCloseLevelDetail;
    public Transform ItemContain;
    int CurrentLevel;
    void Awake()
    {
        uiSelectLevelItems = GetComponentsInChildren<UISelectLevelItem>();
        BtnPlay.SetUpEvent(BtnPlayClick);
        BtnCloseLevelDetail.SetUpEvent(OnBtnCloseLevelDetailClick);
    }
    void OnBtnCloseLevelDetailClick()
    {
        OnLevelDetailHide();
    }
    public UISelectLevelItem GetUISelectLevelItem(int Level)
    {
        for (int i = 0; i < uiSelectLevelItems.Length; i++)
            if (uiSelectLevelItems[i].Level == Level)
                return uiSelectLevelItems[i];
        return null;
    }

    public void SetUpData()
    {
        CurrentLevel = DataController.Instance.CurrentSelected;
        gameObject.SetActive(true);
        for (int i = 0; i < uiSelectLevelItems.Length; i++)
        {
            uiSelectLevelItems[i].SetUpData();
            uiSelectLevelItems[i].OnShowAnimation(uiSelectLevelItems[i].Level == CurrentLevel);
        }
    }


    public void SetUpDataUILevelDetail(int Level)
    {
        OnLevelDetailShow();
        GetUISelectLevelItem(CurrentLevel).OnShowAnimation(false);
        CurrentLevel = Level;
        GetUISelectLevelItem(CurrentLevel).OnShowAnimation(true);
        foreach (Transform child in ItemContain)
        {
            Destroy(child.gameObject);
        }
        var gameStage = DataController.Instance.GetGameStage(Level);
        for (int i = 0; i < Star.Length; i++)
        {
            Star[i].SetActive(gameStage.HardMode >= i + 2);
        }
        for (int i = 0; i < HardMode.Length; i++)
        {
            HardMode[i].SetActive(gameStage.HardMode == i + 1);
        }
        var gsd = DataController.Instance.GetStageDataBase(Level);
        var listItem = gsd.WinReward[gameStage.HardMode - 1];
        for (int i = 0; i < listItem.items.Count; i++)
        {
            if (listItem.items[i].Type != ITEM_TYPE.coin)
            {
                var it = Instantiate(pfUIItem.gameObject, Vector3.zero, Quaternion.identity);
                it.SetActive(true);
                it.transform.SetParent(ItemContain);
                it.transform.SetDefaultTransform();
                it.GetComponent<UiItem>().SetUpData(listItem.items[i], 1);
            }
            else
            {
                DataController.Instance.GoldInGame = listItem.items[i].Quality;
            }
        }
    }
    void BtnPlayClick()
    {
        DataController.Instance.CurrentSelected = CurrentLevel;
        MenuController.Instance.OnBtnPlayClick();
    }
    #region Animation


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

    public void OnLevelDetailHide()
    {
        anim.SetTrigger("HidePanelSetting");
        DG.Tweening.DOVirtual.DelayedCall(0.3f, () =>
        {
            LevelDetail.SetActive(false);
        }, false);
    }
    public void OnLevelDetailShow()
    {
        LevelDetail.SetActive(true);
        anim.SetTrigger("ShowPanelSetting");
    }

    #endregion
}
