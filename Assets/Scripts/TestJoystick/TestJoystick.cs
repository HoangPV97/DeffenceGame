using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestJoystick : Skill
{
    public VariableJoystick variableJoystick;
    Vector2 direction;
    float angle;

    protected void Start()
    {
        base.Start();
    }
    protected void Update()
    {
        base.Update();
        Vector3 MousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        direction = MousePosition - directionGO.transform.position;
        angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        if (TimeLeft <= 0 && variableJoystick.Vertical != 0)
        {
            directionGO.SetActive(true);
            directionGO.transform.eulerAngles = new Vector3(0, 0, angle);
            directionGO.transform.localScale = new Vector3(direction.magnitude / 6 * transform.localScale.x, 0.3f, 1f);
        }
    }

    public void Skill1(Vector2 _direction, float _rotatioZ)
    {
        player.ShootToDirection(_direction, _rotatioZ, "skillbullet1");
        CountdownGo?.gameObject.SetActive(true);
        StartCountdown = true;
        TimeLeft = CountdownTime;
    }
    private void OnMouseUp()
    {
        directionGO.SetActive(false);
        Skill1(direction, angle);
    }

}
