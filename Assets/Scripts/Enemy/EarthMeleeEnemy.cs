using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthMeleeEnemy : EnemyController,IEarthEffectable
{
    public void Attack()
    {
        if (Tower != null && isAttack)
        {
            Tower.TakeDamage(enemy.damage);
        }
    }
    public void EarthImpactEffect(Vector3 _position)
    {
        gameEffect.SpawnEffect("HERO_WIND_BULLET_IMPACT", _position, 0.3f);
    }
}
