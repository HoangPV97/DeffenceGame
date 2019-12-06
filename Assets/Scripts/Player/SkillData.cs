using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class SkillData
{
    /// <summary>
    /// SkillID = WEAPON_ICE_SKILL_1 || SkillID = ALLIANCE_ICE_SKILL_1
    /// </summary>
    public string SkillID;
    public float[] ManaCost;
    public float[] CoolDown;
}

[System.Serializable]
public class SkillWeaponWind1 : SkillData
{
    public float[] SkillSpeed;
    public float[] Damage;
    public float[] KnockbackDistance;
    public float[] KnockbackDuration;
    public float[] EffectedAoe;
}
public class SkillWeaponFire1 : SkillData
{
    public float[] SkillSpeed;
    public float[] Damage;
    public float[] EffectedAoe;
    public float[] TimeEffect;
}
public class SkillWeaponIce1 : SkillData
{
    public float[] SkillSpeed;
    public float[] Damage;
    public float[] EffectedAoe;
    public float[] TimeEffect;
}