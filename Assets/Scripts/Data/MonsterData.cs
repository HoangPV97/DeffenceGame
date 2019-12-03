using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class MonsterData
{
    public string Type;
    public float HP;
    public float ATK;
    public float Range;
    public float ATKSpeed;
    public float MoveSpeed;
    public float Armor;
    public float Growth;
    public float BulletSpeed;
}
[System.Serializable]
public class MonsterDataBases
{
    public List<MonsterData> monsterDatas;
    public MonsterData GetMonsterData(string type)
    {
        for (int i = 0; i < monsterDatas.Count; i++)
        {
            if (monsterDatas[i].Type == type)
                return monsterDatas[i];
        }
        return null;
    }
}
[System.Serializable]
public class IngameAlliance
{
    public Elemental Type;
    public int Tier;
    public int Level;
    public float ATK;
    public float ATKspeed;
    public float ATKRange;
}
[System.Serializable]
public class AllianceData
{
    public Elemental Type;
    public int Tier;
    public List<float> ATK;
    public List<float> ATKspeed;
    public List<float> ATKRange;
    public List<Item> CostEvolution;
    public float BulletSpeed;
}

public class AllianceDataBases
{
    public List<AllianceData> allianceDatas;
    public List<Item> GetCostEvolution(Elemental Type, int Tier)
    {
        return GetAlliance(Type, Tier).CostEvolution;
    }

    public AllianceData GetAlliance(Elemental Type, int Tier)
    {
        for (int i = 0; i < allianceDatas.Count; i++)
            if (allianceDatas[i].Type == Type && allianceDatas[i].Tier == Tier)
                return allianceDatas[i];
        return null;
    }
}
