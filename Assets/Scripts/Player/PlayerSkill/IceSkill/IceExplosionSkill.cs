﻿using System.Collections;
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
            enemyController.Deal_Slow_Effect(SlowSkillData.EffectedTime, SlowSkillData.SlowdownPercent);
            enemyController.DealDamge((int)SlowSkillData.Damage);
        }
    }
}
