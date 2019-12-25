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

    public void FireImpactEffect(Vector3 _position)
    {
        gameEffect.SpawnEffect("fireimpact", _position, 0.3f);
    }
}
