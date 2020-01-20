using Spine;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindMeleeEnemy : EnemyController, IWindEffectable
{

    public void Attack()
    {
        if (Tower != null && isAttack && !disableAttack)
        {
            gameEffect.SpawnEffect("WIND_MELEE_IMPACT", gameObject.transform.position - new Vector3(0, 0.5f, 0), 0.5f);
            Tower.TakeDamage(enemy.damage);
        }
    }

    public void WindImpactEffect(Vector3 _position)
    {
        gameEffect.SpawnEffect("windimpact", _position, 0.3f);
    }
}
