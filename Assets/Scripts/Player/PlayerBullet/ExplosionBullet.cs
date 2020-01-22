using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionBullet : BulletController
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        
        if (collision.gameObject.tag.Equals(bullet.TargetTag))
        {
            Debug.Log("EXPLOSION");
            EnemyController enemy = collision.gameObject.GetComponent<EnemyController>();
            enemy?.DealDamge(bullet.Damage, Mathf.Round(bullet.ATKplus * bullet.Damage / 100));
            Despawn();
        }
    }
}
