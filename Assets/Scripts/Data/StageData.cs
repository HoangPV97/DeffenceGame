using System.Collections;
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
    public int MaxStage;
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
    public List<ListItem> WinReward;
}
[System.Serializable]
public class ListItem
{
    public List<Item> items;
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
public class BossDataBase_Wind_1
{
    public List<WaveEnemyBoss_Wind_1> BossDataBase_Wind;
    public WaveEnemyBoss_Wind_1 GetWaveEnemyBoss_Wind_1(int _hardMode)
    {
        for (int i = 0; i < BossDataBase_Wind.Count; i++)
        {
            if (BossDataBase_Wind[i].HardMode == _hardMode)
            {
                return BossDataBase_Wind[i];
            }
        }
        return null;
    }
}

[System.Serializable]
public class WaveEnemyBoss_Wind_1 : WaveEnemyBoss
{
    public int DamagePlus;
    public int SpeedPlus;
    public int DelayAttack;
}
[System.Serializable]
public class WaveEnemyBoss
{
    public int HardMode;
    public StageEnemyDataBase stageEnemyDataBase;
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