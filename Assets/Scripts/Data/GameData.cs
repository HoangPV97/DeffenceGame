using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class GameData
{
    public int BaseHPLevel = 1;
    public int BaseHPTier = 1;
    public int BaseManaLevel = 1;
    public int BaseManaTier = 1;
    public int BaseShieldLevel = 1;
    public int BaseShieldTier = 1;
    public int CurrentStage = 1;
    public List<Item> Inventory;
    public List<GameDataWeapon> gameDataWeapons;
    public List<GameDataWeapon> gameDataAlliance;
    public List<GameStage> gameStages;
    public Elemental CurrentSelectedWeapon;
    public Elemental Slot1, Slot2;
    public GameStage GetGameStage(int level)
    {
        for (int i = 0; i < gameStages.Count; i++)
            if (gameStages[i].Level == level)
                return gameStages[i];
        GameStage gameStage = new GameStage
        {
            Level = level,
            HardMode = 0
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
        for (int i = 0; i < Inventory.Count; i++)
            if (Inventory[i].Type == Type)
                return Inventory[i];
        return null;
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

    public GameDataWeapon GetGameAlliance(Elemental elemental)
    {
        for (int i = 0; i < gameDataAlliance.Count; i++)
        {
            if (gameDataAlliance[i].Type == elemental)
                return gameDataAlliance[i];
        }
        GameDataWeapon gdw = new GameDataWeapon
        {
            Type = elemental,
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
            }
            }
        };
        gameDataAlliance.Add(gdw);
        return gdw;
    }
}

/**/

[System.Serializable]
public class GameDataWeapon
{
    public Elemental Type;
    public SaveGameTierLevel WeaponTierLevel;
    public List<SaveGameTierLevel> SkillTierLevel;
}

public class SaveGameTierLevel
{
    public int Tier;
    public int Level;
}

public class GameStage
{
    public int Level;
    public int HardMode;
}