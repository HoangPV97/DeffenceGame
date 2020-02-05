using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthAllianceBullet : BulletController
{
    public bool stun;
    public float StunTime;
    // Start is called before the first frame update
    protected override void Start()
    {
        elementalBullet = Elemental.Earth;
    }
    public void SetDataBullet(float _speed, int _damage, float _stunTime, int _increasedamage)
    {
        StunTime = _stunTime;
        base.SetDataBullet(_speed, _damage, _increasedamage);
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
            return;
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
        if (Target.gameObject.tag.Equals(bullet.TargetTag) && !checkCollision)
        {
            checkCollision = true;
            EnemyController enemyController = Target.GetComponent<EnemyController>();
            enemyController.gameEffect.SpawnEffect("HERO_EARTH_BULLET_IMPACT", enemyController.transform.position, 0.5f);
            if (enemyController.enemy.elemental.Equals(Elemental.Ice))
            {
                enemyController.DealDamge(bullet.Damage, Mathf.Round(bullet.ATKplus * bullet.Damage / 100));
            }
            else
            {
                enemyController.DealDamge(bullet.Damage, 0);
            }
            if (stun)
            {
                enemyController.DealEffect(Effect.StunBullet, enemyController.transform.position, StunTime);
            }
            Despawn();
        }
    }
}
