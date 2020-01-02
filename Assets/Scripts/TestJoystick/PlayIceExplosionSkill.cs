using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayIceExplosionSkill : DragAndDropSkill
{
    [SerializeField]
    SkillWeaponIce1 Swi1;
    protected float TimeEffect, SlowdownPercent, EffectedAoe, Damage;
    public override void SetUpData(int Tier = 1, int Level = 1, VariableJoystick variableJoystick = null, Vector3 _position = default)
    {
        base.SetUpData(Tier, Level);
        Swi1 = JsonUtility.FromJson<SkillWeaponIce1>(ConectingFireBase.Instance.GetTextSkill(SkillID));
        this.variableJoystick = variableJoystick;
        manaCost = Swi1.GetManaCost(Tier, Level);
        TimeEffect = Swi1.GetSkillAttributes("TimeEffect", Tier, Level);
        EffectedAoe = Swi1.GetSkillAttributes("EffectedAoe", Tier, Level);
        SlowdownPercent = Swi1.GetSkillAttributes("SlowdownPercent", Tier, Level);
        Damage = Swi1.GetDamage(Tier, Level);
        CountdownTime = Swi1.GetCoolDown(Tier, Level);
        variableJoystick.SetUpData(this);
        positonEffect = _position;
        CountdownGo = variableJoystick.CountDountMask;
    }

    public override void PlaySkill(Vector3 _position)
    {
        GameObject Poison_Skill = SpawnEffect(SkillID, _position, TimeEffect);
        Poison_Skill.GetComponent<SlowSkill>().SetSkillData(TimeEffect, SlowdownPercent, Damage, EffectedAoe);
        SoundManager.Instance.PlayClipOneShot(SoundManager.Instance.Explosion);
        GameObject effectStart = SpawnEffect(EffectName, positonEffect, 1);
    }
}
