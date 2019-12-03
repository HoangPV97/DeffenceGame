using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class VariableJoystick : Joystick
{
    public float MoveThreshold { get { return moveThreshold; } set { moveThreshold = Mathf.Abs(value); } }

    [SerializeField] private float moveThreshold = 1;
    [SerializeField] private JoystickType joystickType = JoystickType.Fixed;

    private Vector2 fixedPosition = Vector2.zero;

    public Skill Skill;
    public Image CountDountMask;

    public void SetUpData(Skill Skill)
    {
        this.Skill = Skill;
        gameObject.SetActive(true);
        //set icon
    }

    public void SetMode(JoystickType joystickType)
    {
        this.joystickType = joystickType;
        if (joystickType == JoystickType.Fixed)
        {
            background.anchoredPosition = fixedPosition;
            background.gameObject.SetActive(true);
        }
        else
            background.gameObject.SetActive(false);
    }

    protected override void Start()
    {
        base.Start();
        fixedPosition = background.anchoredPosition;
        SetMode(joystickType);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        /*   if (joystickType != JoystickType.Fixed)
           {
               background.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
               background.gameObject.SetActive(true);
           }*/
        base.OnPointerDown(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        /*  if (joystickType != JoystickType.Fixed)
              background.gameObject.SetActive(false);
              */
        base.OnPointerUp(eventData);
        StartCoroutine(IEOnPointerUp());
    }

    protected override void HandleInput(float magnitude, Vector2 normalised, Vector2 radius, Camera cam)
    {
        if (joystickType == JoystickType.Dynamic && magnitude > moveThreshold)
        {
            Vector2 difference = normalised * (magnitude - moveThreshold) * radius;
            //  background.anchoredPosition += difference;
        }
        base.HandleInput(magnitude, normalised, radius, cam);
    }

    public IEnumerator IEOnPointerUp()
    {
        yield return new WaitForSeconds(0.02f);
        if (!GameplayController.Instance.CancelSkill)
        {
            if (Skill != null)
                Skill.OnInvokeSkill();
        }
        else
        {
            if (Skill != null)
                Skill.OnCancelSkill();
        }


    }

}


public enum JoystickType { Fixed, Floating, Dynamic }