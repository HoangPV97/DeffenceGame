using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestJoystick : MonoBehaviour
{
    public VariableJoystick variableJoystick;
    Vector2 direction;
    float angle;
    PlaySkill playSkill;
    public GameObject arrow;
    private void Start()
    {
        playSkill = FindObjectOfType<PlaySkill>();
    }
    public void FixedUpdate()
    {
        
        direction = Vector2.up * variableJoystick.Vertical + Vector2.right * variableJoystick.Horizontal;
        angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        if (playSkill.TimeLeft <= 0)
        {
            arrow.SetActive(true);
            arrow.transform.eulerAngles = new Vector3(0, 0, angle);
        }
    }

    private void OnMouseUp()
    {
        arrow.SetActive(false);
        playSkill.Skill1(direction,angle);
    }

}
