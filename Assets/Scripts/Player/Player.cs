using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class Player : Characters
{
    public enum AutoMode { TurnOn, TurnOff };
    private float currentHealth;
    public float Health,Mana, CurrentMana;
    public float recoverMana,recoverTime;
    [SerializeField]
    private Image healthBar,manaBar;
    Vector3 EndPosition;
    //public bool AutoAttack = false;
    public AutoMode currentMode;
    public Elemental elementalPlayer;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = Health;
        CurrentMana = Mana;
        currentMode = AutoMode.TurnOff;
        InvokeRepeating("RecoverMana", 0f, recoverTime);
        base.Start();
    }
    public Elemental GetElemental()
    {
        return elementalPlayer;
    }
    // Update is called once per frame
    private void Update()
    {
        if (RateOfFire < 0)
        {
            switch (currentMode)
            {
                case AutoMode.TurnOff:
                    if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
                    {
                        Vector2 direct = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
                        float rotationZ = Mathf.Atan2(direct.y, direct.x) * Mathf.Rad2Deg;
                        transform.rotation = Quaternion.Euler(0, 0, rotationZ - 90);
                        ShootToDirection(direct, rotationZ, "tankbullet");
                        RateOfFire = 1;
                    }
                    break;
                case AutoMode.TurnOn:
                    if (Target != null)
                    {
                        LookAtEnemy(Target);
                        Vector3 dir = Target.position - transform.position;
                        GameObject bullet = Spawn("tankbullet", Barrel.transform.position);
                        TankBullet mBullet = bullet.GetComponent<TankBullet>();
                        mBullet.DirectShooting(dir);
                        RateOfFire = 1;
                    }
                    break;
            }
        }
        RateOfFire -= Time.deltaTime;
    }
    public void TakeDamge(float _Damge)
    {
        currentHealth -= _Damge;
        healthBar.fillAmount = currentHealth / Health * 1.0f;
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    public void ConsumeMana(float _mana)
    {
        CurrentMana -= _mana;
        Debug.Log($"Mana :  {CurrentMana}");
        manaBar.fillAmount = CurrentMana / Mana * 1.0f;
    }
    public void Die()
    {
        Live = false;
    }
    public void ShootToDirection(Vector2 _direction, float _rotatioZ, string _bullet)
    {
        GameObject bullet = Spawn(_bullet, Barrel.transform.position);
        bullet.transform.rotation = Quaternion.Euler(0, 0, _rotatioZ - 90);
        TankBullet mBullet = bullet.GetComponent<TankBullet>();
        mBullet.DirectShooting(_direction);
    }
    public void RecoverMana()
    {
        if (CurrentMana < Mana  )
        {
            CurrentMana += recoverMana;
            manaBar.fillAmount = CurrentMana / Mana * 1.0f;
            if (CurrentMana > Mana)
                CurrentMana = Mana;
        }
    }
}
