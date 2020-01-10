using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireAllianceBullet : BulletController
{
    Transform Taget;
    protected override void Start()
    {
        elementalBullet = Elemental.Fire;
        base.Start();
    }
    protected override void FixedUpdate()
    {
        if (Target == null || !Target.isLive)
        {
            Despawn();
            return;
        }
        base.FixedUpdate();
    }
    protected override void OnTriggerEnter2D(Collider2D Target)
    {
        if (Target.gameObject.tag.Equals(bullet.TargetTag))
        {
            EnemyController enemy = Target.GetComponent<EnemyController>();
            enemy.gameEffect.SpawnEffect("fireimpact", enemy.transform.position, 0.5f);
            IWindEffectable elemental = enemy.GetComponent<IWindEffectable>();
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
        base.OnTriggerEnter2D(Target);
    }
}
