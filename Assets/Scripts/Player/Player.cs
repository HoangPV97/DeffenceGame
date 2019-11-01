using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Player : Characters
{
    public enum AutoMode { TurnOn, TurnOff };
    private float currentHealth = 100f;
    public float Health = 100f;
    public Image healthBar;
    Vector3 EndPosition;
    public bool AutoAttack=false;
    public AutoMode currentMode;
    // Start is called before the first frame update
    void Start()
    {
        currentMode = AutoMode.TurnOff;
        base.Start();
    }

    // Update is called once per frame
    private void Update()
    {
        if ( RateOfFire<0)
        {
            if (Input.GetMouseButtonDown(0) && currentMode == AutoMode.TurnOff 
                && !EventSystem.current.IsPointerOverGameObject())
            {
                Vector3 pointMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 direct = pointMouse - transform.position;
                float rotationZ = Mathf.Atan2(direct.y, direct.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, 0, rotationZ - 90);
                ShootToDirection(direct, rotationZ, Bullet);
                RateOfFire = 1;
            }
            else if(currentMode == AutoMode.TurnOn && Target!=null)
            {
                LookAtEnemy(Target);
                Vector3 dir = Target.position - transform.position;
                GameObject bullet = Spawn(Bullet,Barrel.transform.position);
                TankBullet mBullet = bullet.GetComponent<TankBullet>();
                mBullet.DirectShooting(dir);
                RateOfFire = 1;
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
    public void Die()
    {
        Live = false;
    }
    public void ShootToDirection(Vector2 _direction,float _rotatioZ,GameObject _bullet)
    {
        GameObject bullet = Spawn(_bullet,Barrel.transform.position);
        bullet.transform.rotation = Quaternion.Euler(0, 0, _rotatioZ - 90);
        TankBullet mBullet = bullet.GetComponent<TankBullet>();
        mBullet.DirectShooting(_direction);
    }
    public void ClickToShoot()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 pointMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direct = pointMouse - transform.position;
            float rotationZ = Mathf.Atan2(direct.y, direct.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, rotationZ - 90);
            ShootToDirection(direct, rotationZ, Bullet);
        }
    }
}
