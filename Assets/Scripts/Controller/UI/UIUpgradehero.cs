using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIUpgradehero : MonoBehaviour
{
    public TextMeshProUGUI txtHeroName, txtHeroLevel;
    public TextMeshProUGUI txtDamage, txtFireRate, txtEXP;
    public Image PBDamage, PBFireRate, PBEXP;
    public Image ElementalIcon;
    public UIButton BtnClose;
    public UiItem[] uiItems;
    public ITEM_TYPE[] Slots = new ITEM_TYPE[] { ITEM_TYPE.None, ITEM_TYPE.None, ITEM_TYPE.None, ITEM_TYPE.None, ITEM_TYPE.None };
    public Image[] ImgSlot;
    public Sprite sprEmptySlot;
    public UIButton[] BtnSlot;
    public Dictionary<ITEM_TYPE, ItemData> ItemDatabase;
    public float currentExp, AddExp;
    public float MaxExp;
    public int GoldCost = 0;
    public Elemental heroElemental;
    public UIButton BtnAuto, BtnUpgrade;
    public TextMeshProUGUI txtGoldCost;
    GameDataWeapon data1;
    Weapons dataBase;
    int RemainEXP;
    int addLevel = 0;
    public int EmptySlot
    {
        get
        {
            for (int i = 0; i < Slots.Length; i++)
                if (Slots[i] == ITEM_TYPE.None)
                    return i;
            return 999;
        }
    }

    public bool HasNotEmptySlot
    {
        get
        {
            for (int i = 0; i < Slots.Length; i++)
                if (Slots[i] != ITEM_TYPE.None)
                    return true;
            return false;
        }
    }
    private void Awake()
    {
        uiItems = GetComponentsInChildren<UiItem>();

    }
    void Start()
    {
        BtnClose.SetUpEvent(OnHide);
        for (int i = 0; i < ImgSlot.Length; i++)
        {
            BtnSlot[i] = ImgSlot[i].gameObject.GetComponent<UIButton>();
        }
        BtnSlot[0].SetUpEvent(() => { ResetSlot(0); });
        BtnSlot[1].SetUpEvent(() => { ResetSlot(1); });
        BtnSlot[2].SetUpEvent(() => { ResetSlot(2); });
        BtnSlot[3].SetUpEvent(() => { ResetSlot(3); });
        BtnSlot[4].SetUpEvent(() => { ResetSlot(4); });
        BtnAuto.SetUpEvent(OnAutoSelectClick);
        BtnUpgrade.SetUpEvent(OnBtnUpgradetClick);
    }
    public void OnHide()
    {
        gameObject.SetActive(false);
    }

    public void SetUpData(Elemental elemental)
    {
        heroElemental = elemental;
        gameObject.SetActive(true);
        txtHeroName.text = Language.GetKey("Name_" + elemental.ToString());
        data1 = DataController.Instance.GetGameDataWeapon(elemental);
        dataBase = DataController.Instance.GetDataBaseWeapons(elemental, data1.WeaponTierLevel.Tier);
        int Level = data1.WeaponTierLevel.Level > 0 ? data1.WeaponTierLevel.Level : 1;
        txtDamage.text = dataBase.ATK[Level - 1].ToString();
        PBDamage.fillAmount = dataBase.ATK[Level - 1] * 1f / dataBase.ATK[dataBase.ATK.Count - 1];
        txtFireRate.text = dataBase.ATKspeed[Level - 1].ToString();
        PBFireRate.fillAmount = dataBase.ATKspeed[Level - 1] * 1f / dataBase.ATK[dataBase.ATKspeed.Count - 1];
        txtFireRate.text = dataBase.ATKspeed[Level - 1].ToString();
        PBFireRate.fillAmount = dataBase.ATKspeed[Level - 1] * 1f / dataBase.ATK[dataBase.ATKspeed.Count - 1];
        currentExp = data1.EXP;
        AddExp = 0;
        GoldCost = 0;
        MaxExp = dataBase.Cost[Level - 1];
        SetTextExp();
        ResetItemSlot();
        CheckBtnUpgrade();
        //set Up List Item 
        for (int i = 0; i < uiItems.Length; i++)
            uiItems[i].SetUpData();

        ItemDatabase = new Dictionary<ITEM_TYPE, ItemData>();
        for (int i = 0; i < uiItems.Length; i++)
        {
            ItemDatabase.Add(uiItems[i].ITEM_TYPE, DataController.Instance.GetItemDataBase(uiItems[i].ITEM_TYPE));
        }
    }
    public void SetTextExp()
    {
        if (AddExp > 0)
            txtEXP.text = string.Format("{0} <color=#65FF00FF>(+{1})</color>", currentExp, AddExp);
        else
            txtEXP.text = currentExp.ToString();

        PBEXP.fillAmount = (currentExp + AddExp) * 1f / MaxExp;
        addLevel = 0;
        int preExp = 0;
        RemainEXP = 0;
        for (int i = data1.WeaponTierLevel.Level - 1; i < dataBase.Cost.Count; i++)
        {
            if (AddExp + currentExp - preExp > dataBase.Cost[i])
            {
                if (i == dataBase.Cost.Count - 1)
                {
                    RemainEXP = (int)dataBase.Cost[i];
                }
                else
                {
                    addLevel++;
                    preExp += (int)dataBase.Cost[i];
                    RemainEXP = (int)(AddExp + currentExp - preExp);
                }
            }
        }
        if (addLevel == 0)
            txtHeroLevel.text = "Lv." + data1.WeaponTierLevel.Level;
        else
            txtHeroLevel.text = string.Format("Lv.{0} <color=#65FF00FF>(+{1})</color>", data1.WeaponTierLevel.Level, addLevel);
    }

    public void ResetItemSlot()
    {
        Slots = new ITEM_TYPE[] { ITEM_TYPE.None, ITEM_TYPE.None, ITEM_TYPE.None, ITEM_TYPE.None, ITEM_TYPE.None };
        for (int i = 0; i < ImgSlot.Length; i++)
        {
            ImgSlot[i].sprite = sprEmptySlot;
        }
    }

    public void ResetSlot(int Index)
    {
        if (Slots[Index] != ITEM_TYPE.None)
        {
            ImgSlot[Index].sprite = sprEmptySlot;
            GetUiItem(Slots[Index]).ChangeItemNumber(1);
            AddExp -= AddExpByType(Slots[Index], ItemDatabase[Slots[Index]].Value);
            GoldCost -= ItemDatabase[Slots[Index]].UseGoldCost;
            SetTextExp();
            Slots[Index] = ITEM_TYPE.None;
            CheckBtnUpgrade();
        }
    }

    public void OnSelectItem(ITEM_TYPE _TYPE, Sprite sprite)
    {
        ImgSlot[EmptySlot].sprite = sprite;
        Slots[EmptySlot] = _TYPE;
        AddExp += AddExpByType(_TYPE, ItemDatabase[_TYPE].Value);
        GoldCost += ItemDatabase[_TYPE].UseGoldCost;
        SetTextExp();
        CheckBtnUpgrade();
    }

    public int AddExpByType(ITEM_TYPE _TYPE, int value)
    {
        if (_TYPE.ToString().ToLower().Contains(heroElemental.ToString().ToLower()))
            return value;
        return (int)(value * 0.5);
    }

    UiItem GetUiItem(ITEM_TYPE _TYPE)
    {
        for (int i = 0; i < uiItems.Length; i++)
        {
            if (uiItems[i].ITEM_TYPE == _TYPE)
                return uiItems[i];
        }
        return null;
    }

    public void CheckBtnUpgrade()
    {
        if (HasNotEmptySlot)
        {
            BtnUpgrade.gameObject.SetActive(true);
            BtnAuto.gameObject.SetActive(false);
            txtGoldCost.text = GoldCost.ToString();
        }
        else
        {
            BtnUpgrade.gameObject.SetActive(false);
            BtnAuto.gameObject.SetActive(true);
        }

    }

    public void OnAutoSelectClick()
    {

    }

    public void OnBtnUpgradetClick()
    {
        if (DataController.Instance.Gold >= GoldCost)
        {
            DataController.Instance.AddWeaponLevel(heroElemental, addLevel, RemainEXP);
            DataController.Instance.Gold -= GoldCost;
            for (int i = 0; i < Slots.Length; i++)
            {
                if (Slots[i] != ITEM_TYPE.None)
                {
                    DataController.Instance.AddItemQuality(Slots[i], -1);
                }
            }
            SetUpData(heroElemental);
            MenuController.Instance.UIPanelHeroAlliance.SetupUIHero(heroElemental);
        }
        else
        {

        }
    }
}
