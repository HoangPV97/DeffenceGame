using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BaseData
{
    public float HP;
    public float Mana;
    public float HPRegen;
    public float ManaRegen;
    public float ShieldBlockChance;
    public float ShieldBlockValue;
    public int Damage;
    public int Critical;
    public int KnockBackChance;
    public int AllianceDamage;
    public int QuickHand;
    public float QuickHandDamagePercent;
    public bool MultiShot;
    public float MultiShotDamage;
    public float MultiShotAddedAttributePercent;
    public float ReduceCooldown;
    public float IncreaseSpellDamage;
}
[System.Serializable]
public class BaseDatabase
{
    public int Tier;
    public List<SkillAttribute> Attributes;
    public int[] UpgradeLevelCost;
    public List<Item> CostEvolution;
    public int MaxLevel
    {
        get
        {
            return UpgradeLevelCost.Length;
        }
    }
    public float GetAttributeValue(string attribute, int Level)
    {
        if (Level == 0)
            Level = 1;
        for (int i = 0; i < Attributes.Count; i++)
        {
            if (Attributes[i].Attribute == attribute)
                return Attributes[i].Value[Level - 1];
        }
        return 0;
    }
}

public class BaseDatabases
{
    public List<BaseDatabase> BaseFortressData;
    public List<BaseDatabase> BaseTempleData;
    public List<BaseDatabase> BaseArcheryData;
    public int MaxTierArchery
    {
        get
        {
            return BaseArcheryData.Count;
        }
    }
    public int MaxTierTemple
    {
        get
        {
            return BaseTempleData.Count;
        }
    }
    public int MaxTierFortress
    {
        get
        {
            return BaseFortressData.Count;
        }
    }
    public BaseDatabase GetBaseFortressData(int tier)
    {
        for (int i = 0; i < BaseFortressData.Count; i++)
            if (BaseFortressData[i].Tier == tier)
                return BaseFortressData[i];
        return null;
    }
    public BaseDatabase GetBaseTempleData(int tier)
    {
        for (int i = 0; i < BaseTempleData.Count; i++)
            if (BaseTempleData[i].Tier == tier)
                return BaseTempleData[i];
        return null;
    }
    public BaseDatabase GetBaseArcheryData(int tier)
    {
        for (int i = 0; i < BaseArcheryData.Count; i++)
            if (BaseArcheryData[i].Tier == tier)
                return BaseArcheryData[i];
        return null;
    }
}