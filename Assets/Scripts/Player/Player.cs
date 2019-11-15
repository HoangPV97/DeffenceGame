using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Player : Characters
{
    public enum AutoMode { TurnOn, TurnOff };
    public AutoMode currentMode;
    //public Elemental elementalPlayer;
    public ViewPlayerController ViewPlayer;
    public Health Health;
    public Mana Mana;
    private float coundown;
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
        if (coundown < 0)
        {
            switch (currentMode)
            {
                case AutoMode.TurnOff:
                    characterState = CharacterState.Idle;
                    if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
                    {
                        //ViewPlayer.SetPositionBone(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                        Vector2 direct = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
                        float rotationZ = Mathf.Atan2(direct.y, direct.x) * Mathf.Rad2Deg;

                        ShootToDirection(direct, rotationZ, "tankbullet");
                        coundown = RateOfFire;
                    }
                    break;
                case AutoMode.TurnOn:
                    if (Target != null)
                    {
                       // ViewPlayer.SetPositionBone(Target.position);
                        characterState = CharacterState.Attack;
                        Vector2 direct = Target.position - transform.position;
                        float rotationZ = Mathf.Atan2(direct.y, direct.x) * Mathf.Rad2Deg;
                        //transform.rotation = Quaternion.Euler(0, 0, rotationZ - 90);
                        ShootToDirection(direct, rotationZ, "tankbullet");
                        coundown = RateOfFire;
                    }
                    break;
            }
        }
        coundown -= Time.deltaTime;
        base.Update();
    }
    public void TakeDamge(float _Damge)
    {
        Health.CurrentHealth -= _Damge;
        float width = Health.healthBar.rectTransform.rect.width;
        float height = Health.healthBar.rectTransform.rect.height;
        Health.healthBar.rectTransform.sizeDelta = new Vector2(width * (Health.CurrentHealth / Health.health * 1.0f), height);
        if (Health.CurrentHealth <= 0)
        {
            Die();
        }
    }
    public void ConsumeMana(float _mana)
    {
        Mana.CurrentMana -= _mana;
        // Debug.Log($"Mana :  {CurrentMana}");
        float width = Mana.maxManaBar.rectTransform.rect.width;
        float height = Mana.maxManaBar.rectTransform.rect.height;
        Mana.manaBar.rectTransform.sizeDelta = new Vector2(width * (Mana.CurrentMana / Mana.mana * 1.0f), height);
    }
    public void Die()
    {
        Live = false;
    }
    public void ShootToDirection(Vector2 _direction, float _rotatioZ, string _bullet)
    {
        characterState = CharacterState.Attack;
        GameObject bullet = Spawn(_bullet, Barrel.position);
        bullet.transform.rotation = Quaternion.Euler(0, 0, _rotatioZ - 90);
        TankBullet mBullet = bullet.GetComponent<TankBullet>();
        mBullet.elementalBullet = elementalType;
        mBullet.DirectShooting(_direction);
    }
    public void RecoverMana()
    {
        if (Mana.CurrentMana < Mana.mana)
        {
            Mana.CurrentMana += Mana.RecoverManaValue;
            float width = Mana.maxManaBar.rectTransform.rect.width;
            float height = Mana.maxManaBar.rectTransform.rect.height;
            Mana.manaBar.rectTransform.sizeDelta = new Vector2(Mana.manaBar.rectTransform.rect.width + (Mana.RecoverManaValue / Mana.mana *width) , height);
            if (Mana.CurrentMana > Mana.mana)
                Mana.CurrentMana = Mana.mana;
        }
    }
    public void RecoverHealth()
    {
        if (Health.CurrentHealth < Health.health)
        {
            Health.CurrentHealth += Health.RecoverHealthValue;
            float width = Health.maxHealthBar.rectTransform.rect.width;
            float height = Health.maxHealthBar.rectTransform.rect.height;
            Health.healthBar.rectTransform.sizeDelta = new Vector2(Health.healthBar.rectTransform.rect.width + (Health.RecoverHealthValue / Health.health *width ), height);
            if (Health.CurrentHealth > Health.health)
                Health.CurrentHealth = Health.health;
        }
    }
}
