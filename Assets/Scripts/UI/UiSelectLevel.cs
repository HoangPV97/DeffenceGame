using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.UI;
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
    public InputField InputField;
    public RectTransform SelectContent;
    public float DeltaY;
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
        return uiSelectLevelItems[0];
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
        SetSelectItem2Center();
    }


    public void HackLevelClick()
    {
        SetUpDataUILevelDetail(int.Parse(InputField.text));
    }

    public void SetUpDataUILevelDetail(int Level)
    {
        OnLevelDetailShow();
        var uiLv = GetUISelectLevelItem(CurrentLevel);
        if (uiLv != null)
            uiLv.OnShowAnimation(false);

        CurrentLevel = Level;

        var uiLv2 = GetUISelectLevelItem(CurrentLevel);
        if (uiLv2 != null)
            uiLv2.OnShowAnimation(true);
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

        SetSelectItem2Center();
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

    public void SetSelectItem2Center()
    {
        var it = GetUISelectLevelItem(CurrentLevel);
        float Height = -SelectContent.rect.height;
        float raito = Screen.height / 2 - DeltaY;
        Vector2 pos = new Vector2(0, Height - it.transform.parent.GetComponent<RectTransform>().anchoredPosition.y - it.GetComponent<RectTransform>().anchoredPosition.y + raito);
        SelectContent.anchoredPosition = pos;
    }
}
