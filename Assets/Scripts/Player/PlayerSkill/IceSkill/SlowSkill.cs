using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region IceExplosionData
[System.Serializable]
public class SlowSkillData
{
    public float EffectedTime;
    public float EffectedAoe;
    public float SlowdownPercent;
    public float Damage;
    public SlowSkillData(float _effectedTime, float _EffectedAoe, float _SlowdownPercent, float _Damage)
    {
        this.EffectedTime = _effectedTime;
        this.EffectedAoe = _EffectedAoe;
        this.SlowdownPercent = _SlowdownPercent;
        this.Damage = _Damage;
    }
}
#endregion
public class SlowSkill : MonoBehaviour
{
    public SlowSkillData SlowSkillData;
    //public ParticleScaler ParticleScaler;
    //public ParticleSystem ParticleSystem;
    //List<EnemyController> enemies;
    // Start is called before the first frame update
    void Start()
    {
        //ParticleScaler.ScaleByTransform(ParticleSystem, SlowSkillData.EffectedAoe/10, true);
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
                enemyController.Deal_Slow_Effect(SlowSkillData.EffectedTime, SlowSkillData.SlowdownPercent);
                var element = enemyController.enemy.elemental;
                int _damage = (int)SlowSkillData.Damage;
                int _damageplus = (int)DataController.Instance.inGameWeapons.ATKplus;
                if (!element.Equals(Elemental.Earth) && enemyController.enemy.Resistance)
                {
                    enemyController.DealDamge(_damage / 2);
                }
                else if (element.Equals(Elemental.Ice))
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
