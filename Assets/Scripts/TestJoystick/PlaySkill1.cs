using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySkill1 : Skill
{
    public PlayerController playerController;
    public GameObject arrow;
    public VariableJoystick variableJoystick;
    private Vector2 direction;
    private float angle;
    ObjectPoolManager poolManager;

    protected void Start()
    {
        poolManager = ObjectPoolManager.Instance;
        base.Start();
    }
    protected void Update()
    {
        base.Update();

        if (TimeLeft <= 0 && Tower.Mana.CurrentMana >= manaNumber && variableJoystick.Vertical != 0)
        {
            direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - arrow.transform.position;
            angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            RotateArrow();
        }
        if (Tower.Mana.CurrentMana > manaNumber)
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
        arrow.transform.eulerAngles = new Vector3(0, 0, angle - 90.0f);
    }
    public void Skill1(Vector2 _direction, float _rotatioZ)
    {
        GameObject SlowSkill = poolManager.SpawnObject("slowskill", arrow.transform.position, Quaternion.identity);
        SlowSkill.transform.rotation = Quaternion.Euler(0, 0, _rotatioZ);
        Rigidbody2D rigidbody = SlowSkill.GetComponent<Rigidbody2D>();
        float speed = SlowSkill.GetComponent<BulletController>().bullet.Speed;
        rigidbody.velocity = _direction.normalized *40* speed * Time.deltaTime;
       
    }
    private void OnMouseUp()
    {
        arrow.SetActive(false);
        if (Tower.Mana.CurrentMana >= manaNumber)
        {
            playerController.SlowSkill(direction, angle - 90f);
            CountdownGo?.gameObject.SetActive(true);
            StartCountdown = true;
            TimeLeft = CountdownTime;
            Tower.Mana.ConsumeMana(manaNumber);
            arrow.SetActive(false);
        }
    }

}
