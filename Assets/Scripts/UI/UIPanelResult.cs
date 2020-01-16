using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UIPanelResult : BaseUIView
{
    public Animator AnimVictory, AnimFailed;
    public TextMeshProUGUI[] txtKills, txtLife, txtCoin;
    public UiItem pfUIItem;
    public UIButton BtnClose;
    public Transform ItemContain;
    public int index;
    private void Start()
    {
        BtnClose.SetUpEvent(OnBtnCloseCLick);
    }

    private void OnBtnCloseCLick()
    {
        if (index == 0)
        {
            AnimVictory.SetTrigger("HidePanelSetting");
            DG.Tweening.DOVirtual.DelayedCall(0.3f, () =>
            {
                gameObject.SetActive(false);
            }, false);
        }
        else
        {
            AnimFailed.SetTrigger("HidePanelSetting");
            DG.Tweening.DOVirtual.DelayedCall(0.3f, () =>
            {
                gameObject.SetActive(false);
            }, false);
        }
        GameController.Instance.Restart();
    }

    // Start is called before the first frame update
    public void SetUpdataVictory(int KillNumber, int LifePercent, int Coin)
    {
        txtKills[0].text = KillNumber.ToString();
        txtLife[0].text = LifePercent.ToString() + " %";
        txtCoin[0].text = Coin.ToString();
        index = 0;
        foreach (Transform child in ItemContain)
        {
            Destroy(child.gameObject);
        }
        int Level = DataController.Instance.CurrentSelected;
        var gsd = DataController.Instance.GetStageDataBase(Level);
        var gameStage = DataController.Instance.GetGameStage(Level);
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
        }
        gameObject.SetActive(true);
        AnimVictory.gameObject.SetActive(true);
        AnimVictory.SetTrigger("ShowPanelSetting");
    }

    public void SetUpDataFailed(int KillNumber, int LifePercent, int Coin)
    {
        txtKills[1].text = KillNumber.ToString();
        txtLife[1].text = LifePercent.ToString();
        txtCoin[1].text = Coin.ToString();
        index = 1;
        gameObject.SetActive(true);
        AnimFailed.gameObject.SetActive(true);
        AnimFailed.SetTrigger("ShowPanelSetting");
    }
}
