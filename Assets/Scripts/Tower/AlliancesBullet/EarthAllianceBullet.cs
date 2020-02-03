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
            EnemyController enemy = Target.GetComponent<EnemyController>();
            enemy.gameEffect.SpawnEffect("HERO_EARTH_BULLET_IMPACT", enemy.transform.position, 0.5f);
            IIceEffectable elemental = enemy?.GetComponent<IIceEffectable>();
            if (elemental != null)
            {
                enemy.DealDamge(bullet.Damage, Mathf.Round(bullet.ATKplus * bullet.Damage / 100));
            }
            else
            {
                enemy.DealDamge(bullet.Damage, 0);
            }
            if (stun)
            {
                enemy.DealEffect(Effect.StunBullet, enemy.transform.position, StunTime);
            }
            if (SeekTarget)
            {
                Despawn();
            }
        }
    }
}
