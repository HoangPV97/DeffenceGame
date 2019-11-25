using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySkill2 : Skill
{
    public VariableJoystick variableJoystick;
    public GameObject circle;
    public IceAllianceCharacter Alliance;
    ObjectPoolManager poolManager;
    // Start is called before the first frame update
    void Start()
    {
        poolManager = ObjectPoolManager.Instance;
        base.Start();
    }

    // Update is called once per frame
    protected void Update()
    {
        base.Update();
        if (TimeLeft <= 0 && Tower.Mana.CurrentMana >= manaNumber && variableJoystick.Vertical != 0 )
        {
            Vector3 MousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = MousePosition - circle.transform.position;
            MoveObject(circle, direction);
        }
        if (Tower.Mana.CurrentMana > manaNumber)
        {
            LowMana.SetActive(false);
        }
        else
        {
            LowMana.SetActive(true);
        }

    }
    public void MoveObject(GameObject _object, Vector3 _postion)
    {
        _object.SetActive(true);
        _object.transform.Translate(_postion);
    }
    public void FixedUpdate()
    {

    }
    public void Play()
    {
        TimeLeft = CountdownTime;
        StartCountdown = true;
        CountdownGo?.gameObject.SetActive(true);
        Tower.Mana.ConsumeMana(manaNumber);
        Alliance.IceSkill(circle.transform.position);
        circle.SetActive(false);
    }
    private void OnMouseUp()
    {
        circle.SetActive(false);
        if (Tower.Mana.CurrentMana >= manaNumber)
        {
            Play();
        }
    }
}
