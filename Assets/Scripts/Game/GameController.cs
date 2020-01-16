using System.Collections;
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
        gameStage.HardMode++;

        if (Level % 10 == 0)
            DataController.Instance.CheckDailyQuest(QUEST_TYPE.QUEST_5, 1);

        var gameStage2 = DataController.Instance.GetGameStage(Level + 1);
        if (gameStage2.HardMode == 0)
            gameStage2.HardMode = 1;

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
        if (DataController.Instance.CurrentSelected < DataController.Instance.MaxStage)
            DataController.Instance.CurrentSelected++;
        DataController.Instance.Save();
        //  WingamePanel.SetActive(true);
        float healthPercent = ((float)GameplayController.Instance.Tower.Health.CurrentHealth / (float)GameplayController.Instance.Tower.Health.health) * 100;

        UIPanelResult.SetUpdataVictory((int)(EnemyNumber - EnemyLive), (int)healthPercent, (int)GameplayController.Instance.TotalGoldDrop);

        GameplayController.Instance.Tower.StopRecoverHealth();
        GameplayController.Instance.Tower.StopRecoverMana();
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
        if (
        GameplayController.Instance.TotalGoldDrop > DataController.Instance.GoldInGame)
        {
            GameplayController.Instance.TotalGoldDrop = Mathf.RoundToInt(DataController.Instance.GoldInGame);
        }
        GameplayController.Instance.TotalGoldDrop += GameplayController.Instance.GoldEachEnemy;
        EnemyLive -= value;
        if (EnemyLive == 0)
            WinGame();
        DataController.Instance.CheckDailyQuest(QUEST_TYPE.QUEST_1, 1);
    }
}
