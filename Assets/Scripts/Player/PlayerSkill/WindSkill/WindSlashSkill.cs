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
        particleScaler.ScaleByTransform(ParticleSystem, (effectedAoe / 10), true);
    }
    // Start is called before the first frame update
    public void setDataSkill(float _knockback, float _effectAoe)
    {
        this.KnockBackDistance = _knockback;
        this.effectedAoe = _effectAoe;
        //if (effectedAoe != 0)
        //{
        //    collider2D.size = new Vector2(effectedAoe, 1);
        //}
    }
    public override void SetDataBullet(float _speed, int _damage, float _atkplus)
    {
        listCheckColision = new List<GameObject>();
        base.SetDataBullet(_speed, _damage, _atkplus);
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
                EnemyController enemyController = _Target.GetComponent<EnemyController>();
                listCheckColision.Add(_Target.gameObject);
                if (enemyController != null)
                {
                    enemyController.gameEffect.SpawnEffect("HERO_WIND_BULLET_IMPACT", enemyController.transform.position, 0.5f);
                    var element = enemyController.enemy.elemental;
                    if (!element.Equals(Elemental.Wind) && enemyController.enemy.Resistance)
                    {
                        enemyController.DealDamge(Mathf.RoundToInt(bullet.Damage / 2));
                    }
                    else if (element.Equals(Elemental.Earth))
                    {
                        enemyController.DealDamge(bullet.Damage, Mathf.Round(bullet.ATKplus * bullet.Damage / 100));
                    }
                    else
                    {
                        enemyController.DealDamge(bullet.Damage);
                    }
                    enemyController.KnockBack(KnockBackDistance);
                    return;
                }
            }
        }
    }
}