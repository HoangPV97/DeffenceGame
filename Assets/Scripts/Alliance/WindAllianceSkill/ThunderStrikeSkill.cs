using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ThunderStrikeSkill : MonoBehaviour
{
    public ParticleScaler ParticleScaler;
    public ParticleSystem ParticleSystem;
    public float EffectedTime, Damage, EffectedAoe;
    private void Start()
    {
        ParticleScaler.ScaleByTransform(ParticleSystem, (EffectedAoe/10), true);
    }
    // Start is called before the first frame update
    public virtual void SetSkillData(float _EffectedTime, float _Damage, float _EffectedAoe)
    {
        EffectedTime = _EffectedTime;
        Damage = _Damage;
        EffectedAoe = _EffectedAoe;
    }
    protected void OnTriggerEnter2D(Collider2D Target)
    {
        if (Target.gameObject.tag.Equals("Enemy"))
        {
            int tier = DataController.Instance.GetGameAlliance(Elemental.Wind).WeaponTierLevel.Tier;
            float _damageplus = (int)DataController.Instance.AllianceDataBases.GetAlliance(Elemental.Wind, tier).weapons.ATKplus;
            EnemyController enemyController = Target.GetComponent<EnemyController>();
            if (enemyController != null)
            {
                var element = enemyController.enemy.elemental;
                if (!element.Equals(Elemental.Wind) && enemyController.enemy.Resistance)
                {
                    enemyController.DealDamge(Mathf.RoundToInt(Damage / 2));
                }
                else if (element.Equals(Elemental.Earth))
                {
                    enemyController.DealDamge((int)Damage, (int)(_damageplus * Damage / 100));
                }
                else
                {
                    enemyController.DealDamge(Mathf.RoundToInt(Damage));
                }
                enemyController.DealEffect(Effect.Stun, enemyController.transform.position + new Vector3(0, 0.5f, 0), EffectedTime);
            }
        }
    }
}
