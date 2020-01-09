using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIEvolveFortress : BaseUIView
{
    public Animator Anim;
    public TextMeshProUGUI txtHeroName1, txtHeroName2, PopUpTitle;
    public TextMeshProUGUI[] lb, txtValue, lb2, txtValue2;
    public TextMeshProUGUI GoldCost;
    public Transform ItemContain;
    public UIButton BtnEvolve, BtnClose;
    public UiItem pfUIItem;
    SaveGameTierLevel data1;
    [SerializeField]
    BaseDatabase dataBase;
    [SerializeField]
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
                    MenuController.Instance.UIPanelForstress.OnTabArcheryClick();
                    break;
                case 2:
                    DataController.Instance.AddTempleTier();
                    MenuController.Instance.UIPanelForstress.OnTabTemplClick();
                    break;
                case 3:
                    DataController.Instance.AddFortressTier();
                    MenuController.Instance.UIPanelForstress.OnTabFortressClick();
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
                txtHeroName1.text = Language.GetKey("Archery") + "." + ToolHelper.ToRoman(data1.Tier);
                txtHeroName2.text = Language.GetKey("Archery") + "." + ToolHelper.ToRoman(data1.Tier + 1);
                PopUpTitle.text = Language.GetKey("Evolve_Archery_Title");
                break;
            case 2:
                data1 = DataController.Instance.GetTempleGameData();
                dataBase = DataController.Instance.BaseDatabases.GetBaseTempleData(data1.Tier);
                dataBase2 = DataController.Instance.BaseDatabases.GetBaseTempleData(data1.Tier + 1);
                txtHeroName1.text = Language.GetKey("Temple") + "." + ToolHelper.ToRoman(data1.Tier);
                txtHeroName2.text = Language.GetKey("Temple") + "." + ToolHelper.ToRoman(data1.Tier + 1);
                PopUpTitle.text = Language.GetKey("Evolve_Temple_Title");
                break;
            case 3:
                data1 = DataController.Instance.GetFortressGameData();
                dataBase = DataController.Instance.BaseDatabases.GetBaseFortressData(data1.Tier);
                dataBase2 = DataController.Instance.BaseDatabases.GetBaseFortressData(data1.Tier + 1);
                txtHeroName1.text = Language.GetKey("Fortress") + "." + ToolHelper.ToRoman(data1.Tier);
                txtHeroName2.text = Language.GetKey("Fortress") + "." + ToolHelper.ToRoman(data1.Tier + 1);
                PopUpTitle.text = Language.GetKey("Evolve_Fortress_Title");
                break;
        }

        int Level = data1.Level > 0 ? data1.Level : 1;
        //init attribute
        for (int i = 0; i < 3; i++)
        {
            lb[i].gameObject.SetActive(false);
            lb2[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < dataBase.Attributes.Count; i++)
        {
            if (i < 3)
            {
                lb[i].gameObject.SetActive(true);
                lb[i].text = Language.GetKey(dataBase.Attributes[i].Attribute);
                txtValue[i].text = dataBase.GetAttributeValue(dataBase.Attributes[i].Attribute, data1.Level).ToString();
            }
        }

        for (int i = 0; i < dataBase2.Attributes.Count; i++)
        {
            if (i < 3)
            {
                lb2[i].gameObject.SetActive(true);
                lb2[i].text = Language.GetKey(dataBase2.Attributes[i].Attribute);
                txtValue2[i].text = dataBase2.GetAttributeValue(dataBase2.Attributes[i].Attribute, 1).ToString();
            }
        }
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
