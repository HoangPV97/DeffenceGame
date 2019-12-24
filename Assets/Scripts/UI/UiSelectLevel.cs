using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
public class UiSelectLevel : MonoBehaviour, IBaseUI
{
    public UISelectLevelItem[] uiSelectLevelItems;

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
        LevelDetail.SetActive(false);
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
        gameObject.SetActive(true);
        for (int i = 0; i < uiSelectLevelItems.Length; i++)
        {
            uiSelectLevelItems[i].SetUpData();
        }
    }

    public void SetUpDataUILevelDetail(int Level)
    {
        LevelDetail.SetActive(true);
        CurrentLevel = Level;
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
            else {
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
    public void OnHide()
    {
        gameObject.SetActive(false);
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
        SetUpData();
    }

    public void OnShowFromLeft(UnityAction unityAction = null)
    {
        throw new System.NotImplementedException();
    }

    public void OnShowFromRight(UnityAction unityAction = null)
    {
        throw new System.NotImplementedException();
    }

    public void ReSetUI()
    {
        throw new System.NotImplementedException();
    }

    public void UpDateData()
    {
        throw new System.NotImplementedException();
    }
    #endregion
}
