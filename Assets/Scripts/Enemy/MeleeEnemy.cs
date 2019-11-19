using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : EnemyController
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

        distancetoPlayer = Vector3.Distance(transform.position, Tower.transform.position);
        if (distancetoPlayer < enemy.range )
        {
            CurrentState = EnemyState.Attack;
            if (Tower != null)
            {
                Tower.GetComponent<Tower>().TakeDamage(enemy.damage);
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
