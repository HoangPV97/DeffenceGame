using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceAllianceBullet : BulletController
{
    private void Start()
    {
        elementalBullet = Elemental.Ice;
        base.Start();
    }
    private void Update()
    {
        if (Target == null || !Target.isLive)
        {
            Despawn();
            return;
        }
        base.Update();
    }
    private void OnTriggerEnter2D(Collider2D Target)
    {
        if (Target.gameObject.tag.Equals(bullet.TargetTag))
        {
            EnemyController enemy = Target.GetComponent<EnemyController>();
            IFireEffectable elemental = enemy.GetComponent<IFireEffectable>();
            if (elemental != null)
            {
                elemental.FireImpactEffect(enemy.transform.position);
                enemy.DealDamge(bullet.Damage, Mathf.Round(damagePlus * bullet.Damage / 100));
            }
            else
            {
                enemy.DealDamge(bullet.Damage, 0);
            }
            Despawn();
        }
    }
}
