using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayThunderStrikeSkill : DragAndDropSkill
{
    [SerializeField]
    SkillWeaponFire1 Swf1;
    [SerializeField]
    SkillAllianceWind2 SkillAllianceWind2;
    SkillAllianceWind3 SkillAllianceWind3;
    protected override void Start()
    {
        circle.transform.localScale *= EffectedAoe / 10;
        base.Start();
    }
    public override void Update()
    {
        base.Update();
        if (TimeLeft <= 0 && Tower.Mana.CurrentMana >= manaCost && variableJoystick.Vertical != 0)
        {
            Vector3 MousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = MousePosition - circle.transform.position;
            MoveObject(circle, direction);
        }
    }
    public override void SetUpData(int Tier = 1, int Level = 1, VariableJoystick variableJoystick = null, Vector3 _position = default)
    {
        base.SetUpData(Tier, Level);
        //SkillAllianceWind2 = JsonUtility.FromJson<SkillAllianceWind2>(ConectingFireBase.Instance.GetTextSkill("ALLIANCE_WIND_SKILL_2"));
        //float BuffTimeEffectStats = 0;
        //float BuffDamageStats = 0;

        //if (DataController.Instance.GetGameAlliance(Elemental.Wind).WeaponTierLevel.Tier > 1)
        //{
        //    var SkilldataSaver = DataController.Instance.GetGameAlliance(Elemental.Wind).GetSkillTierLevel("ALLIANCE_WIND_SKILL_2");
        //    BuffTimeEffectStats = SkillAllianceWind2.GetSkillAttributes("IncreaseTimeEffect", SkilldataSaver.Tier, SkilldataSaver.Level);
        //}
        
        //if(DataController.Instance.GetGameAlliance(Elemental.Wind).WeaponTierLevel.Tier > 2)
        //{
        //    var SkilldataSaver = DataController.Instance.GetGameAlliance(Elemental.Wind).GetSkillTierLevel("ALLIANCE_WIND_SKILL_3");
        //    BuffDamageStats = SkillAllianceWind3.GetSkillAttributes("IncreaseDamage", SkilldataSaver.Tier, SkilldataSaver.Level);
        //}
        Swf1 = JsonUtility.FromJson<SkillWeaponFire1>(ConectingFireBase.Instance.GetTextSkill(SkillID));
        this.variableJoystick = variableJoystick;
        manaCost = Swf1.GetManaCost(Tier, Level);
        CountdownTime = Swf1.GetCoolDown(Tier, Level);
        EffectTime = Swf1.GetSkillAttributes("TimeEffect", Tier, Level);
        //EffectTime += EffectTime * BuffTimeEffectStats / 100;
        Damage = Swf1.GetDamage(Tier, Level);
        //Damage += Damage * BuffDamageStats / 100;
        EffectedAoe = Swf1.GetSkillAttributes("EffectedAoe", Tier, Level);
        variableJoystick.SetUpData(this);
        positonEffect = _position;
        CountdownGo = variableJoystick.CountDountMask;
    }
    public override void PlaySkill(Vector3 _position)
    {
        GameObject stunSkill = ObjectPoolManager.Instance.SpawnObject(SkillID, _position, Quaternion.identity);
        stunSkill.GetComponent<ThunderStrikeSkill>().SetSkillData(EffectTime, Damage, EffectedAoe);    
        float particleTime = stunSkill.GetComponentInChildren<ParticleSystem>().main.duration;
        GameObject effectStart = SpawnEffect(EffectName, positonEffect, 1f);
        CheckDestroyEffect(stunSkill, particleTime);
    }
    public override void AddDatatAttribute(string _attribute, float _value)
    {
        if(_attribute== "TimeEffect")
        {
            if(_value > 5f) // Percent
                EffectTime += EffectTime *_value/100;
            else //Value
                EffectTime += _value;
        }
    }
}
