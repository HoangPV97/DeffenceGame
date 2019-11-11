using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySkill1 : Skill
{
    public GameObject arrow;
    public VariableJoystick variableJoystick;
    private Vector2 direction;
    private float angle;

    protected void Start()
    {
        base.Start();
    }
    protected void Update()
    {
        base.Update();

        if (TimeLeft <= 0 && player.CurrentMana >= manaNumber && variableJoystick.Vertical != 0)
        {
            direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - arrow.transform.position;
            angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            RotateArrow();
        }
        if (player.CurrentMana > manaNumber)
        {
            LowMana.SetActive(false);
        }
        else
        {
            LowMana.SetActive(true);
            //float time = ((manaNumber - player.CurrentMana) / player.recoverMana) * player.recoverTime;
            //StartCoroutine(WaitingActiveObject(LowMana, time, false));
        }
    }
    public void RotateArrow()
    {
        arrow.SetActive(true);
        arrow.transform.eulerAngles = new Vector3(0, 0, angle);
        arrow.transform.localScale = new Vector3(direction.magnitude / 6 * transform.localScale.x, 0.3f, 1f);
    }
    public void Skill1(Vector2 _direction, float _rotatioZ)
    {
        player.ShootToDirection(_direction, _rotatioZ, "skillbullet1");
        CountdownGo?.gameObject.SetActive(true);
        StartCountdown = true;
        TimeLeft = CountdownTime;
        player.ConsumeMana(manaNumber);
        arrow.SetActive(false);
    }
    private void OnMouseUp()
    {
        arrow.SetActive(false);
        if (player.CurrentMana >= manaNumber)
        {
            Skill1(direction, angle);
        }
    }

}
