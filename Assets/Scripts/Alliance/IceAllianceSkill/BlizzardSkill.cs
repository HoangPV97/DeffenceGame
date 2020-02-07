using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlizzardSkill : BulletController
{
    public ParticleSystem ParticleSystem;
    public ParticleScaler ParticleScaler;
    public float EffectedTime = 3, EffectedAoe = 10;
    // Start is called before the first frame update
    protected override void Start()
    {
        ParticleScaler.ScaleByTransform(ParticleSystem, EffectedAoe / 10, true);
        elementalBullet = Elemental.Ice;
    }
    public void SetSkillData(float _EffectedTime, float _Damage, float _EffectedAoe)
    {
        EffectedTime = _EffectedTime;
        bullet.Damage = Mathf.RoundToInt(_Damage);
        EffectedAoe = _EffectedAoe;
    }

    protected override void OnTriggerEnter2D(Collider2D Target)
    {
        if (Target.gameObject.tag.Equals(bullet.TargetTag))
        {
            EnemyController enemyController = Target.GetComponent<EnemyController>();
            if (enemyController != null)
            {
                enemyController.gameEffect.SpawnEffect("ALLIANCE_ICE_BULLET_IMPACT", enemyController.transform.position, 0.5f);
                var element = enemyController.enemy.elemental;
                var selectedLevel = DataController.Instance.StageData.Level;
                if (element.Equals(Elemental.Ice) && enemyController.enemy.Resistance <= selectedLevel)
                {
                    enemyController.DealDamge(Mathf.RoundToInt(bullet.Damage / 2));
                }
                else if (element.Equals(Elemental.Fire))
                {
                    enemyController.DealDamge(bullet.Damage, Mathf.Round(bullet.ATKplus * bullet.Damage / 100));
                }
                else
                {
                    enemyController.DealDamge(bullet.Damage);
                }
                enemyController.DealEffect(Effect.Freeze, enemyController.transform.position, EffectedTime);
            }
            Despawn();
        }
    }
}
