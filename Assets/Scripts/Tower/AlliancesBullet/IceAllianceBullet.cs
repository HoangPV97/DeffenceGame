using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceAllianceBullet : BulletController
{
    private float number_Bullet;
    private void Start()
    {
        elementalBullet = Elemental.Ice;
        base.Start();
    }
    public override void SetDataBullet(float _speed, float _damage)
    {
        base.SetDataBullet(_speed, _damage);
        number_Bullet = 3f;
    }

    protected override void OnTriggerEnter2D(Collider2D _Target)
    {
        if (_Target.gameObject.tag.Equals(bullet.TargetTag))
        {
            EnemyController enemy = _Target.GetComponent<EnemyController>();
            enemy.gameEffect.SpawnEffect("iceimpact", enemy.transform.position, 0.5f);
            IFireEffectable elemental = enemy.GetComponent<IFireEffectable>();
            if (elemental != null)
            {
                enemy.DealDamge(bullet.Damage, Mathf.Round(damagePlus * bullet.Damage / 100));
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
