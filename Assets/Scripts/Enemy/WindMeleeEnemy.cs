using Spine;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindMeleeEnemy : EnemyController,IWindEffectable
{
    void Start()
    {
        base.Start();
    }
    // Update is called once per frame
    void Update()
    {
        CheckAttack();
        base.Update();
    }
    public void Attack()
    {
        if (Tower != null)
        {

            Tower.GetComponent<Tower>().TakeDamage(enemy.damage);
            gameEffect.SpawnEffect("WindMeleeImpact", this.transform.position+new Vector3(0,-1,0), 0.5f);
        }
    }
    public void CheckAttack()
    {
        if (distance <= enemy.range && isLive)
        {
            if (countdown <= 0f && isAttack)
            {
                Rigidbody2D.velocity = Vector2.zero;
                CurrentState = EnemyState.Idle;
                CurrentState = EnemyState.Attack;
                countdown = enemy.rateOfFire;
            }
            countdown -= Time.deltaTime;
        }
    }

}
