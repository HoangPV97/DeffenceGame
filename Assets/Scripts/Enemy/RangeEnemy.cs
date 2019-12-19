using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeEnemy : EnemyController
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
        distancetoTower = Vector3.Distance(transform.position, Tower.transform.position);
        if (distancetoTower < enemy.range )
        {
            CurrentState = EnemyState.Attack;
            isMove = false;
            Move(enemy.speed); 
            GameObject EnemyBullet = ObjectPoolManager.Instance.SpawnObject("enemybullet", transform.position, Quaternion.identity);
            EnemyBullet m_EnemyBullet = EnemyBullet.GetComponent<EnemyBullet>();
            if (m_EnemyBullet != null)
            {
                m_EnemyBullet.SetTarget(Tower.transform);
                m_EnemyBullet.SetDamage(enemy.damage);
            }
        }
    }
    public void AutoAttack()
    {
        if (countdown <= 0f)
        {
            Attack();
            countdown = enemy.rateOfFire;
        }
        countdown -= Time.deltaTime;
    }
}
