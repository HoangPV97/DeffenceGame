using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcePlayerBullet : BulletController
{
    protected override void FixedUpdate()
    {
        if (GameplayController.Instance.PlayerController.currentMode == AutoMode.TurnOff)
            Move(dir);
        else
        {
            base.FixedUpdate();
        }
    }
    protected override void OnTriggerEnter2D(Collider2D _Target)
    {
        base.OnTriggerEnter2D(_Target);
        if (_Target.gameObject.tag.Equals(bullet.TargetTag) && !checkCollision)
        {
            checkCollision = true;
            EnemyController enemyController = _Target.GetComponent<EnemyController>();
            if (enemyController != null)
            {
                enemyController.gameEffect.SpawnEffect("HERO_WIND_BULLET_IMPACT", enemyController.transform.position, 0.5f);
                if (enemyController.enemy.elemental == Elemental.Fire)
                {
                    enemyController.DealDamge(bullet.Damage, Mathf.Round(bullet.ATKplus * bullet.Damage / 100));
                }
                else
                {
                    enemyController.DealDamge(bullet.Damage, 0);
                }
                if (poison)
                {
                    enemyController.DealPoison(poisonDamage, 2.5f);
                }
                Despawn();
            }
        }
    }
}
