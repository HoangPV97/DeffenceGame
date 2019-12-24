using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InviGiant.Tools;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;
public class DataController : Singleton<DataController>
{
    #region DataBase
    public WeaponDatabase WeaponsDatas;
    public MonsterDataBases MonsterDataBases;
    public GameStageDataBase GameStageDataBase;
    public GameEnemyDataBase GameEnemyDataBase;
    public AllianceDataBase AllianceDataBases;
    public BaseDatabases BaseDatabases;
    public DefaultData DefaultData;
    public ItemDataBase ItemDataBase;
    public BossDataBase_Wind_1 BossDataBase_Wind;
    #endregion

    #region Data Player
    private GameData GameData;
    public bool CanEquipAlliance
    {
        get
        {
            return ElementalSlot1 == Elemental.None || ElementalSlot2 == Elemental.None;
        }
    }
    public Elemental ElementalSlot1
    {
        get
        {
            return GameData.Slot1;
        }
        set
        {
            GameData.Slot1 = value;
        }
    }
    public Elemental ElementalSlot2
    {
        get
        {
            return GameData.Slot2;
        }
        set
        {
            GameData.Slot2 = value;
        }
    }
    public Elemental CurrentSelectedWeapon
    {
        get
        {
            return GameData.CurrentSelectedWeapon;
        }
        set
        {
            GameData.CurrentSelectedWeapon = value;
        }
    }
    public int Gold
    {
        get
        {
            return GameData.Gold;
        }
        set
        {
            GameData.Gold = value;
        }
    }
    #endregion

    #region in game
    public StageData StageData;
    public int CurrentSelected;
    public InGameWeapon inGameWeapons;
    public IngameAlliance IngameAlliance1, IngameAlliance2;
    public BaseData InGameBaseData;
    public int GoldInGame;
    public int MaxStage
    {
        get
        {
            return GameStageDataBase.MaxStage;
        }
    }
    #endregion
    private string dataPath = "";
    private void Start()
    {
        dataPath = Path.Combine(Application.persistentDataPath, "data.dat");
        DontDestroyOnLoad(gameObject);
    }

    public MonsterData GetMonsterData(string type)
    {
        return MonsterDataBases.GetMonsterData(type);
    }

