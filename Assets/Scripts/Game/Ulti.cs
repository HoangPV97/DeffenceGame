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
    public List<float> ATK;
    public List<float> ATKspeed;
    public List<float> Cost;
    public List<Item> CostEvolution;
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
    Mithril,
    coin,
    Gem
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

