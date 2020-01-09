using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class TabItem : MonoBehaviour, IPointerUpHandler, IPointerDownHandler, IPointerClickHandler
{
    public UnityEvent unityEvent;
    TabController TabController;
    public bool isNotScale = false;
    [SerializeField]
    public Sprite EnabldeSprite, DisableSprite;
    [SerializeField]
    public Color EnableColorText, DisableColorText;
    [SerializeField]
    Image Image;
    TextMeshProUGUI text;
    public int Tab;
    // Start is called before the first frame update
    void Awake()
    {
        TabController = transform.parent.parent.GetComponent<TabController>();
        Image = GetComponent<Image>();
        text = GetComponentInChildren<TextMeshProUGUI>();
        Tab = transform.GetSiblingIndex();
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
        if (TabController == null)
        {
            TabController = transform.parent.parent.GetComponent<TabController>();
            Image = GetComponent<Image>();
            text = GetComponentInChildren<TextMeshProUGUI>();
        }
        TabController.OnTabClick(this);
    }

    public virtual void OnEnableTab()
    {
        Image.sprite = EnabldeSprite;
        text.color = EnableColorText;
    }

    public virtual void OnDisableTab()
    {
        Image.sprite = DisableSprite;
        text.color = DisableColorText;
    }
}
