using Spine;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindMeleeEnemy : EnemyController, IWindEffectable
{

    public void Attack()
    {
        if (Tower != null && isAttack)
        {
            Tower.GetComponent<Tower>().TakeDamage(enemy.damage);
        }
    }

    public void WindImpactEffect(Vector3 _position)
    {
        GameObject effect = ObjectPoolManager.Instance.SpawnObject("windimpact", _position, Quaternion.identity);
        StartCoroutine(WaitingDestroyEffect(effect, 0.3f));
    }
}
