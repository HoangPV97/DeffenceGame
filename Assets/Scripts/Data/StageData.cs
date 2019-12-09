﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class StageData
{
    public int Level;
    public int HardMode;
    public StageDataBase stageDataBase;
    public StageEnemyDataBase stageEnemyDataBase;
}
[System.Serializable]
public class GameStageDataBase
{
    public List<StageDataBase> stageDataBases;
    public StageDataBase GetStageDataBase(int Level)
    {
        for (int i = 0; i < stageDataBases.Count; i++)
        {
            if (stageDataBases[i].Level == Level)
            {
                return stageDataBases[i];
            }
        }
        return null;
    }
}

[System.Serializable]
public class StageDataBase
{
    public int Level;
    public int[] StarCondition;
    public List<Item> WinReward;
}
[System.Serializable]
public class GameEnemyDataBase
{
    public List<StageEnemyDataBase> stageEnemies;
    public StageEnemyDataBase GetStageEnemyDataBase(int Level)
    {
        for (int i = 0; i < stageEnemies.Count; i++)
        {
            if (stageEnemies[i].Level == Level)
            {
                return stageEnemies[i];
            }
        }
        return null;
    }
}
[System.Serializable]
public class StageEnemyDataBase
{
    public int Level;
    public int NightMareAddLevel;
    public int HellAddLevel;
    public List<StageEnemy> stageEnemies;
}
[System.Serializable]
public class StageEnemy
{
    public string Type;
    public int Level;
    public float StartTime;
    public float RepeatTime;
    public int Position;
    public int Number;
}