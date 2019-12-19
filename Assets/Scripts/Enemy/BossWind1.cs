using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BossWindData : MonsterData
{

}
public class BossWind1 : EnemyController
{
    void Start()
    {
        base.Start();
    }
    // Update is called once per frame
    protected override void Update()
    {
        CheckAttack();
        base.Update();
    }
    public void Attack()
    {
        GameObject EnemyBullet = ObjectPoolManager.Instance.SpawnObject("windenemybullet", transform.position, Quaternion.identity);
        EnemyBullet m_EnemyBullet = EnemyBullet.GetComponent<EnemyBullet>();
        if (m_EnemyBullet != null)
        {
            m_EnemyBullet.SetTarget(Tower.transform);
            m_EnemyBullet.SetDamage(enemy.damage);
        }
    }
    public override void CheckAttack()
    {
        distancetoTower = Mathf.Abs(transform.position.y - Tower.transform.position.y);
        if (distancetoTower < enemy.range && isLive)
        {
            if (countdown <= 0f && !isAttack)
            {
                isAttack = true;
                Rigidbody2D.velocity = Vector2.zero;
                CurrentState = EnemyState.Idle;
                CurrentState = EnemyState.Attack;
                countdown = enemy.rateOfFire;
            }
            countdown -= Time.deltaTime;
        }
        if (enemy.health.CurrentHealth <= enemy.health.health / 2)
        {
            ChargeAttack();
        }
    }

    private void ChargeAttack()
    {
        //Charge
    }

    public void WindImpactEffect(Vector3 _position)
    {
        GameObject effect = ObjectPoolManager.Instance.SpawnObject("windimpact", _position, Quaternion.identity);
        StartCoroutine(WaitingDestroyEffect(effect, 0.3f));
    }
}
