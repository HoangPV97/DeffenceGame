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
}

[System.Serializable]
public class SkillWeaponWind1 : SkillData
{
    public struct SkillWeaponWind1Data
    {
        public int Tier;
        public float[] SkillSpeed;
        public float[] KnockbackDistance;
        public float[] KnockbackDuration;
        public float[] EffectedAoe;
    }
    public List<SkillWeaponWind1Data> SkillWeaponWind1Datas;
}
public class SkillWeaponFire1 : SkillData
{
    public float[] SkillSpeed;
    public float[] EffectedAoe;
    public float[] TimeEffect;
}
public class SkillWeaponIce1 : SkillData
{
    public float[] SkillSpeed;
    public float[] EffectedAoe;
    public float[] TimeEffect;
}