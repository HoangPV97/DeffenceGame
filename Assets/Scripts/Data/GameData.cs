using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class GameData
{
    public SaveGameTierLevel Archery;
    public List<SaveGameTierLevel> ArcherySkillTierLevel;
    public SaveGameTierLevel Temple;
    public List<SaveGameTierLevel> TempleSkillTierLevel;
    public SaveGameTierLevel Fortress;
    public List<SaveGameTierLevel> FortressSkillTierLevel;
    public int CurrentStage = 1;
    public int Gold;
    public int Gem;
    public List<Item> Inventory;
    public List<GameDataWeapon> gameDataWeapons;
    public List<GameDataWeapon> gameDataAlliance;
    public List<GameStage> gameStages;
    public Elemental CurrentSelectedWeapon;
    public Elemental Slot1, Slot2;
    public string UNLOCK_UI = "NONE";
    [SerializeField]
    public DateTime timeStamp;
    public DateTime LastPlayGacha;
    public List<GameDataQuest> gameDataQuests;
    public List<int> gameDataQuestLevels;
    public int Energy;
    public DateTime EnergyTimeStamp;
    public List<GameDataAchievement> gameDataAchievements;
    public int DailyLogin, DailyLoginDone;
    public GameDataAchievement GetGameDataAchievement(ACHIEVEMENT_TYPE _TYPE)
    {
        if (gameDataAchievements == null)
            gameDataAchievements = new List<GameDataAchievement>();
        for (int i = 0; i < gameDataAchievements.Count; i++)
        {
            if (gameDataAchievements[i]._TYPE == _TYPE)
                return gameDataAchievements[i];
        }
        var gda = new GameDataAchievement(_TYPE);
        gameDataAchievements.Add(gda);
        return gda;
    }
    public GameStage GetGameStage(int level)
    {
        if (gameStages == null)
            gameStages = new List<GameStage>();
        for (int i = 0; i < gameStages.Count; i++)
            if (gameStages[i].Level == level)
            {
                return gameStages[i];
            }

        GameStage gameStage = new GameStage
        {
            Level = level,
            HardMode = level == 1 ? 1 : 0
        };
        gameStages.Add(gameStage);
        return gameStage;
    }
    public void SaveGameStage(int level, int hardMode = 0)
    {
        var gs = GetGameStage(level);
        if (gs != null)
            gs.HardMode = hardMode;
        else
        {
            GameStage gameStage = new GameStage
            {
                Level = level,
                HardMode = hardMode
            };
            gameStages.Add(gameStage);
        }
    }
    public Item GetItem(ITEM_TYPE Type)
    {
        if (Inventory == null)
            Inventory = new List<Item>();
        for (int i = 0; i < Inventory.Count; i++)
            if (Inventory[i].Type == Type)
                return Inventory[i];
        Item item = new Item { Quality = 0, Type = Type };
        Inventory.Add(item);
        return item;
    }
    public void AddItemQuality(ITEM_TYPE type, int number)
    {
        var it = GetItem(type);
        it.Quality += number;
        if (it.Quality < 0)
            it.Quality = 0;
    }
    public void SaveItem(ITEM_TYPE type, int number)
    {
        var it = GetItem(type);
        if (it != null)
            it.Quality = number;
        else
        {
            Item item = new Item
            {
                Type = type,
                Quality = number
            };
            Inventory.Add(item);
        }
    }
    public GameDataWeapon GetGameDataWeapon(Elemental elemental)
    {
        for (int i = 0; i < gameDataWeapons.Count; i++)
        {
            if (gameDataWeapons[i].Type == elemental)
                return gameDataWeapons[i];
        }
        GameDataWeapon gdw = new GameDataWeapon
        {
            Type = elemental,
            ID = elemental.ToString() + "1",
            WeaponTierLevel = new SaveGameTierLevel
            {
                Tier = 1,
                Level = 1,
            },
            SkillTierLevel = new List<SaveGameTierLevel>() {
                new SaveGameTierLevel
            {
                Tier = 1,
                Level = 1,
            },new SaveGameTierLevel
            {
                Tier = 2,
                Level = 0,
            },new SaveGameTierLevel
            {
                Tier = 3,
                Level = 0,
            },new SaveGameTierLevel
            {
                Tier = 999,
                Level = 1,
            }
            }
        };
        gameDataWeapons.Add(gdw);
        return gdw;
    }
    public GameDataWeapon GetGameDataAlliance(Elemental elemental)
    {
        for (int i = 0; i < gameDataAlliance.Count; i++)
        {
            if (gameDataAlliance[i].Type == elemental)
                return gameDataAlliance[i];
        }
        GameDataWeapon gdw = new GameDataWeapon
        {
            Type = elemental,
            ID = elemental.ToString() + "_Alliance1",
            WeaponTierLevel = new SaveGameTierLevel
            {
                Tier = 1,
                Level = 0,
            },
            SkillTierLevel = new List<SaveGameTierLevel>() {
                new SaveGameTierLevel
            {
                Tier = 1,
                Level = 1,
            },new SaveGameTierLevel
            {
                Tier = 2,
                Level = 0,
            },new SaveGameTierLevel
            {
                Tier = 3,
                Level = 0,
            },new SaveGameTierLevel
            {
                Tier = 999,
                Level = 1,
            }
            }
        };
        gameDataAlliance.Add(gdw);
        return gdw;
    }
    public void ResetDailyQuest()
    {
        Debug.Log("ResetDailyQuest");
        gameDataQuests = new List<GameDataQuest>();
        List<QUEST_TYPE> ListQuest = new List<QUEST_TYPE>() { QUEST_TYPE.QUEST_1,QUEST_TYPE.QUEST_2,QUEST_TYPE.QUEST_3,
                        QUEST_TYPE.QUEST_4,QUEST_TYPE.QUEST_5,QUEST_TYPE.QUEST_6,QUEST_TYPE.QUEST_7,QUEST_TYPE.QUEST_8};
        var randoms = InviGiant.Tools.IGMaths.GetRandomNoDuplicate(ListQuest, 3);
        for (int i = 0; i < randoms.Count; i++)
        {
            int Level = GetQuestLevel(randoms[i]);
            var data = DataController.Instance.GetDailyQuestDatabase(randoms[i]);
            if (Level > data.MAX_LEVEL)
                Level = data.MAX_LEVEL;
            GameDataQuest gameDataQuest = new GameDataQuest()
            {
                QUEST_TYPE = randoms[i],
                Target = data.Target[Level],
                Current = 0,
                Rewards = data.GetRandomReward(Level),
                Status = 0
            };
            gameDataQuests.Add(gameDataQuest);
        }
    }
    public int GetQuestLevel(QUEST_TYPE _TYPE)
    {
        if (gameDataQuestLevels == null || gameDataQuestLevels.Count == 0)
            gameDataQuestLevels = new List<int> { 0, 0, 0, 0, 0, 0, 0, 0 };
        return gameDataQuestLevels[(int)_TYPE];

    }
}

