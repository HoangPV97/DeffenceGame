using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayEarthDragonSkill : DragAndDropSkill
{
    protected float TimeEffect, SlowdownPercent, EffectedAoe, Damage;
    [SerializeField] SkillWeaponEarth1 Swe1;
    public override void SetUpData(int Tier = 1, int Level = 1, VariableJoystick variableJoystick = null, Vector3 _position = default)
    {
        base.SetUpData(Tier, Level);
        Swe1 = JsonUtility.FromJson<SkillWeaponEarth1>(ConectingFireBase.Instance.GetTextSkill(SkillID));
        this.variableJoystick = variableJoystick;
        manaCost = Swe1.GetManaCost(Tier, Level);
        TimeEffect = Swe1.GetSkillAttributes("TimeEffect", Tier, Level);
        EffectedAoe = Swe1.GetSkillAttributes("EffectedAoe", Tier, Level);
        SlowdownPercent = Swe1.GetSkillAttributes("SlowdownPercent", Tier, Level);
        Damage = Swe1.GetDamage(Tier, Level);
        CountdownTime = Swe1.GetCoolDown(Tier, Level);
        variableJoystick.SetUpData(this);
        positonEffect = _position;
        CountdownGo = variableJoystick.CountDountMask;
    }
    public override void PlaySkill(Vector3 _position)
    {
        GameObject Earth_Dragon_Skill = SpawnEffect(SkillID, _position, TimeEffect);
        Earth_Dragon_Skill.GetComponent<SlowSkill>().SetSkillData(TimeEffect, SlowdownPercent, Damage, EffectedAoe);
        GameObject effectStart = SpawnEffect(EffectName, positonEffect, 1);
    }
}
