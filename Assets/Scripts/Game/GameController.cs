﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController Instance;
    public UIPanelResult UIPanelResult;
    public float EnemyLive;
    public GameObject GameoverPanel, WingamePanel, PausePanel;
    public Text GoldText, WinGold;
    float Gold = 0;
    float count = 0;
    //float maxGold = 0;
    public float EnemyNumber;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        Time.timeScale = 1;
    }
    // Start is called before the first frame update
    void Start()
    {
        EnemyNumber = EnemyLive;
        //maxGold = DataController.Instance.GoldInGame + DataController.Instance.InGameBaseData.achi_AddedGoldKilled * EnemyLive;
    }

    public void SaveGame()
    {
        DataController.Instance.Save();
    }
    public void EndGame()
    {
        // GameoverPanel.SetActive(true);
        Time.timeScale = 0;
        UIPanelResult.SetUpDataFailed((int)(EnemyNumber - EnemyLive), 0, (int)GameplayController.Instance.TotalGoldDrop);
        GameplayController.Instance.Tower.StopRecoverHealth();
        GameplayController.Instance.Tower.StopRecoverMana();
        DataController.Instance.Save();
        DataController.Instance.ShowGachaFail = true;
    }
    public void Restart()
    {
        SceneManager.LoadScene(1);
        Time.timeScale = 1;
    }
    public void SetGoldText(float _gold)
    {
        GoldText.text = "" + _gold;
        WinGold.text = "Gold : " + _gold;
    }
    public void GoldUp(float _bonusgold)
    {
        Gold += _bonusgold;
        SetGoldText(Gold);
    }
    public void WinGame()
    {
        DataController.Instance.WinGachaCount++;
        // Add Item
        int Level = DataController.Instance.CurrentSelected;
        var gameStage = DataController.Instance.GetGameStage(Level);
        var gsd = DataController.Instance.GetStageDataBase(Level);
        var listItem = gsd.WinReward[gameStage.HardMode - 1];
        for (int i = 0; i < listItem.items.Count; i++)
        {
            /* if (listItem.items[i].Type == ITEM_TYPE.coin)
             {
                 DataController.Instance.Gold += listItem.items[i].Quality;
             }
             else*/
            DataController.Instance.AddItemQuality(listItem.items[i].Type, listItem.items[i].Quality);
        }
        DataController.Instance.Gold += Mathf.RoundToInt(GameplayController.Instance.TotalGoldDrop);
        if (gameStage.HardMode == 3)
            DataController.Instance.CheckDailyQuest(QUEST_TYPE.QUEST_7, 1);

        if (Level % 10 == 0)
            DataController.Instance.CheckDailyQuest(QUEST_TYPE.QUEST_5, 1);

        var gameStage2 = DataController.Instance.GetGameStage(Level + 1);
        if (gameStage2.HardMode == 0)
            gameStage2.HardMode = 1;

        if (gameStage.HardMode == 1)
        {
            if (Level == 5)
            {
                DataController.Instance.UnLockAlliance(Elemental.Wind);//Wind            
            }
            if (Level == 15)
            {
                DataController.Instance.UnLockAlliance(Elemental.Earth);
            }
            if (Level == 25)
            {
                DataController.Instance.UnLockAlliance(Elemental.Ice);
            }
            if (Level == 35)
            {
                DataController.Instance.UnLockAlliance(Elemental.Fire);
            }
            if (Level == 10)
            {
                DataController.Instance.UnLockWeapon(Elemental.Earth);
            }
            if (Level == 20)
            {
                DataController.Instance.UnLockWeapon(Elemental.Ice);
            }
            if (Level == 30)
            {
                DataController.Instance.UnLockWeapon(Elemental.Fire);
            }
        }
        //  WingamePanel.SetActive(true);
        float healthPercent = ((float)GameplayController.Instance.Tower.Health.CurrentHealth / (float)GameplayController.Instance.Tower.Health.health) * 100;

        UIPanelResult.SetUpdataVictory((int)(EnemyNumber - EnemyLive), (int)healthPercent, Mathf.RoundToInt(GameplayController.Instance.TotalGoldDrop));
        for (int i = 0; i < GameplayController.Instance.AllianceController.Count; i++)
        {
            switch (GameplayController.Instance.AllianceController[i].elementalType)
            {
                case Elemental.None:
                    break;
                case Elemental.Wind:
                    DataController.Instance.CheckAchievement(ACHIEVEMENT_TYPE.ACHIEVEMENT_7, 1);
                    break;
                case Elemental.Ice:
                    DataController.Instance.CheckAchievement(ACHIEVEMENT_TYPE.ACHIEVEMENT_9, 1);
                    break;
                case Elemental.Earth:
                    DataController.Instance.CheckAchievement(ACHIEVEMENT_TYPE.ACHIEVEMENT_8, 1);
                    break;
                case Elemental.Fire:
                    DataController.Instance.CheckAchievement(ACHIEVEMENT_TYPE.ACHIEVEMENT_10, 1);
                    break;
            }
        }
        GameplayController.Instance.Tower.StopRecoverHealth();
        GameplayController.Instance.Tower.StopRecoverMana();
        if (DataController.Instance.CurrentSelected < DataController.Instance.MaxStage)
            DataController.Instance.CurrentSelected++;
        gameStage.HardMode++;
        DataController.Instance.Energy++;
    }
    public void PauseGame()
    {
        Time.timeScale = 0;
        PausePanel.SetActive(true);
    }
    public void Continue()
    {
        Time.timeScale = 1;
        PausePanel.SetActive(false);
    }

    public void OnEnemyDie(int value)
    {
        //if (
        //GameplayController.Instance.TotalGoldDrop > maxGold)
        //{
        //    GameplayController.Instance.TotalGoldDrop = maxGold;
        //}

        GameplayController.Instance.TotalGoldDrop += GameplayController.Instance.GoldEachEnemy + DataController.Instance.InGameBaseData.achi_AddedGoldKilled;
        EnemyLive -= value;
        if (EnemyLive == 0)
            WinGame();
        DataController.Instance.CheckDailyQuest(QUEST_TYPE.QUEST_1, 1);
        DataController.Instance.CheckAchievement(ACHIEVEMENT_TYPE.ACHIEVEMENT_2, 1);
    }
}
