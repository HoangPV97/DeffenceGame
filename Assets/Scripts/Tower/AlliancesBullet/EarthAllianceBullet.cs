﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthAllianceBullet : BulletController
{
    public float StunTime;
    // Start is called before the first frame update
    protected override void Start()
    {
        elementalBullet = Elemental.Earth;
    }
    public void SetDataBullet(float _speed, int _damage, float _stunTime, int _increasedamage)
    {
        StunTime = _stunTime;
        damagePlus = _increasedamage;
        base.SetDataBullet(_speed, _damage);
    }
    protected override void FixedUpdate()
    {
        if (Target == null || !Target.isLive)
        {
            Target = null;
            Move(dir);
        }
        else
        {
            dir = Target.transform.position - transform.position;
            Move(dir);
        }
        if (dir.magnitude < 0.1f)
        {
            Despawn();
        }

    }
    protected override void OnTriggerEnter2D(Collider2D Target)
    {
        if (Target.gameObject.tag.Equals("BlockPoint"))
        {
            Despawn();
        }
        if (Target.gameObject.tag.Equals(bullet.TargetTag))
        {
            EnemyController enemy = Target.GetComponent<EnemyController>();
            enemy.gameEffect.SpawnEffect("HERO_WIND_BULLET_IMPACT", enemy.transform.position, 0.5f);
            IIceEffectable elemental = enemy?.GetComponent<IIceEffectable>();
            if (elemental != null)
            {
                enemy.DealDamge(bullet.Damage, Mathf.Round(damagePlus * bullet.Damage / 100));
            }
            else
            {
                enemy.DealDamge(bullet.Damage, 0);
            }
            if (stun)
            {
                enemy.DealEffect(Effect.StunBullet, enemy.transform.position, StunTime);
            }
        }
    }
}