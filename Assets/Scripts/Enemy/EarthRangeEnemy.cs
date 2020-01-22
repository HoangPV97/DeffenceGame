using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthRangeEnemy : EnemyController,IEarthEffectable
{
    [SerializeField] private string BulletName;
    [SerializeField] private GameObject Barrel;
    public void Attack()
    {
        if (isAttack)
        {
            GameObject EnemyBullet = ObjectPoolManager.Instance.SpawnObject(BulletName, Barrel.transform.position, Quaternion.identity);
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
        gameEffect.SpawnEffect("ENEMY_EARTH_IMPACT", _position, 0.3f);
    }
}
