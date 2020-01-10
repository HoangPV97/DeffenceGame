using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierSkill : MonoBehaviour
{
    protected float TimeEffect, HealthRecover, EffectedAoe;
    [SerializeField] SkillWeaponEarth2 Swe2;
    protected Tower Tower { get { return GameplayController.Instance.Tower; } }
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("RecoverHealth()",TimeEffect,1f);
    }
    public void SetDataBarrierSkill(float _time,float _health,float _aoe)
    {
        TimeEffect = _time;
        HealthRecover = _health;
        EffectedAoe = _aoe;
    }
    
    public void RecoverHealth()
    {
        Tower.Health.CurrentHealth += HealthRecover;
        Tower.Health.healthBar.fillAmount += HealthRecover / Tower.Health.health;
        if (Tower.Health.CurrentHealth > Tower.Health.health)
        {
            Tower.Health.CurrentHealth = Tower.Health.health;
        }
        Tower.Health.UpdateValueText();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
