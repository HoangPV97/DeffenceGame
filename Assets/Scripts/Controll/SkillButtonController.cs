using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Runtime.InteropServices;

public class SkillButtonController : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler, IPointerClickHandler
{
    public int Slot;
    public TYPE_OF_SKILL_CONTROLL typeControll;
    private Image imgBgSkill, imgJoystick, imgJoystickBg, imgCoolDownMash;
    private Vector3 _inputvector;
    bool LockBtn = false;
    Color cNotEnoughMana = new Color(1, 1, 1, 0.3f);
    Color cShow = new Color(1, 1, 1, 1f);
    bool isSetup = false;
    public Skill Skill;
    float currentCooldown
    {
        get
        {
            return 0;
        }
    }

    float currentMana
    {
        get
        {
            return 0;
        }
    }

    bool isDraging = false;
    private float countDrag = 0;

    private void Awake()
    {
        imgBgSkill = transform.GetChild(0).GetComponent<Image>();
        imgJoystickBg = transform.GetChild(1).GetComponent<Image>();
        imgJoystick = transform.GetChild(2).GetComponent<Image>();
        imgCoolDownMash = transform.GetChild(3).GetComponent<Image>();
        isSetup = false;
    }

    public void HideBtn()
    {
        gameObject.SetActive(false);
    }

    public void SetupData(int Slot, TYPE_OF_SKILL_CONTROLL typeControll, Skill Skill)
    {
        imgJoystickBg.gameObject.SetActive(false);
        imgJoystick.gameObject.SetActive(false);
        this.Slot = Slot;
        this.typeControll = typeControll;
        this.Skill = Skill;
        // imgBgSkill.sprite = AssetBundleController.Instance.LoadFromBundleItem("icons_" + SkillModelData.SkillID.ToString());
        //imgCoolDownMash.sprite = AssetBundleController.Instance.LoadFromBundleItem("icons_" + SkillModelData.SkillID.ToString());
        imgBgSkill.SetNativeSize();
        imgCoolDownMash.SetNativeSize();
        if (currentCooldown == 0)
            imgCoolDownMash.gameObject.SetActive(false);
        OnLockBtn(false);
        isSetup = true;
    }

    public void OnPointerDown(PointerEventData Point)
    {

        isDraging = false;
        countDrag = 0;
        if (!LockBtn)
        {
            if (typeControll != TYPE_OF_SKILL_CONTROLL.Touch)
            {
                Time.timeScale = 0f;
                imgJoystickBg.gameObject.SetActive(true);
                imgJoystick.gameObject.SetActive(true);
                OnChangeJoyStickImage(false);
            }
        }

    }

    public void OnChangeJoyStickImage(bool cancel)
    {
        if (cancel)
        {
            // imgJoystickBg.sprite = DataManager.Instance.GamePlayResource.CancelSkillJoystickBG;
            //  imgJoystick.sprite = DataManager.Instance.GamePlayResource.CancelSkillJoystick;
        }
        else
        {
            //  imgJoystickBg.sprite = DataManager.Instance.GamePlayResource.NormalSkillJoystickBG;
            //  imgJoystick.sprite = DataManager.Instance.GamePlayResource.NormalSkillJoystick;
        }
    }

    public void OnPointerUp(PointerEventData Point)
    {
        if (!LockBtn)
        {
            if (isDraging)
            {
                StartCoroutine("IEOnPointerUp");
            }
        }
        onShowSkillGuilde(false);
        _inputvector = Vector3.zero;
        imgJoystick.rectTransform.anchoredPosition = Vector3.zero;
        //    UIGamePlayController.Instance.OnShowCancelSkill(false);
        imgJoystickBg.gameObject.SetActive(false);
        imgJoystick.gameObject.SetActive(false);
        //  UIGamePlayController.Instance.OnLockSwitchHero(false);

    }

