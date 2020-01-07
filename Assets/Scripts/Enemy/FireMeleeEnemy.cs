using Spine;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireMeleeEnemy : EnemyController, IFireEffectable
{
    void Start()
    {
        base.Start();
    }

    // Update is called once per frame
   
    public void Attack()
    {
        if (Tower != null)
        {
            Tower.TakeDamage(enemy.damage);
        }
    }
 

    public void FireImpactEffect(Vector3 _position)
    {
        throw new System.NotImplementedException();
    }
}