/**/
public enum Elemental { None = 0, Wind = 1, Ice = 2, Earth = 3, Fire = 4 }
[System.Serializable]
public class GameDataWeapon
{
    public Elemental Type;
    public string ID;
    public int EXP;
    public SaveGameTierLevel WeaponTierLevel;
    public List<SaveGameTierLevel> SkillTierLevel;
    public SaveGameTierLevel GetSkillTierLevel(string SkillID)
    {
        if (SkillTierLevel == null)
            SkillTierLevel = new List<SaveGameTierLevel>();
        for (int i = 0; i < SkillTierLevel.Count; i++)
            if (SkillTierLevel[i].Des == SkillID)
                return SkillTierLevel[i];

        var ktl = new SaveGameTierLevel()
        {
            Tier = 1,
            Level = 0,
            Des = SkillID
        };
        SkillTierLevel.Add(ktl);
        return ktl;
    }
}
[System.Serializable]
public class SaveGameTierLevel
{
    public int Tier;
    public int Level;
    public string Des;
}
[System.Serializable]
public class GameStage
{
    public int Level;
    public int HardMode;
}

[System.Serializable]
public class GameDataQuest
{
    public QUEST_TYPE QUEST_TYPE;
    public int Target;
    public int Current;
    public List<Item> Rewards;
    /// <summary>
    /// 0:undone 1:done 2:Finish
    /// </summary>
    public int Status;

    public void AddCurrent(int value)
    {
        if (Status == 0)
        {
            Current += value;
            if (Current >= Target)
            {
                Current = Target;
                Status = 1;
            }
        }
    }

    public void ClaimReward()
    {
        Status = 2;
        for (int i = 0; i < Rewards.Count; i++)
        {
            if (Rewards[i].Type == ITEM_TYPE.coin)
            {
                DataController.Instance.Gold += Rewards[i].Quality;
            }
            if (Rewards[i].Type == ITEM_TYPE.gem)
            {
                DataController.Instance.Gem += Rewards[i].Quality;
            }
            else
                DataController.Instance.AddItemQuality(Rewards[i].Type, Rewards[i].Quality);
        }
        DataController.Instance.Save();
    }
}

[System.Serializable]
public class GameDataAchievement
{
    public ACHIEVEMENT_TYPE _TYPE;
    public int Level;
    public int Target;
    public int Current;
    public GameDataAchievement(ACHIEVEMENT_TYPE _TYPE)
    {
        this._TYPE = _TYPE;
        Level = 0;
        Current = 0;
        var ad = DataController.Instance.GetAchievementDatabase(_TYPE);
        Target = ad.GetTarget(Level + 1);
    }
    public void AddCurrent(int value)
    {
        Current += value;
        if (Current >= Target)
        {
            Level++;
            var ad = DataController.Instance.GetAchievementDatabase(_TYPE);
            if (Level >= ad.MAX_LEVEL)
            {
                Level = ad.MAX_LEVEL;
                Current = Target;
            }
            else
            {
                Target = ad.GetTarget(Level + 1);
            }
        }

    }
}