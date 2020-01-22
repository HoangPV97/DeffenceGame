using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankBullet : BulletController
{
    protected override void OnTriggerEnter2D(Collider2D Target)
    {
        if (Target.gameObject.tag.Equals(bullet.TargetTag) || Target.gameObject.tag.Equals("BlockPoint"))
        {
            EnemyController enemy = Target.GetComponent<EnemyController>();
            //enemy.CurrentState = EnemyState.Hurt;

            //Elemental elemental= enemy.GetComponent<I>
            enemy?.DealDamge( bullet.Damage, Mathf.Round(bullet.ATKplus * bullet.Damage / 100));
            if (SeekTarget)
            {
                Despawn();
            }
        }
    }
}
