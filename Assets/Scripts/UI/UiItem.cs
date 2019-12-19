using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
public class UiItem : MonoBehaviour
{
    public ITEM_TYPE ITEM_TYPE;
    public TextMeshProUGUI txtNumber;
    Item Item;
    UIButton UIButton;
    int count = 0;
    /// <summary>
    /// TypeInUI =0 : UIPanelHeroAlliance.UIUpgradehero
    /// TypeInUI =1 : SelectLevel.LevelDetail
    /// </summary>
    public int TypeInUI = 0;
    public UIUpgradehero UIUpgradehero
    {
        get
        {
            return MenuController.Instance.UIPanelHeroAlliance.UIUpgradehero;
        }
    }
    private void Awake()
    {
        txtNumber = GetComponentInChildren<TextMeshProUGUI>();
        UIButton = GetComponent<UIButton>();
        UIButton.SetUpEvent(OnClick);
    }
    private void OnClick()
    {
        if (count > 0 && UIUpgradehero.EmptySlot != 999 && TypeInUI == 0)
        {
            ChangeItemNumber(-1);
            UIUpgradehero.OnSelectItem(ITEM_TYPE, GetComponent<Image>().sprite);
        }
    }


    public void ChangeItemNumber(int value)
    {
        count += value;
        txtNumber.text = count.ToString();
    }

    public void SetUpData(int TypeInUI = 0)
    {
        this.TypeInUI = TypeInUI;
        Item = DataController.Instance.GetGameItemData(ITEM_TYPE);
        count = Item.Quality;
        txtNumber.text = Item.Quality.ToString();
    }

    public void SetUpData(ITEM_TYPE _TYPE, int TypeInUI = 0)
    {
        this.TypeInUI = TypeInUI;
        ITEM_TYPE = _TYPE;
        Item = DataController.Instance.GetGameItemData(ITEM_TYPE);
        count = Item.Quality;
        txtNumber.text = Item.Quality.ToString();
    }
    public void SetUpData(Item item, int TypeInUI = 0)
    {
        this.TypeInUI = TypeInUI;
        ITEM_TYPE = item.Type;
        Item = item;
        count = Item.Quality;
        txtNumber.text = Item.Quality.ToString();
        GetComponent<Image>().sprite = DataController.Instance.DefaultData.GetSpriteItem(ITEM_TYPE);
    }
}
