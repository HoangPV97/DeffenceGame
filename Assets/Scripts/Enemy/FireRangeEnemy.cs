using System;
using System.Collections;
using System.Collections.Generic;
using Spine;
using Spine.Unity;
using UnityEngine;

public class FireRangeEnemy : EnemyController, IFireEffectable
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
    public new void Attack()
    {
        GameObject EnemyBullet = ObjectPoolManager.Instance.SpawnObject("fireenemybullet", transform.position, Quaternion.identity);
        EnemyBullet m_EnemyBullet = EnemyBullet.GetComponent<EnemyBullet>();
        if (m_EnemyBullet != null)
        {
            m_EnemyBullet.SetTarget(Tower.transform);
            m_EnemyBullet.SetDamage(enemy.damage);
            m_EnemyBullet.SetSpeed(enemy.bulletSpeed);
        }
    }
    public void CheckAttack()
    {
        if (distance <= enemy.range && isLive)
        {
            if (countdown <= 0f && isAttack)
            {
                Rigidbody2D.velocity = Vector2.zero;
                CurrentState = EnemyState.Idle;
                CurrentState = EnemyState.Attack;
                countdown = enemy.rateOfFire;
            }
            countdown -= Time.deltaTime;
        }
    }
}
