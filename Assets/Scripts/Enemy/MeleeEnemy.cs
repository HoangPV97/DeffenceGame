using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : EnemyController
{
    // Update is called once per frame
    protected override void Update()
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
            if (Tower != null)
            {
                Tower.TakeDamage(enemy.damage);
            }
        }
    }
    protected void AutoAttack()
    {
        if (countdown <= 0f)
        {
            Attack();
            countdown = enemy.rateOfFire;
        }
        countdown -= Time.deltaTime;
    }
}
