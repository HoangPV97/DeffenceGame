using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceAllianceBullet : BulletController
{
    private float IncreaseDamage;
    private float number_Bullet;
    private float percent_Slow = 20;
    protected override void Start()
    {
        elementalBullet = Elemental.Ice;
        base.Start();
    }
    public override void SetDataBullet(float _speed, int _damage, float _atkplus)
    {
        base.SetDataBullet(_speed, _damage, _atkplus);
        number_Bullet = 3f;
    }

    protected override void OnTriggerEnter2D(Collider2D _Target)
    {
        if (_Target.gameObject.tag.Equals(bullet.TargetTag))
        {
            EnemyController enemy = _Target.GetComponent<EnemyController>();
            enemy.gameEffect.SpawnEffect("iceimpact", enemy.transform.position, 0.5f);
            enemy.Deal_Slow_Effect(2f, percent_Slow);
            IFireEffectable elemental = enemy.GetComponent<IFireEffectable>();
            if (elemental != null)
            {
                enemy.DealDamge(bullet.Damage, Mathf.Round(bullet.ATKplus * bullet.Damage / 100));
            }
            else
            {
                enemy.DealDamge(bullet.Damage, 0);
            }
            Despawn();
        }
        base.OnTriggerEnter2D(_Target);
    }
}
