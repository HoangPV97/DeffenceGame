using Spine;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindMeleeEnemy : EnemyController, IWindEffectable
{
    protected override void Update()
    {
        if (Vector3.Distance(transform.position, Tower.transform.position) > 0)
        {
            isMove = true;
            isAttack = false;
        }
        base.Update();
    }
    public void Attack()
    {
        if (Tower != null)
        {
            Tower.GetComponent<Tower>().TakeDamage(enemy.damage);
        }
    }

    public void WindImpactEffect(Vector3 _position)
    {
        GameObject effect = ObjectPoolManager.Instance.SpawnObject("windimpact", _position, Quaternion.identity);
        StartCoroutine(WaitingDestroyEffect(effect, 0.3f));
    }
    public override void CheckAttack()
    {
        base.CheckAttack();
    }
}
