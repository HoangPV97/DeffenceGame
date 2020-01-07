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
            Tower.TakeDamage(enemy.damage);
        }
    }

    public void WindImpactEffect(Vector3 _position)
    {
        gameEffect.SpawnEffect("windimpact", _position, 0.3f);
    }
}
