using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
public class TabItem : MonoBehaviour, IPointerUpHandler, IPointerDownHandler, IPointerClickHandler
{
    TabController TabController;
    public bool isNotScale = false;
    [SerializeField]
    public Sprite EnabldeSprite, DisableSprite;
    [SerializeField]
    public Color EnableColorText, DisableColorText;
    [SerializeField]
    Image Image;
    TextMeshProUGUI text;
    // Start is called before the first frame update
    void Awake()
    {
        TabController = transform.parent.parent.GetComponent<TabController>();
        Image = GetComponent<Image>();
        text = GetComponentInChildren<TextMeshProUGUI>();
    }


    public void OnPointerUp(PointerEventData eventData)
    {
        transform.localScale = Vector3.one * 1f;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        transform.localScale = Vector3.one * 0.9f;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnTabClick();
    }

    public void OnTabClick()
    {
        TabController.OnTabClick(this);
    }

    public void OnEnableTab()
    {
        Image.sprite = EnabldeSprite;
        text.color = EnableColorText;
    }

    public void OnDisableTab()
    {
        Image.sprite = DisableSprite;
        text.color = DisableColorText;
    }
}
