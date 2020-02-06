using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceExplosionSkill : MonoBehaviour
{
    public SlowSkillData SlowSkillData;
    public ParticleScaler ParticleScaler;
    public ParticleSystem ParticleSystem;
    // Start is called before the first frame update
    void Start()
    {
        ParticleScaler.ScaleByTransform(ParticleSystem, SlowSkillData.EffectedAoe/10, true);
    }
    public virtual void SetSkillData(float EffectedTime, float SlownDownPercent, float Damage, float EffectedAoe)
    {
        SlowSkillData.EffectedTime = EffectedTime;
        SlowSkillData.EffectedAoe = EffectedAoe;
        SlowSkillData.SlowdownPercent = SlownDownPercent;
        SlowSkillData.Damage = Damage;
    }
    private void OnTriggerEnter2D(Collider2D _target)
    {
        if (_target.gameObject.tag.Equals("Enemy"))
        {
            EnemyController enemyController = _target.gameObject.GetComponent<EnemyController>();
            if (enemyController != null)
            {
                enemyController.gameEffect.SpawnEffect("ALLIANCE_ICE_BULLET_IMPACT", enemyController.transform.position, 0.5f);
                enemyController.Deal_Slow_Effect(SlowSkillData.EffectedTime, SlowSkillData.SlowdownPercent);
                var element = enemyController.enemy.elemental;
                int _damage = (int)SlowSkillData.Damage;
                int _damageplus = (int)DataController.Instance.inGameWeapons.ATKplus;
                if (!element.Equals(Elemental.Ice) && enemyController.enemy.Resistance)
                {
                    enemyController.DealDamge(_damage / 2);
                }
                else if (element.Equals(Elemental.Fire))
                {
                    enemyController.DealDamge(_damage, Mathf.Round(_damageplus * _damage / 100));
                }
                else
                {
                    enemyController.DealDamge(_damage);
                }
            }
        }
    }
}
