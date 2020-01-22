using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthRangeEnemy : EnemyController,IEarthEffectable
{
    [SerializeField] private GameObject Barrel;
    public void Attack()
    {
        if (isAttack)
        {
            GameObject EnemyBullet = ObjectPoolManager.Instance.SpawnObject("WIND_ENEMY_BULLET", Barrel.transform.position, Quaternion.identity);
            EnemyBullet m_EnemyBullet = EnemyBullet.GetComponent<EnemyBullet>();
            if (m_EnemyBullet != null)
            {
                m_EnemyBullet.SetTarget(Tower.transform);
                m_EnemyBullet.SetDamage(enemy.damage);
                m_EnemyBullet.SetSpeed(enemy.bulletSpeed);
            }
        }

    }
    public void EarthImpactEffect(Vector3 _position)
    {
        gameEffect.SpawnEffect("HERO_WIND_BULLET_IMPACT", _position, 0.3f);
    }
}
