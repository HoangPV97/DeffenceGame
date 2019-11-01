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
        if (Bullet != null)
        {
            GameObject bullet = Spawn(Bullet, Barrel.transform.position);
            Bullet towerBullet = bullet.GetComponent<Bullet>();
            towerBullet?.SetTarget(Target);
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
