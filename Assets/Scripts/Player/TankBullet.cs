﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankBullet : BulletController
{
    public void Start()
    {
        base.Start();
    }
    private void Update()
    {

        //base.Update();
    }
    // Update is called once per frame
    public void DirectShooting(Vector2 _direction)
    {
        Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
        GetComponent<Rigidbody2D>().velocity = _direction.normalized * 50 * bullet.Speed * Time.deltaTime;
    }
    protected void OnTriggerEnter2D(Collider2D Target)
    {
        if (Target.gameObject.tag.Equals(bullet.TargetTag) || Target.gameObject.tag.Equals("BlockPoint"))
        {
            EnemyController enemy = Target.GetComponent<EnemyController>();
            //enemy.CurrentState = EnemyState.Hurt;

            //Elemental elemental= enemy.GetComponent<I>
            enemy?.DealDamge( bullet.Damage, Mathf.Round(damagePlus * bullet.Damage / 100));
            if (SeekTarget)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
