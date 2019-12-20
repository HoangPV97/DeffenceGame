﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindAllianceBullet : BulletController
{
    protected override void Start()
    {
        elementalBullet = Elemental.Wind;
    }

    protected override void OnTriggerEnter2D(Collider2D Target)
    {
        base.OnTriggerEnter2D(Target);
        if (Target.gameObject.tag.Equals(bullet.TargetTag))
        {
            EnemyController enemy = Target.GetComponent<EnemyController>();
            enemy.gameEffect.SpawnEffect("windimpact", enemy.transform.position, 0.5f);
            IIceEffectable elemental = enemy?.GetComponent<IIceEffectable>();
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
    }
}
