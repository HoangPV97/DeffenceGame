using Spine;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindRangeEnemy : EnemyController, IWindEffectable
{
    [SerializeField] private GameObject Barrel;
    // Update is called once per frame
    //protected override void Update()
    //{
        
    //    base.Update();
    //    CheckAttack();
    //}
    public void Attack()
    {
        if(isAttack)
        {
            GameObject EnemyBullet = ObjectPoolManager.Instance.SpawnObject("windenemybullet", Barrel.transform.position, Quaternion.identity);
            EnemyBullet m_EnemyBullet = EnemyBullet.GetComponent<EnemyBullet>();
            if (m_EnemyBullet != null)
            {
                m_EnemyBullet.SetTarget(Tower.transform);
                m_EnemyBullet.SetDamage(enemy.damage);
                m_EnemyBullet.SetSpeed(enemy.bulletSpeed);
            }
        }
        
    }
    public void WindImpactEffect(Vector3 _position)
    {
        gameEffect.SpawnEffect("windimpact", _position, 0.3f);
    }
}
