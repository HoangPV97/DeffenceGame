using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceAllianceBullet : BulletController
{
    private float SlowPercent = 0;
    protected override void Start()
    {
        elementalBullet = Elemental.Ice;
        base.Start();
    }
    public void SetDataAllyBullet(float _speed, int _damage, float _atkplus, int _slowPercent)
    {
        SlowPercent = _slowPercent;
        base.SetDataBullet(_speed, _damage, _atkplus);
    }

    protected override void OnTriggerEnter2D(Collider2D _Target)
    {
        if (_Target.gameObject.tag.Equals(bullet.TargetTag))
        {
            EnemyController enemyController = _Target.GetComponent<EnemyController>();
            if (enemyController != null)
            {
                enemyController.gameEffect.SpawnEffect("HERO_ICE_BULLET_IMPACT", enemyController.transform.position, 0.5f);
                enemyController.Deal_Slow_Effect(2f, SlowPercent);
                if (enemyController.enemy.elemental.Equals(Elemental.Fire))
                {
                    enemyController.DealDamge(bullet.Damage, Mathf.Round(bullet.ATKplus * bullet.Damage / 100));
                }
                else
                {
                    enemyController.DealDamge(bullet.Damage, 0);
                }
                Despawn();
            }     
        }
        base.OnTriggerEnter2D(_Target);
    }
}
