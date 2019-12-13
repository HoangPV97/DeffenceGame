using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class UIButton : MonoBehaviour, IPointerUpHandler, IPointerDownHandler, IPointerClickHandler
{
    public GameObject Notification;
    public GameObject ClickEffect;
    [SerializeField]
    public Sprite EnabldeSprite, DisableSprite;
    [SerializeField]
    public Color EnableColorText, DisableColorText;
    [SerializeField]
    public Color EnableColorOutline, DisableColorOutline;
    public Transform parent = null;
    public UnityAction unityAction = null;
    public bool isNotScale;
    public bool canclick = true;
    public float TimeUnlock = 1;
    public bool Suspend = false;
    public bool Enable = true;
    public Sprite NormalSprite;
    public Sprite SpriteOnPress;
    public Image Image, RootImage;
    string BtnEnable, BtnDisable;
    public BtnType TypeBtn;
    public void Awake()
    {
        RootImage = GetComponent<Image>();
        Suspend = false;
        if (Image == null)
            Image = RootImage;
        if (Image != null)
            NormalSprite = Image.sprite;
    }

    private void Start()
    {
        /* if (Enable)
             OnEnableButton();
         else
             OnDisableButton();*/
    }
    public void OnPointerUp(PointerEventData data)
    {
        transform.localScale = Vector3.one;
        if (NormalSprite != null && SpriteOnPress != null && Enable)
            Image.sprite = NormalSprite;
        if (TypeBtn == BtnType.Unlock)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }

    public void OnPointerDown(PointerEventData data)
    {
        if (Enable)
        {
            if (isNotScale)
                transform.localScale = Vector3.one * 1f;
            else
                transform.localScale = Vector3.one * 0.9f;
        }
        if (NormalSprite != null && SpriteOnPress != null && Enable)
        {
            Image.sprite = SpriteOnPress;
            if (ClickEffect != null)
                ClickEffect.SetActive(true);
        }
        if (TypeBtn == BtnType.Unlock)
        {
            transform.localScale = new Vector3(1.05f, 1.05f, 1);
        }

    }

    public void SuspendClick(bool active)
    {
        Suspend = active;
    }

    public void OnPointerClick(PointerEventData data)
    {
        if (canclick && !Suspend)
        {
            canclick = false;
            if (TypeBtn != BtnType.Unlock && TypeBtn != BtnType.OpenChest)
            {
                if (BtnEnable == "BtnDefault")
                {
                    //  SoundManager.Instance.PlaySound("silentsound");
                }
                else
                {
                    //  SoundManager.Instance.PlaySound(BtnEnable);
                }
            }
            if (unityAction != null)
                unityAction();

            DG.Tweening.DOVirtual.DelayedCall(TimeUnlock, () =>
            {
                canclick = true;
            }, false);
            // UIController.Instance.SpawnTapEffect(data.pressPosition);
        }
        else if (TypeBtn != BtnType.OpenChest)
        {
            // SoundManager.Instance.PlaySound(BtnDisable);
        }


    }

    public void SetUpEvent(UnityAction action, string BtnEnable = "BtnDefault", string BtnDisable = "BtnDisable")
    {
        unityAction = action;
        this.BtnEnable = BtnEnable;
        this.BtnDisable = BtnDisable;
    }


    public void SetUpEvent(UnityAction action, float TimeUnlock, bool changeParrent = false)
    {
        this.TimeUnlock = TimeUnlock;
        if (changeParrent || parent == null)
        {
            parent = transform.parent;
        }
        unityAction = action;
    }

    public void OnDisableButton()
    {
        if (DisableSprite != null)
        {
            //   gameObject.GetComponent<Image>().sprite = DisableSprite;
            if (Image == null)
                Image = GetComponent<Image>();
            Image.sprite = DisableSprite;
        }
        else if (RootImage != null)
            RootImage.enabled = false;
        Text[] texts = GetComponentsInChildren<Text>();
        if (texts.Length > 0)
        {
            for (int i = 0; i < texts.Length; i++)
                texts[i].color = DisableColorText;
        }

        Outline[] Outlines = GetComponentsInChildren<Outline>();
        if (Outlines.Length > 0)
        {
            for (int i = 0; i < Outlines.Length; i++)
                Outlines[i].effectColor = DisableColorOutline;
        }
        Enable = false;
    }

    public void OnEnableButton()
    {
        if (RootImage != null)
            RootImage.enabled = true;
        if (EnabldeSprite != null)
        {
            if (Image == null)
                Image = gameObject.GetComponent<Image>();
            //sprite = EnabldeSprite;
            Image.sprite = EnabldeSprite;
        }
        Text[] texts = GetComponentsInChildren<Text>();
        if (texts.Length > 0)
        {
            for (int i = 0; i < texts.Length; i++)
                texts[i].color = EnableColorText;
        }

        Outline[] Outlines = GetComponentsInChildren<Outline>();
        if (Outlines.Length > 0)
        {
            for (int i = 0; i < Outlines.Length; i++)
                Outlines[i].effectColor = EnableColorOutline;
        }
        Enable = true;
    }

    public void ShowNotification()
    {
        if (Notification != null)
            Notification.SetActive(true);
    }
    public void HideNotification()
    {
        if (Notification != null)
            Notification.SetActive(false);
    }

    public enum BtnType { Normal, Unlock, OpenChest };
}

