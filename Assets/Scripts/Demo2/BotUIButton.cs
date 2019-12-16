using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BotUIButton : MonoBehaviour, IPointerClickHandler
{
    public Sprite SpriteIconNormal, SpriteIconShow, SpriteIconDisable;
    public Image Icon;
    public GameObject Notification;
    public GameObject ClickEffect;
    public Transform parent = null;
    public UnityAction unityAction = null;
    public bool isNotScale;
    public bool canclick = true;
    public float TimeUnlock = 1;
    public bool Suspend = false;
    public bool Enable = true;

    public Sprite SpriteOnPress;
    public Sprite SpriteOnEnable;
    public Sprite SpriteOnDisable;
    public int ButtonState = 1;
    private Animator Animator;
    int soundIndex;
    public void Awake()
    {
        Suspend = false;
        Animator = GetComponent<Animator>();
    }

    public void SetButton(bool enable)
    {
        if (enable)
            OnEnableButton();
        else
            OnDisableButton();
    }

    void CheckEnable()
    {
        if (Enable)
            OnEnableButton();
        else
            OnDisableButton();
    }

    public void OnDisableButton()
    {
        Enable = false;
        Icon.sprite = SpriteIconDisable;
        GetComponent<Image>().sprite = SpriteOnDisable;
    }

    public void OnEnableButton()
    {
        Enable = true;
        Icon.sprite = SpriteIconNormal;
        GetComponent<Image>().sprite = SpriteOnEnable;
    }

    public void SuspendClick(bool active)
    {
        Suspend = active;
    }

    public void OnPointerClick(PointerEventData data)
    {
        if (Enable)
        {
            //Debug.Log(Disable);
            if (canclick && !Suspend)
            {
                // UIController.Instance.SpawnTapEffect(data.pressPosition);
            }
            OnClick();
        }
    }


    public void OnClick()
    {
        if (canclick && !Suspend)
        {
            //  UIController.Instance.canClickCloseUI = false;
            canclick = false;
            //  SoundController.Instance.PlayClickSound(soundIndex);
            if (ButtonState != 2)
                SetActiveSize(true);
            if (unityAction != null)
                unityAction();
            // UIController.Instance.SuspendAllBotButton();
            DG.Tweening.DOVirtual.DelayedCall(TimeUnlock, () =>
            {
                canclick = true;
                //   UIController.Instance.canClickCloseUI = true;
            }, true);
        }
    }
    public void SetUpEvent(UnityAction action, int soundIndex = 0)
    {
        this.soundIndex = soundIndex;
        unityAction = action;
        ButtonState = 1;
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

    public void SetActiveSize(bool active)
    {
        if (active)
        {
            // StartCoroutine("DoScaleUp");
            Icon.sprite = SpriteIconShow;
            //  Icon.SetNativeSize();
            BottomBarController.Instance.RefreshAllButton(this);
            switch (ButtonState)
            {
                case 1:
                    Animator.SetTrigger("ScaleUp");
                    // CheckEnable();
                    break;
                case 2:
                    //  CheckEnable();
                    break;
                case 3:
                    Animator.SetTrigger("ScaleUp2");
                    //   CheckEnable();
                    break;
            }
            ButtonState = 2;
        }
        else
        {
            // StartCoroutine("DoScaleDown");
            Icon.sprite = SpriteIconNormal;
            //   Icon.SetNativeSize();
            switch (ButtonState)
            {
                case 1:
                    Animator.SetTrigger("ScaleDown");
                    //  CheckEnable();
                    break;
                case 2:
                    Animator.SetTrigger("ScaleDown1");
                    // CheckEnable();
                    break;
                case 3:
                    //  CheckEnable();
                    break;
            }
            ButtonState = 3;
        }
    }
    public void SetDefaultSize()
    {
        Icon.sprite = SpriteIconNormal;
        //   Icon.SetNativeSize();
        switch (ButtonState)
        {
            case 1:
            case 2:
                Animator.SetTrigger("ScaleDownToNormal");
                // CheckEnable();
                break;
            case 3:
                Animator.SetTrigger("ScaleUpToNormal");
                // CheckEnable();
                break;
        }
        ButtonState = 1;
    }

}