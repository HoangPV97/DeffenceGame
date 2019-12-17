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
    public List<BaseSkill> baseSkills;
    public float GetManaCost(int Tier, int Level)
    {
        for (int i = 0; i < baseSkills.Count; i++)
        {
            if (baseSkills[i].Tier == Tier)
                return baseSkills[i].ManaCost[Level - 1];
        }
        return 0;
    }

    public float GetCoolDown(int Tier, int Level)
    {
        for (int i = 0; i < baseSkills.Count; i++)
        {
            if (baseSkills[i].Tier == Tier)
                return baseSkills[i].CoolDown[Level - 1];
        }
        return 0;
    }

    public float GetDamage(int Tier, int Level)
    {
        for (int i = 0; i < baseSkills.Count; i++)
        {
            if (baseSkills[i].Tier == Tier)
                return baseSkills[i].Damage[Level - 1];
        }
        return 0;
    }

    public List<Item> GetUpgradeItems(int Tier, int Level)
    {
        for (int i = 0; i < baseSkills.Count; i++)
        {
            if (baseSkills[i].Tier == Tier)
                return baseSkills[i].UpgradeItems[Level - 1];
        }
        return null;
    }

}
[System.Serializable]
public class BaseSkill
{
    public int Tier;
    public float[] ManaCost;
    public float[] CoolDown;
    public float[] Damage;
    public List<Item>[] UpgradeItems;
    public List<Item> EvolutionItems;
    public List<SkillAttribute> SkillAttributes;

}

[System.Serializable]
public class SkillWeaponWind1 : SkillData
{
    public const string SkillSpeed = "SkillSpeed";
    public const string KnockbackDistance = "KnockbackDistance";
    public const string KnockbackDuration = "KnockbackDuration";
    public const string EffectedAoe = "EffectedAoe";
}
public class SkillWeaponFire1 : SkillData
{
    public const string SkillSpeed = "SkillSpeed";
    public const string TimeEffect = "TimeEffect";
    public const string EffectedAoe = "EffectedAoe";
}
public class SkillWeaponIce1 : SkillData
{
    public const string SkillSpeed = "SkillSpeed";
    public const string TimeEffect = "TimeEffect";
    public const string EffectedAoe = "EffectedAoe";
}

public class SkillAttribute
{
    public string Attribute;
    public float[] Value;
}