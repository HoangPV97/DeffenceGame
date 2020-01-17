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
    public float Damage;
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
    public int achi_AddedGoldKilled;
    public float achi_AddedDmgWeaponAlliance;
    public float achi_AddedDmgSpellWind;
    public float achi_AddedDmgSpellEarth;
    public float achi_AddedDmgSpellIce;
    public float achi_AddedDmgSpellFire;
    public float achi_AddedDmgAllianceWind;
    public float achi_AddedDmgAllianceEarth;
    public float achi_AddedDmgAllianceIce;
    public float achi_AddedDmgAllianceFire;
    public float achi_AddedDmgWeapon;
    public float achi_AddedDmgAlliance;
    public BaseData()
    {
        achi_AddedGoldKilled = 0;
        achi_AddedDmgWeaponAlliance = 1;
        achi_AddedDmgSpellWind = 1;
        achi_AddedDmgSpellEarth = 1;
        achi_AddedDmgSpellIce = 1;
        achi_AddedDmgSpellFire = 1;
        achi_AddedDmgAllianceWind = 1;
        achi_AddedDmgAllianceEarth = 1;
        achi_AddedDmgAllianceIce = 1;
        achi_AddedDmgAllianceFire = 1;
        achi_AddedDmgWeapon = 1;
        achi_AddedDmgAlliance = 1;
    }
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