using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class GameData
{
    public int BaseLevel;
    public int CurrentStage;
    public List<Item> Inventory;
    public List<GameDataWeapon> gameDataWeapons;
    public List<Elemental> EquipedWeapon;
    public List<GameStage> gameStages;

    public GameStage GetGameStage(int Level)
    {
        for (int i = 0; i < gameStages.Count; i++)
            if (gameStages[i].Level == Level)
                return gameStages[i];
        return null;
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
}

[System.Serializable]
public class BaseData
{
    public float HP;
    public float Mana;
    public float HPRegen;
    public float ManaRegen;
    public float Shield;
}

[System.Serializable]
public class GameDataWeapon
{
    public Elemental Type;
    public int Tier;
    public int Level;
}

public class GameStage
{
    public int Level;
    public int HardMode;
}