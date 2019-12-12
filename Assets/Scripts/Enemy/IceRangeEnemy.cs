﻿using Spine;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceRangeEnemy : EnemyController,IIceEffectable
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


        GameObject EnemyBullet = ObjectPoolManager.Instance.SpawnObject("iceenemybullet", transform.position, Quaternion.identity);
        EnemyBullet m_EnemyBullet = EnemyBullet.GetComponent<EnemyBullet>();
        if (m_EnemyBullet != null)
        {
            m_EnemyBullet.SetTarget(Tower.transform);
            m_EnemyBullet.SetDamage(enemy.damage);
        }
    }

    public void IceImpactEffect(Vector3 _position)
    {
        GameObject effect = ObjectPoolManager.Instance.SpawnObject("iceimpact", _position, Quaternion.identity);
        StartCoroutine(WaitingDestroyEffect(effect, 0.3f));
    }
}
