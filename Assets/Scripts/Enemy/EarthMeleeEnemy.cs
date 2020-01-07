using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthMeleeEnemy : EnemyController,IEarthEffectable
{
    public void Attack()
    {
        if (Tower != null && isAttack)
        {
            Tower.GetComponent<Tower>().TakeDamage(enemy.damage);
        }
    }
    public void EarthImpactEffect(Vector3 _position)
    {
        gameEffect.SpawnEffect("windimpact", _position, 0.3f);
    }
}
