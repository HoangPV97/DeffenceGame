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
    /// <summary>
    /// 0: active
    /// 1: Passive
    /// </summary>
    public int SkillType;
    public List<BaseSkill> baseSkills;
    public float GetManaCost(int Tier, int Level)
    {
        if (Level == 0)
            Level = 1;
        for (int i = 0; i < baseSkills.Count; i++)
        {
            if (baseSkills[i].Tier == Tier)
            {
                if (Level <= baseSkills[i].ManaCost.Length)
                    return baseSkills[i].ManaCost[Level - 1];
                else
                    return baseSkills[i].ManaCost[baseSkills[i].ManaCost.Length - 1];
            }
        }
        return 0;
    }
    public float GetSkillSpeed(int Tier, int Level)
    {
        if (Level == 0)
            Level = 1;
        for (int i = 0; i < baseSkills.Count; i++)
        {
            if (baseSkills[i].Tier == Tier)
            {
                if (Level <= baseSkills[i].SkillSpeed.Length)
                    return baseSkills[i].SkillSpeed[Level - 1];
                else
                    return baseSkills[i].SkillSpeed[baseSkills[i].SkillSpeed.Length - 1];
            }
        }
        return 0;
    }

    public float GetCoolDown(int Tier, int Level)
    {
        if (Level == 0)
            Level = 1;
        for (int i = 0; i < baseSkills.Count; i++)
        {
            if (baseSkills[i].Tier == Tier)
            {
                if (Level <= baseSkills[i].CoolDown.Length)
                    return baseSkills[i].CoolDown[Level - 1];
                else
                    return baseSkills[i].CoolDown[baseSkills[i].CoolDown.Length - 1];
            }
        }
        return 0;
    }

    public float GetDamage(int Tier, int Level)
    {
        if (Level == 0)
            Level = 1;
        for (int i = 0; i < baseSkills.Count; i++)
        {
            if (Level <= baseSkills[i].Damage.Length)
                return baseSkills[i].Damage[Level - 1];
            else
                return baseSkills[i].Damage[baseSkills[i].Damage.Length - 1];
        }
        return 0;
    }

    public List<Item> GetUpgradeItems(int Tier, int Level)
    {
        if (Level == 0)
            Level = 1;
        for (int i = 0; i < baseSkills.Count; i++)
        {
            if (baseSkills[i].Tier == Tier)
                return baseSkills[i].UpgradeItems[Level - 1].items;
        }
        return null;
    }
    public float GetSkillAttributes(string attribute, int Tier, int Level)
    {
        if (Level == 0)
            Level = 1;
        for (int i = 0; i < baseSkills.Count; i++)
        {
            if (baseSkills[i].Tier == Tier)
                return GetAttribute(baseSkills[i].SkillAttributes, attribute, Level - 1);
        }
        return 0;
    }
    protected float GetAttribute(List<SkillAttribute> attributeList, string _attribute, int level)
    {
        for (int i = 0; i < attributeList.Count; i++)
        {
            if (attributeList[i].Attribute.Equals(_attribute))
            {
                return attributeList[i].Value[level];
            }
        }
        return 0;
    }
}
[System.Serializable]
public class BaseSkill
{
    public int Tier;
    public float[] ManaCost;
    public float[] CoolDown;
    public float[] Damage;
    public float[] SkillSpeed;
    public List<ListItem> UpgradeItems;
    public List<Item> EvolutionItems;
    public List<SkillAttribute> SkillAttributes;
    public int MaxLevel
    {
        get
        {
            if (SkillAttributes != null && SkillAttributes.Count > 0)
                return SkillAttributes[0].Value.Length;
            return UpgradeItems.Count;
        }
    }
}


[System.Serializable]
public class SkillWeaponWind1 : SkillData
{
    public const string KnockbackDistance = "KnockbackDistance";
    public const string KnockbackDuration = "KnockbackDuration";
    public const string EffectedAoe = "EffectedAoe";
    public List<SkillAttribute> SpecialSkillAttributes;
}
[System.Serializable]
public class SkillWeaponWind2 : SkillData
{
    public const string TimeEffect = "TimeEffect";
    public const string EffectedAoe = "EffectedAoe";
    public List<SkillAttribute> SpecialSkillAttributes;
}
public class SkillWeaponFire1 : SkillData
{
    public const string TimeEffect = "TimeEffect";
    public const string EffectedAoe = "EffectedAoe";
}
public class SkillWeaponIce1 : SkillData
{
    public const string TimeEffect = "TimeEffect";
    public const string EffectedAoe = "EffectedAoe";
}
[System.Serializable]
public class SkillAttribute
{
    public string Attribute;
    public float[] Value;
}