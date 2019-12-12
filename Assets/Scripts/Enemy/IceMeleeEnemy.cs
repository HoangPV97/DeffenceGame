using Spine;
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

    public void IceImpactEffect(Vector3 _position)
    {
        GameObject effect = ObjectPoolManager.Instance.SpawnObject("iceimpact", _position, Quaternion.identity);
        StartCoroutine(WaitingDestroyEffect(effect, 0.3f));
    }
}
