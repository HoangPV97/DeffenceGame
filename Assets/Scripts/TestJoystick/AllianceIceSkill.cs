using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllianceIceSkill : Skill
{
    public VariableJoystick variableJoystick;
    public GameObject circle;
    public IceAllianceCharacter Alliance;
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected void Update()
    {
        base.Update();
        if (TimeLeft <= 0 && Tower.Mana.CurrentMana >= manaCost && variableJoystick.Vertical != 0)
        {
            Vector3 MousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = MousePosition - circle.transform.position;
            MoveObject(circle, direction);
        }
        if (Tower.Mana.CurrentMana > manaCost)
        {
            //   LowMana.SetActive(false);
        }
        else
        {
            // LowMana.SetActive(true);
        }

    }
    public void MoveObject(GameObject _object, Vector3 _postion)
    {
        if (!_object.activeSelf)
            _object.SetActive(true);
        _object.transform.Translate(_postion);
    }

    public void Play()
    {
        TimeLeft = CountdownTime;
        StartCountdown = true;
        CountdownGo?.gameObject.SetActive(true);
        Tower.Mana.ConsumeMana(manaCost);
        Alliance.IceSkill(circle.transform.position);
        circle.SetActive(false);
    }
    public override void OnInvokeSkill()
    {
        Debug.Log(Tower.Mana.CurrentMana + "___" + manaCost);
        circle.SetActive(false);
        if (Tower.Mana.CurrentMana >= manaCost)
        {
            Play();
        }
    }
    public override void OnCancelSkill()
    {
        circle.SetActive(false);
    }
}
