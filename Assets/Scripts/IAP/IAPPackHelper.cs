using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Packs
{
    public Pack[] packs;
    public float GetPackPrice(string packId)
    {
        foreach (var pack in packs)
            if (pack.id == packId)
                return pack.price;
        return 0;
    }
}
[System.Serializable]
public class Pack
{
    public string id;
    public float price;
}
public class IAPPackHelper
{
    public static float GetPackPrice(string packId)
    {
        string packsData = "";
        Packs packs = JsonUtility.FromJson<Packs>(packsData);
        return packs.GetPackPrice(packId);
    }
}

[System.Serializable]
public class PackDataBase
{
    public string id;
    public float price;
    public ListItem rewards;
}
[System.Serializable]
public class GameIAP
{
    public List<PackDataBase> IAPs;
}