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
}
[System.Serializable]
public class BaseDatabase
{
    public int Tier;
    public float[] Value1;
    public float[] Value2;
    public int[] UpgradeLevelCost;
    public List<Item> EvolutionCost;
}

public class BaseDatabases
{
    public List<BaseDatabase> BaseHpData;
    public List<BaseDatabase> BaseManaData;
    public List<BaseDatabase> BaseShieldData;
    public BaseDatabase GetBaseHpData(int tier)
    {
        for (int i = 0; i < BaseHpData.Count; i++)
            if (BaseHpData[i].Tier == tier)
                return BaseHpData[i];
        return null;
    }
    public BaseDatabase GetBaseManaData(int tier)
    {
        for (int i = 0; i < BaseManaData.Count; i++)
            if (BaseManaData[i].Tier == tier)
                return BaseManaData[i];
        return null;
    }
    public BaseDatabase GetBaseShieldData(int tier)
    {
        for (int i = 0; i < BaseShieldData.Count; i++)
            if (BaseShieldData[i].Tier == tier)
                return BaseShieldData[i];
        return null;
    }
}