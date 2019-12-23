using InviGiant.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnEnemy : Singleton<SpawnEnemy>
{
    public void SpawnEnemyBoss_Wind_1()
    {
        int HardMode = DataController.Instance.StageData.HardMode;
        var sd = DataController.Instance.BossDataBase_Wind.GetWaveEnemyBoss_Wind_1(HardMode);
        for (int i = 0; i < sd.stageEnemyDataBase.stageEnemies.Count; i++)
        {
            StartCoroutine(GameplayController.Instance.IESpawnEnemyBoss(sd.stageEnemyDataBase,
                                                                        i, sd.stageEnemyDataBase.stageEnemies[i].StartTime));
            GameController.Instance.EnemyLive += sd.stageEnemyDataBase.stageEnemies[i].Number;
        }
    }

}
