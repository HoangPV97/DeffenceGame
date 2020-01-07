using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIEvolveFortress : BaseUIView
{
    public Animator Anim;
    public TextMeshProUGUI txtHeroName1, txtHeroName2;
    public TextMeshProUGUI[] lb, txtValue;
    public TextMeshProUGUI GoldCost;
    public Transform ItemContain;
    public UIButton BtnEvolve, BtnClose;
    public UiItem pfUIItem;
    SaveGameTierLevel data1;
    BaseDatabase dataBase;
    BaseDatabase dataBase2;

    int Gold = 0;
    [SerializeField]
    bool canEvolve = true;
    List<Item> listItem;
    public int TabIndex
    {
        get
        {
            return MenuController.Instance.UIPanelForstress.TabIndex;
        }
    }
    private void Start()
    {
        BtnClose.SetUpEvent(OnBtnCloseClick);
        BtnEvolve.SetUpEvent(OnBtnEvolveClick);
    }

    void OnBtnEvolveClick()
    {
        if (canEvolve)
        {
            switch (TabIndex)
            {
                case 1:
                    DataController.Instance.AddArcheryTier();
                    break;
                case 2:
                    DataController.Instance.AddTempleTier();
                    break;
                case 3:
                    DataController.Instance.AddFortressTier();
                    break;
            }

            DataController.Instance.Gold -= Gold;
            for (int i = 0; i < listItem.Count; i++)
            {
                if (listItem[i].Type != ITEM_TYPE.None)
                {
                    DataController.Instance.AddItemQuality(listItem[i].Type, -listItem[i].Quality);
                }
            }
            //   MenuController.Instance.UIPanelHeroAlliance.SetupUIHero(heroElemental);
            DataController.Instance.Save();
            OnBtnCloseClick();
            // SetUpData(heroElemental);
        }
    }

    void OnBtnCloseClick()
    {
        OnHide();
    }
    public void SetUpData()
    {
        OnShow();
        canEvolve = true;
        Gold = 0;
        switch (TabIndex)
        {
            case 1:
                data1 = DataController.Instance.GetArcheryGameData();
                dataBase = DataController.Instance.BaseDatabases.GetBaseArcheryData(data1.Tier);
                dataBase2 = DataController.Instance.BaseDatabases.GetBaseArcheryData(data1.Tier + 1);
                break;
            case 2:
                data1 = DataController.Instance.GetTempleGameData();
                dataBase = DataController.Instance.BaseDatabases.GetBaseTempleData(data1.Tier);
                dataBase2 = DataController.Instance.BaseDatabases.GetBaseTempleData(data1.Tier + 1);
                break;
            case 3:
                data1 = DataController.Instance.GetFortressGameData();
                dataBase = DataController.Instance.BaseDatabases.GetBaseFortressData(data1.Tier);
                dataBase2 = DataController.Instance.BaseDatabases.GetBaseFortressData(data1.Tier + 1);
                break;
        }

        int Level = data1.Level > 0 ? data1.Level : 1;
        /*  txtDamage1.text = dataBase.ATK[Level - 1].ToString();
          txtFireRate1.text = dataBase.ATKspeed[Level - 1].ToString();
          txtDamage2.text = dataBase2.ATK[0] > dataBase.ATK[Level - 1] ? string.Format("<color=#65FF00FF>{0}</color>", dataBase2.ATK[0]) : dataBase2.ATK[0].ToString();
          txtFireRate2.text = dataBase2.ATKspeed[0] > dataBase.ATKspeed[Level - 1] ? string.Format("<color=#65FF00FF>{0}</color>", dataBase2.ATKspeed[0]) : dataBase2.ATKspeed[0].ToString();
          */
        /// init Item
        foreach (Transform child in ItemContain)
        {
            Destroy(child.gameObject);
        }
        listItem = dataBase.CostEvolution;
        for (int i = 0; i < listItem.Count; i++)
        {
            var it = Instantiate(pfUIItem.gameObject, Vector3.zero, Quaternion.identity);
            it.SetActive(true);
            it.transform.SetParent(ItemContain);
            it.transform.SetDefaultTransform();
            var itInInventory = DataController.Instance.GetGameItemData(listItem[i].Type);
            it.GetComponent<UiItem>().SetUpData(listItem[i], itInInventory.Quality, 2);
            var itDataBase = DataController.Instance.GetItemDataBase(listItem[i].Type);
            Gold += listItem[i].Quality * itDataBase.UseGoldCost;
            if (listItem[i].Quality > itInInventory.Quality)
                canEvolve = false;
        }
        if (Gold > DataController.Instance.Gold)
            canEvolve = false;
        GoldCost.text = Gold.ToString();
    }
    public override void OnHide()
    {
        base.OnHide();
        Anim.SetTrigger("HidePanelSetting");
        DG.Tweening.DOVirtual.DelayedCall(0.3f, () =>
        {
            gameObject.SetActive(false);
        }, false);
    }
    public override void OnShow()
    {
        gameObject.SetActive(true);
        Anim.SetTrigger("ShowPanelSetting");
    }
}
