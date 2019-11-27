using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class TestWeapon
{
    public List<Weapons> Weapons;
}
[System.Serializable]
public class Weapons
{
    public Elemental Type;
    public int Tier;
    public List<int> ATK;
    public List<int> ATKspeed;
    public List<int> Cost;
    public List<ItemEvolution> CostEvolution;
}
[System.Serializable]
public class ItemEvolution
{
    public  itemEvolutionType Type;
    public int Quality;
}
[System.Serializable]
public enum itemEvolutionType{
    WindObs=1,
    IceObs,
    FireObs,
    EarthObs,
    Mithril,
    Gold
}


