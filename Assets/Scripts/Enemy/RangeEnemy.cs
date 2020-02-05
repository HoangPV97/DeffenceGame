using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeEnemy : EnemyController
{
    [SerializeField] private string BulletName;
    public override void SetUpdata(string type, int Level)
    {
        base.SetUpdata(type, Level);
        enemy.range += Random.Range(-1, 1);
    }
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
}
