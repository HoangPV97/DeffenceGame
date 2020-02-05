using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : EnemyController
{
    protected override void Start()
    {

        base.Start();
    }
    public void Attack()
    {
        if (Tower != null && isAttack && !disableAttack)
        {
            gameEffect.SpawnEffect("WIND_MELEE_IMPACT", gameObject.transform.position - new Vector3(0, 0.5f, 0), 0.5f);
            Tower.TakeDamage(enemy.damage);
        }
    }
}
