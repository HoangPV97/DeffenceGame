using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindPlayerBullet : BulletController, IExplosionBullet
{

    public void explosionBulletEffect()
    {

    }
    protected override void FixedUpdate()
    {
        if (Target == null || !Target.isLive)
        {
            Target = null;
            StartCoroutine(DelayDespawn(3));
        }
        if (dir == Vector3.zero)
            dir = new Vector3(0, 1, 0);
        Move(dir);

    }
    protected override void OnTriggerEnter2D(Collider2D _Target)
    {
        base.OnTriggerEnter2D(_Target);
        if (_Target.gameObject.tag.Equals(bullet.TargetTag) && !checkCollision)
        {
            checkCollision = true;
            EnemyController enemyController = _Target.GetComponent<EnemyController>();
            enemyController.gameEffect.SpawnEffect("HERO_WIND_BULLET_IMPACT", enemyController.transform.position, 0.5f);
            if (enemyController.enemy.elemental == Elemental.Earth)
            {
                enemyController.DealDamge(bullet.Damage, Mathf.Round(bullet.ATKplus * bullet.Damage / 100));
            }
            else
            {
                enemyController.DealDamge(bullet.Damage, 0);
            }
            if (SeekTarget)
            {
                Despawn();
            }
        }
    }
}


