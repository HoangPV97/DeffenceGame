using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : Characters
{
    private void Start()
    {
        base.Start();
    }
    private void Update()
    {
        AutoShoot();
        base.Update();
    }
    protected void Shoot()
    {
        if (Target !=null)
        {
            characterState = CharacterState.Attack;
            GameObject bullet = Spawn("towerbullet", Barrel.position);
            Bullet towerBullet = bullet.GetComponent<Bullet>();
            towerBullet.elementalBullet = elementalType;
            towerBullet?.SetTarget(Target);
        }
        else
        {
            characterState = CharacterState.Idle;
        }
    }
    private void AutoShoot()
    {
        if (RateOfFire <= 0f)
        {
            Shoot();
            RateOfFire = 1f;
        }
        RateOfFire -= Time.deltaTime;
    }
}