    public void OnPointerClick(PointerEventData Point)
    {
        if (!LockBtn && !isDraging)
        {
           /* if (GamePlayController.Instance.CurrentHero.OnAutoCastSkill(SkillModelData.SkillID))
            {
                GamePlayController.Instance.CurrentHero.cooldownSkill[Slot - 1] = SkillModelData.Cooldown;
                ActiveCoolDownMask();
            }
            else
            {
            }*/
        }

    }

    void Update()
    {
        if (isSetup)
        {
            if (currentCooldown > 0)
            {
                imgCoolDownMash.fillAmount = currentCooldown / Skill.CountdownTime;
                if (!LockBtn)
                    OnLockBtn(true);
            }
            else
            {
                if (LockBtn)
                {
                    OnLockBtn(false);
                    imgCoolDownMash.gameObject.SetActive(false);
                }
            }

            if (currentMana < Skill.manaCost)
            {
                // imgCoolDownMash.color = cNotEnoughMana;
                imgBgSkill.color = cNotEnoughMana;
            }
            else
            {
                // imgCoolDownMash.color = cShow;
                imgBgSkill.color = cShow;
            }
        }
    }

    public IEnumerator IEOnPointerUp()
    {
        yield return new WaitForSeconds(0.02f);
    
          /*  if (!UIGamePlayController.Instance.CancelSkill)
            {

                if (GamePlayController.Instance.CurrentHero.OnSkill(SkillModelData.SkillID))
                {
                    GamePlayController.Instance.CurrentHero.cooldownSkill[Slot - 1] = SkillModelData.Cooldown;
                    ActiveCoolDownMask();
                }
                else
                {
                }
            }
            else
            {
                UIGamePlayController.Instance.OnCalcelSkillClick();
            }
            */
        
    }


    public void OnDrag(PointerEventData Point)
    {
        if (typeControll != TYPE_OF_SKILL_CONTROLL.Touch)
        {
            countDrag++;
            isDraging = true;
            if (!LockBtn && countDrag > 3)
            {
                Vector2 poss;
                if (RectTransformUtility.ScreenPointToLocalPointInRectangle(imgBgSkill.rectTransform, Point.position, Point.pressEventCamera, out poss))
                {
                    poss.x = (poss.x / imgBgSkill.rectTransform.sizeDelta.x);
                    poss.y = (poss.y / imgBgSkill.rectTransform.sizeDelta.y);
                    _inputvector = new Vector3(poss.x, poss.y, 0f);
                    _inputvector = (_inputvector.magnitude > 1f) ? _inputvector.normalized : _inputvector;
                    imgJoystick.rectTransform.anchoredPosition = new Vector3(_inputvector.x * (imgJoystickBg.rectTransform.sizeDelta.x / 2), _inputvector.y * (imgJoystickBg.rectTransform.sizeDelta.y / 2));
                }
                // Maingcamera.instance.JoyStickRotationDrag();
                if (typeControll == TYPE_OF_SKILL_CONTROLL.Drag)
                {
                    //  GamePlayController.Instance.CurrentHero.MoveSkillDragTarget(_inputvector.magnitude);
                }

            }
        }

    }
    public void OnLockBtn(bool bLock)
    {
        LockBtn = bLock;
    }

    private void ActiveCoolDownMask()
    {
        //currentCooldown = SkillModelData.Cooldown;
        imgCoolDownMash.gameObject.SetActive(true);
        OnLockBtn(true);
    }


    private void onShowSkillGuilde(bool active)
    {
        switch (typeControll)
        {
            case TYPE_OF_SKILL_CONTROLL.Drag:
                //  GamePlayController.Instance.CurrentHero.OnShowSkillDrag(SkillModelData.SkillID, active);
                break;
            case TYPE_OF_SKILL_CONTROLL.Joystick:
                //   GamePlayController.Instance.CurrentHero.OnShowSkillDirection(SkillModelData.SkillID, active);
                break;
            case TYPE_OF_SKILL_CONTROLL.Touch:
                break;
        }
    }
}

public enum TYPE_OF_SKILL_CONTROLL
{
    Drag,
    Joystick,
    Touch
}