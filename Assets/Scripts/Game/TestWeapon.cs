using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class TestWeapon
{
    public List<Weapons> Weapons;
}
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
    WindObs = 1,
    IceObs,
    FireObs,
    EarthObs,
    Mithril,
    Gold
}


