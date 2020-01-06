﻿using System.Collections;
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
    public int KnockBack;
    public int AllianceDamage;
    public int QuickHand;
    public float QuickHandDamagePercent;
    public bool MultiShot;
    public float MultiShotDamage;
    public float MultiShotAddedAttributePercent;
    public float ReduceCooldown;
    public float IncreaseSpellDamage;
    public float BlockDamage;
}
[System.Serializable]
public class BaseDatabase
{
    public int Tier;
    public List<SkillAttribute> Attributes;
    public int[] UpgradeLevelCost;
    public List<Item> EvolutionCost;
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
    public List<BaseDatabase> BaseShieldData;
    public List<BaseDatabase> BaseArcheryData;
    public BaseDatabase GetBaseHpData(int tier)
    {
        for (int i = 0; i < BaseFortressData.Count; i++)
            if (BaseFortressData[i].Tier == tier)
                return BaseFortressData[i];
        return null;
    }
    public BaseDatabase GetBaseManaData(int tier)
    {
        for (int i = 0; i < BaseTempleData.Count; i++)
            if (BaseTempleData[i].Tier == tier)
                return BaseTempleData[i];
        return null;
    }
    public BaseDatabase GetBaseShieldData(int tier)
    {
        for (int i = 0; i < BaseShieldData.Count; i++)
            if (BaseShieldData[i].Tier == tier)
                return BaseShieldData[i];
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