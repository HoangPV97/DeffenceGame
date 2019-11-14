using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : Enemy
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
        if (distancetoPlayer < range )
        {
            CurrentState = EnemyState.Attack;
            if (Player != null)
            {
                Player.GetComponent<Player>().TakeDamge(Damge);
            }
        }
    }
    protected void AutoAttack()
    {
        if (countdown <= 0f)
        {
            Attack();
            countdown = RateOfFire;
        }
        countdown -= Time.deltaTime;
    }
}
