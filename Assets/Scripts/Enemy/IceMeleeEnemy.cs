﻿using Spine;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceMeleeEnemy : EnemyController,IIceEffectable
{
    void Start()
    {
        base.Start();
    }
    // Update is called once per frame
    void Update()
    {
        CheckAttack();
        base.Update();
    }
    public void Attack()
    {
            if (Tower != null)
            {
                Tower.GetComponent<Tower>().TakeDamage(enemy.damage);
            }
    }
    public void CheckAttack()
    {
        if (distance <= enemy.range && isLive)
        {
            if (countdown <= 0f && isAttack)
            {
                isAttack = true;
                Rigidbody2D.velocity = Vector2.zero;
                CurrentState = EnemyState.Idle;
                CurrentState = EnemyState.Attack;
                countdown = enemy.rateOfFire;
            }
            countdown -= Time.deltaTime;
        }
    }
    
}
