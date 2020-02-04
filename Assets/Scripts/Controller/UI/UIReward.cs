using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIReward : BaseUIView
{
    public Animator Anim;
    public UIButton btnClose;
    public UiItem pfUIItem;
    public Transform ItemContain;
    void Start()
    {
        btnClose.SetUpEvent(OnHide);
    }
    public void SetUpData(List<Item> items)
    {
        OnShow();
        foreach (Transform child in ItemContain)
        {
            Destroy(child.gameObject);
        }
        for (int i = 0; i < items.Count; i++)
        {
            var it = Instantiate(pfUIItem.gameObject, Vector3.zero, Quaternion.identity);
            it.SetActive(true);
            it.transform.SetParent(ItemContain);
            it.transform.SetDefaultTransform();
            it.GetComponent<UiItem>().SetUpData(items[i], 3);
        }
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
