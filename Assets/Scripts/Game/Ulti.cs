using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeaponDatabase
{
    public List<Weapons> Weapons;
    public List<Item> GetCostEvolution(Elemental Type, int Tier)
    {
        return GetWeapons(Type, Tier).CostEvolution;
    }

    public Weapons GetWeapons(Elemental Type, int Tier)
    {
        for (int i = 0; i < Weapons.Count; i++)
            if (Weapons[i].Type == Type && Weapons[i].Tier == Tier)
                return Weapons[i];

        return null;
    }
}
[System.Serializable]
public class InGameWeapon
{
    public Elemental Type;
    public int Tier;
    public int Level;
    public float ATK;
    public float ATKplus;
    public float ATKspeed;
    public float BulletSpeed;
    public float CritChance;
    public float CritValue;
}

[System.Serializable]
public class Weapons
{
    public Elemental Type;
    public int Tier;
    public float BulletSpeed;
    public float ATKplus;
    public List<float> ATK;
    public List<float> ATKspeed;
    public List<float> Cost;
    public List<Item> CostEvolution;
    public int MaxLevel
    {
        get
        {
            return ATK.Count;
        }
    }
    public float MaxEXP
    {
        get
        {
            return Cost[Cost.Count - 1];
        }
    }

    public float GetATK(int Level)
    {
        if (Level < ATK.Count)
            return ATK[Level - 1];
        return ATK[ATK.Count - 1];
    }
    public float GetATKspeed(int Level)
    {
        if (Level < ATKspeed.Count)
            return ATKspeed[Level - 1];
        return ATKspeed[ATKspeed.Count - 1];
    }
    public float GetCost(int Level)
    {
        return Cost[Level];
    }
}
[System.Serializable]
public class Item
{
    public ITEM_TYPE Type;
    public int Quality;
}
[System.Serializable]
public enum ITEM_TYPE
{
    None = 0,
    WindObs_1,
    WindObs_2,
    WindObs_3,
    IceObs_1,
    IceObs_2,
    IceObs_3,
    FireObs_1,
    FireObs_2,
    FireObs_3,
    EarthObs_1,
    EarthObs_2,
    EarthObs_3,
    coin,
    gem,
    Evolve_Weapon,
    Evolve_Spell,
    Evolve_Wind,
    Evolve_Ice,
    Evolve_Earth,
    Evolve_Fire,
    Upgrade_Spell_Wind_1,
    Upgrade_Spell_Ice_1,
    Upgrade_Spell_Earth_1,
    Upgrade_Spell_Fire_1,
    Upgrade_Spell_Wind_2,
    Upgrade_Spell_Ice_2,
    Upgrade_Spell_Earth_2,
    Upgrade_Spell_Fire_2,
}
[System.Serializable]
public class ItemDataBase
{
    public List<ItemData> Items;
    public ItemData GetItemData(ITEM_TYPE Type)
    {
        for (int i = 0; i < Items.Count; i++)
            if (Items[i].Type == Type)
                return Items[i];
        return null;
    }
}
[System.Serializable]
public class ItemData
{
    public ITEM_TYPE Type;
    public string ID;
    public int Value;
    public int UseGemCost;
    public int UseGoldCost;
    public int GemCost;
    public int GoldCost;
}

