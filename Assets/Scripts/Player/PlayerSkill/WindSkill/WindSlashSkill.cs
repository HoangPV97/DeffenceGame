using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindSlashSkill : BulletController
{
    List<GameObject> listCheckColision;
    float KnockBackDistance;
    float effectedAoe;
    public ParticleScaler particleScaler;
    public ParticleSystem ParticleSystem;
    protected override void Start()
    {
        particleScaler.ScaleByTransform(ParticleSystem, (effectedAoe/10), true);
    }
    // Start is called before the first frame update
    public void setDataSkill( float _knockback,float _effectAoe )
    {
        this.KnockBackDistance = _knockback;
        this.effectedAoe = _effectAoe;
        //if (effectedAoe != 0)
        //{
        //    collider2D.size = new Vector2(effectedAoe, 1);
        //}
    }
    public override void SetDataBullet(float _speed, int _damage)
    {
        listCheckColision = new List<GameObject>();
        base.SetDataBullet(_speed, _damage);
    }
    protected override void OnTriggerEnter2D(Collider2D _Target)
    {
        if (_Target.gameObject.tag.Equals("BlockPoint"))
        {
            Despawn();
        }
        if (_Target.gameObject.tag.Equals(bullet.TargetTag))
        {
            if (!listCheckColision.Contains(_Target.gameObject))
            {
                EnemyController enemy = _Target.GetComponent<EnemyController>();
                listCheckColision.Add(_Target.gameObject);
                if (enemy != null)
                {
                    enemy.gameEffect.SpawnEffect("HERO_WIND_BULLET_IMPACT", enemy.transform.position, 0.5f);
                    IEarthEffectable elemental = enemy.GetComponent<IEarthEffectable>();
                    if (elemental != null)
                    {
                        enemy.DealDamge(bullet.Damage, Mathf.Round(damagePlus * bullet.Damage / 100));
                    }
                    else
                    {
                        enemy.DealDamge(bullet.Damage);
                    }
                    enemy.KnockBack( KnockBackDistance);
                    return;
                }

            }
        }
    }

}
