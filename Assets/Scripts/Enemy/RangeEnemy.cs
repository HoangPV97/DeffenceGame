using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeEnemy : Enemy
{
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        AutoAttack();
        base.Update();
    }
    public void Attack()
    {
        
        distancetoPlayer = Vector3.Distance(transform.position, Player.transform.position);
        if (distancetoPlayer < range)
        {
            CurrentState = EnemyState.Attack;
            GameObject EnemyBullet = PoolManager.SpawnObject("enemybullet", transform.position, Quaternion.identity);
            EnemyBullet m_EnemyBullet = EnemyBullet.GetComponent<EnemyBullet>();
            if (m_EnemyBullet != null)
            {
                m_EnemyBullet.SetTarget(Player.transform);
                m_EnemyBullet.SetDamage(Damge);
            }
        }
        base.Attack();
    }
    public void AutoAttack()
    {
        if (countdown <= 0f)
        {
            Attack();
            countdown = RateOfFire;
        }
        countdown -= Time.deltaTime;
        base.AutoAttack();
    }
}