    private void Load()
    {
        if (File.Exists(dataPath))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            string data;
            using (FileStream fileStream = File.Open(dataPath, FileMode.Open))
            {
                try
                {
                    data = (string)binaryFormatter.Deserialize(fileStream);
                    GameData = JsonUtility.FromJson<GameData>(data);
                }
                catch (System.Exception e)
                {
                    Debug.LogError(e.Message);
                    ResetData();
                }
            }
        }
        else
        {
            Debug.Log("<color=red> File not exist</color>");
            ResetData();
        }
        GameData.SaveItem(ITEM_TYPE.WindObs_1, 10);
        GameData.SaveItem(ITEM_TYPE.WindObs_2, 4);
        GameData.SaveItem(ITEM_TYPE.WindObs_3, 1);
        GameData.SaveItem(ITEM_TYPE.IceObs_3, 10);
        GameData.SaveItem(ITEM_TYPE.FireObs_1, 10);
        GameData.SaveItem(ITEM_TYPE.EarthObs_2, 4);
        Gold = 10000;
    }

    public void Save()
    {
        string origin = JsonUtility.ToJson(GameData);
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        using (FileStream fileStream = File.Open(dataPath, FileMode.OpenOrCreate))
        {
            binaryFormatter.Serialize(fileStream, origin);
        }
    }

    void ResetData()
    {
        GameData = new GameData
        {
            BaseHPLevel = 1,
            BaseHPTier = 1,
            BaseManaLevel = 1,
            BaseManaTier = 1,
            BaseShieldLevel = 1,
            BaseShieldTier = 1,
            CurrentStage = 1,
            gameDataWeapons = new List<GameDataWeapon> {
                new GameDataWeapon
        {
            Type = Elemental.Wind,
            ID = "Wind1",
            WeaponTierLevel = new SaveGameTierLevel
            {
                Tier = 1,
                Level = 1,
            },
            SkillTierLevel = new List<SaveGameTierLevel>() {
                new SaveGameTierLevel
            {
                Des = "WEAPON_WIND_SKILL_1",
                Tier = 1,
                Level = 1,
            }
            }
        },                new GameDataWeapon
        {
            Type = Elemental.Ice,
            ID = "Ice1",
            WeaponTierLevel = new SaveGameTierLevel
            {
                Tier = 1,
                Level = 0,
            }
        },                new GameDataWeapon
        {
            Type = Elemental.Fire,
            ID = "Fire1",
            WeaponTierLevel = new SaveGameTierLevel
            {
                Tier = 1,
                Level = 0,
            }
        },                new GameDataWeapon
        {
            Type = Elemental.Earth,
            ID = "Earth",
            WeaponTierLevel = new SaveGameTierLevel
            {
                Tier = 1,
                Level = 0,
            }
        }
            },
            gameDataAlliance = new List<GameDataWeapon>(),
            CurrentSelectedWeapon = Elemental.Wind,
            gameStages = new List<GameStage>()
        };
    }

    public void SetUpData()
    {
        ///Set up database
        WeaponsDatas = JsonUtility.FromJson<WeaponDatabase>(ConectingFireBase.Instance.GetTextWeaponDatabase());
        MonsterDataBases = JsonUtility.FromJson<MonsterDataBases>(ConectingFireBase.Instance.GetTextMonsterDataBases());
        GameStageDataBase = JsonUtility.FromJson<GameStageDataBase>(ConectingFireBase.Instance.GetTextGameStageDataBase());
        GameEnemyDataBase = JsonUtility.FromJson<GameEnemyDataBase>(ConectingFireBase.Instance.GetTextGameEnemyDataBase());
        AllianceDataBases = JsonUtility.FromJson<AllianceDataBase>(ConectingFireBase.Instance.GetTextAllianceDatabase());
        BaseDatabases = JsonUtility.FromJson<BaseDatabases>(ConectingFireBase.Instance.GetTextBaseDataBases());
        ItemDataBase = JsonUtility.FromJson<ItemDataBase>(ConectingFireBase.Instance.GetTextItemDataBase());
        BossDataBase_Wind = JsonUtility.FromJson<BossDataBase_Wind_1>(ConectingFireBase.Instance.GetTexSpawnEnemyBoss());
        ///Load data 
        Load();

        //LoadScene Menu
        SceneManager.LoadScene(1);
    }

    public void LoadIngameStage()
    {
        // Load Stage Data
        StageData = new StageData
        {
            Level = CurrentSelected,
            HardMode = GameData.GetGameStage(CurrentSelected).HardMode,
            stageDataBase = GameStageDataBase.GetStageDataBase(CurrentSelected),
            stageEnemyDataBase = GameEnemyDataBase.GetStageEnemyDataBase(CurrentSelected)
        };

        // load Weapon data
        // GameData.CurrentSelectedWeapon
        var slwp = GetGameDataWeapon(GameData.CurrentSelectedWeapon);
        var wp = GetDataBaseWeapons(slwp.Type, slwp.WeaponTierLevel.Tier);
        inGameWeapons = new InGameWeapon
        {
            Type = wp.Type,
            Tier = wp.Tier,
            Level = slwp.WeaponTierLevel.Level,
            ATK = wp.ATK[slwp.WeaponTierLevel.Level - 1],
            ATKspeed = wp.ATKspeed[slwp.WeaponTierLevel.Level - 1],
            BulletSpeed = wp.BulletSpeed,
        };

        // load alliance data
        //Load slot 1
        if (GameData.Slot1 != Elemental.None)
        {
            //load
            var sl1 = GetGameAlliance(GameData.Slot1);
            var wp1 = GetAllianceDataBases(sl1.Type, sl1.WeaponTierLevel.Tier);
            IngameAlliance1 = new IngameAlliance
            {
                Type = wp1.weapons.Type,
                Tier = wp1.weapons.Tier,
                Level = sl1.WeaponTierLevel.Level,
                ATK = wp1.weapons.ATK[sl1.WeaponTierLevel.Level - 1],
                ATKspeed = wp1.weapons.ATKspeed[slwp.WeaponTierLevel.Level - 1],
                BulletSpeed = wp1.weapons.BulletSpeed
            };
        }

        //Load slot 2
        if (GameData.Slot2 != Elemental.None)
        {
            //load
            var sl2 = GetGameAlliance(GameData.Slot2);
            var wp2 = GetAllianceDataBases(sl2.Type, sl2.WeaponTierLevel.Tier);
            IngameAlliance2 = new IngameAlliance
            {
                Type = wp2.weapons.Type,
                Tier = wp2.weapons.Tier,
                Level = sl2.WeaponTierLevel.Level,
                ATK = wp2.weapons.ATK[sl2.WeaponTierLevel.Level - 1],
                ATKspeed = wp2.weapons.ATKspeed[slwp.WeaponTierLevel.Level - 1],
                ATKRange = wp2.ATKRange[slwp.WeaponTierLevel.Level - 1],
                BulletSpeed = wp2.weapons.BulletSpeed
            };
        }

        // load base
        var bdbHP = BaseDatabases.GetBaseHpData(GameData.BaseHPTier);
        var bdbMana = BaseDatabases.GetBaseManaData(GameData.BaseManaTier);
        var bdbShield = BaseDatabases.GetBaseShieldData(GameData.BaseShieldTier);
        InGameBaseData = new BaseData
        {
            HP = bdbHP.Value1[GameData.BaseHPLevel - 1],
            HPRegen = bdbHP.Value2[GameData.BaseHPLevel - 1],
            Mana = bdbMana.Value1[GameData.BaseManaLevel - 1],
            ManaRegen = bdbMana.Value2[GameData.BaseManaLevel - 1],
            ShieldBlockValue = bdbShield.Value1[GameData.BaseShieldLevel - 1],
            ShieldBlockChance = bdbShield.Value2[GameData.BaseShieldLevel - 1],
        };

    }

    public Weapons GetDataBaseWeapons(Elemental elemental, int Tier)
    {
        return WeaponsDatas.GetWeapons(elemental, Tier);
    }

    public GameDataWeapon GetGameDataWeapon(Elemental elemental)
    {
        return GameData.GetGameDataWeapon(elemental);
    }

    public GameDataWeapon GetGameAlliance(Elemental elemental)
    {
        return GameData.GetGameDataAlliance(elemental);
    }

    public AllianceData GetAllianceDataBases(Elemental elemental, int Tier)
    {
        return AllianceDataBases.GetAlliance(elemental, Tier);
    }

    public Item GetGameItemData(ITEM_TYPE _TYPE)
    {
        return GameData.GetItem(_TYPE);
    }

    public ItemData GetItemDataBase(ITEM_TYPE _TYPE)
    {
        return ItemDataBase.GetItemData(_TYPE);
    }

    public void AddWeaponTier(Elemental elemental)
    {
        var gdw = GameData.GetGameDataWeapon(elemental);
        gdw.EXP = 0;
        gdw.WeaponTierLevel.Level = 1;
        gdw.WeaponTierLevel.Tier++;
    }

    public void AddAllianceTier(Elemental elemental)
    {
        var gdw = GameData.GetGameDataAlliance(elemental);
        gdw.EXP = 0;
        gdw.WeaponTierLevel.Level = 1;
        gdw.WeaponTierLevel.Tier++;
    }

    public void AddWeaponLevel(Elemental elemental, int AddLevel, int CurrentEXP)
    {
        var gdw = GameData.GetGameDataWeapon(elemental);
        gdw.EXP = CurrentEXP;
        gdw.WeaponTierLevel.Level += AddLevel;
    }

    public void AddAllianceLevel(Elemental elemental, int AddLevel, int CurrentEXP)
    {
        var gdw = GameData.GetGameDataAlliance(elemental);
        gdw.EXP = CurrentEXP;
        gdw.WeaponTierLevel.Level += AddLevel;
    }

    public void AddItemQuality(ITEM_TYPE type, int number)
    {
        GameData.AddItemQuality(type, number);
    }

    public GameStage GetGameStage(int level)
    {
        return GameData.GetGameStage(level);
    }

    public StageDataBase GetStageDataBase(int Level)
    {
        return GameStageDataBase.GetStageDataBase(Level);
    }

    public void UnLockAlliance(Elemental elemental)
    {
        var gdw = GameData.GetGameDataAlliance(elemental);
        if (gdw.WeaponTierLevel.Tier == 1 && gdw.WeaponTierLevel.Level == 0)
            gdw.WeaponTierLevel.Level = 1;
    }

    public void UnLockWeapon(Elemental elemental)
    {
        var gdw = GameData.GetGameDataWeapon(elemental);
        if (gdw.WeaponTierLevel.Tier == 1 && gdw.WeaponTierLevel.Level == 0)
            gdw.WeaponTierLevel.Level = 1;
    }

    public SaveGameTierLevel GetGameSkillData(string SkillID)
    {
        /// SkillID = WEAPON_ICE_SKILL_1 || SkillID = ALLIANCE_ICE_SKILL_1
        /// 
        var skillInfo = SkillID.Split('_');
        Elemental e = ConvertToElement(skillInfo[1]);
        if (skillInfo[0] == "WEAPON")
            return GetGameDataWeapon(e).GetSkillTierLevel(SkillID);
        else
            return GetGameAlliance(e).GetSkillTierLevel(SkillID);
    }

    public Elemental ConvertToElement(string srt)
    {
        switch (srt.ToLower())
        {
            case "wind":
                return Elemental.Wind;
            case "ice":
                return Elemental.Ice;
            case "earth":
                return Elemental.Earth;
            case "fire":
                return Elemental.Fire;
        }
        return Elemental.None;
    }
}




