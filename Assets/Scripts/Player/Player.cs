using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Player : Characters
{
    public enum AutoMode { TurnOn, TurnOff };
    public AutoMode currentMode;
    public Elemental elementalPlayer;
    public Health Health;
    public Mana Mana;
    // Start is called before the first frame update
    void Start()
    {
        Health.CurrentHealth = Health.health;
        Mana.CurrentMana = Mana.mana;
        currentMode = AutoMode.TurnOff;
        InvokeRepeating("RecoverMana", 0f, Mana.RecoverManaTime);
        InvokeRepeating("RecoverHealth", 0f, Health.RecoverHealthTime);
        base.Start();
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
        Health.CurrentHealth -= _Damge;
        Health.healthBar.fillAmount = Health.CurrentHealth / Health.health * 1.0f;
        if (Health.CurrentHealth <= 0)
        {
            Die();
        }
    }
    public void ConsumeMana(float _mana)
    {
        Mana.CurrentMana -= _mana;
        // Debug.Log($"Mana :  {CurrentMana}");
        Mana.manaBar.fillAmount = Mana.CurrentMana / Mana.mana * 1.0f;
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
        if (Mana.CurrentMana < Mana.mana)
        {
            Mana.CurrentMana += Mana.RecoverManaValue;
            Mana.manaBar.fillAmount = Mana.CurrentMana / Mana.mana * 1.0f;
            if (Mana.CurrentMana > Mana.mana)
                Mana.CurrentMana = Mana.mana;
        }
    }
    public void RecoverHealth()
    {
        if (Health.CurrentHealth < Health.health)
        {
            Health.CurrentHealth += Health.RecoverHealthValue;
            Health.healthBar.fillAmount = Health.CurrentHealth / Health.health * 1.0f;
            if (Health.CurrentHealth > Health.health)
                Health.CurrentHealth = Health.health;
        }
    }
}
