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
    [SerializeField]
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
    public int Gem
    {
        get
        {
            return GameData.Gem;
        }
        set
        {
            GameData.Gem = value;
        }
    }
    #endregion

    #region in game
    public StageData StageData;
    public int CurrentSelected
    {
        get
        {
            return GameData.CurrentStage;
        }
        set
        {
            GameData.CurrentStage = value;
        }
    }
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
        Language.ChangeLanguage(Languages.en);
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
        GameData.SaveItem(ITEM_TYPE.WindObs_1, 200);
        GameData.SaveItem(ITEM_TYPE.WindObs_2, 2000);
        GameData.SaveItem(ITEM_TYPE.WindObs_3, 2000);
        GameData.SaveItem(ITEM_TYPE.IceObs_3, 2000);
        GameData.SaveItem(ITEM_TYPE.FireObs_1, 10);
        GameData.SaveItem(ITEM_TYPE.EarthObs_2, 4);
        GameData.SaveItem(ITEM_TYPE.EarthObs_3, 244);
        Gold = 1000000;
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
            Archery = new SaveGameTierLevel { Level = 1, Tier = 1 },
            ArcherySkillTierLevel = new List<SaveGameTierLevel> { new SaveGameTierLevel { Level = 1, Tier = 1,Des = "ARCHERY_SKILL_1" },
            new SaveGameTierLevel { Level = 0, Tier = 1,Des = "ARCHERY_SKILL_2" },
            new SaveGameTierLevel { Level = 0, Tier = 1,Des = "ARCHERY_SKILL_3" },
            new SaveGameTierLevel { Level = 0, Tier = 1,Des = "ARCHERY_SKILL_4" }},
            Temple = new SaveGameTierLevel { Level = 1, Tier = 1 },
            TempleSkillTierLevel = new List<SaveGameTierLevel> { new SaveGameTierLevel { Level = 1, Tier = 1,Des = "TEMPLE_SKILL_1" },
            new SaveGameTierLevel { Level = 0, Tier = 1,Des = "TEMPLE_SKILL_2" },
            new SaveGameTierLevel { Level = 0, Tier = 1,Des = "TEMPLE_SKILL_3" },
            new SaveGameTierLevel { Level = 0, Tier = 1,Des = "TEMPLE_SKILL_4" }},
            Fortress = new SaveGameTierLevel { Level = 1, Tier = 1 },
            FortressSkillTierLevel = new List<SaveGameTierLevel> { new SaveGameTierLevel { Level = 1, Tier = 1,Des = "FORTRESS_SKILL_1" },
            new SaveGameTierLevel { Level = 0, Tier = 1,Des = "FORTRESS_SKILL_2" },
            new SaveGameTierLevel { Level = 0, Tier = 1,Des = "FORTRESS_SKILL_3" },
            new SaveGameTierLevel { Level = 0, Tier = 1,Des = "FORTRESS_SKILL_4" }},
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
        var bdbHP = BaseDatabases.GetBaseFortressData(GameData.Fortress.Level);
        var bdbMana = BaseDatabases.GetBaseTempleData(GameData.Temple.Level);
        var bdbShield = BaseDatabases.GetBaseShieldData(GameData.Fortress.Level);
        InGameBaseData = new BaseData
        {
            HP = bdbHP.GetAttributeValue("HP", GameData.Fortress.Level),
            HPRegen = bdbHP.GetAttributeValue("HPRegen", GameData.Fortress.Level),
            Mana = bdbMana.GetAttributeValue("Mana", GameData.Temple.Level),
            ManaRegen = bdbMana.GetAttributeValue("ManaRegen", GameData.Temple.Level),
            ShieldBlockValue = bdbMana.GetAttributeValue("ShieldBlockValue", GameData.Fortress.Level),
            ShieldBlockChance = bdbMana.GetAttributeValue("ShieldBlockChance", GameData.Fortress.Level),
            //   Mana = bdbMana.Value1[GameData.Temple.Level - 1],
            //  ManaRegen = bdbMana.Value2[GameData.Temple.Level - 1],
            //  ShieldBlockValue = bdbShield.Value1[GameData.Fortress.Level - 1],
            //  ShieldBlockChance = bdbShield.Value2[GameData.Fortress.Level - 1],
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
        var SkillList = GetWeaponSkillID(elemental);
        string skillID = SkillList[gdw.WeaponTierLevel.Tier - 1];
        var sgl = GetGameSkillData(skillID);
        sgl.Level = 1;
    }

    public void AddAllianceTier(Elemental elemental)
    {
        var gdw = GameData.GetGameDataAlliance(elemental);
        gdw.EXP = 0;
        gdw.WeaponTierLevel.Level = 1;
        gdw.WeaponTierLevel.Tier++;
        var SkillList = GetAllianceSkillID(elemental);
        string skillID = SkillList[gdw.WeaponTierLevel.Tier - 1];
        var sgl = GetGameSkillData(skillID);
        sgl.Level = 1;
    }

    public void AddSkillLevel(string SkillID)
    {
        GetGameSkillData(SkillID).Level++;
    }

    public void AddSkillTier(string SkillID)
    {
        var gsd = GetGameSkillData(SkillID);
        gsd.Level = 1;
        gsd.Tier++;
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

    public void AddArcheryLevel()
    {
        GetArcheryGameData().Level++;
    }

    public void AddArcheryTier()
    {
        GetArcheryGameData().Tier++;
    }

    public void AddTempleLevel()
    {
        GetTempleGameData().Level++;
    }

    public void AddTempleTier()
    {
        GetTempleGameData().Tier++;
    }
    public void AddFortressLevel()
    {
        GetFortressGameData().Level++;
    }

    public void AddFortressTier()
    {
        GetFortressGameData().Tier++;
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
        var SkillList = GetAllianceSkillID(elemental);
        string skillID = SkillList[gdw.WeaponTierLevel.Tier - 1];
        var sgl = GetGameSkillData(skillID);
        sgl.Level = 1;
    }

    public void UnLockWeapon(Elemental elemental)
    {
        var gdw = GameData.GetGameDataWeapon(elemental);
        if (gdw.WeaponTierLevel.Tier == 1 && gdw.WeaponTierLevel.Level == 0)
            gdw.WeaponTierLevel.Level = 1;
        var SkillList = GetWeaponSkillID(elemental);
        string skillID = SkillList[gdw.WeaponTierLevel.Tier - 1];
        var sgl = GetGameSkillData(skillID);
        sgl.Level = 1;
    }

    public SaveGameTierLevel GetGameSkillData(string SkillID)
    {
        /// SkillID = WEAPON_ICE_SKILL_1 || SkillID = ALLIANCE_ICE_SKILL_1 || "BASE_ARCHERY_SKILL_1"
        /// 
        var skillInfo = SkillID.Split('_');
        Elemental e = ConvertToElement(skillInfo[1]);
        if (skillInfo[0] == "WEAPON")
            return GetGameDataWeapon(e).GetSkillTierLevel(SkillID);
        else if (skillInfo[0] == "ALLIANCE")
            return GetGameAlliance(e).GetSkillTierLevel(SkillID);
        else if (skillInfo[0] == "BASE" && skillInfo[1] == "ARCHERY")
        {
            for (int i = 0; i < GameData.ArcherySkillTierLevel.Count; i++)
            {
                if (GameData.ArcherySkillTierLevel[i].Des == SkillID)
                    return GameData.ArcherySkillTierLevel[i];
            }
            SaveGameTierLevel sgtl = new SaveGameTierLevel
            {
                Des = SkillID,
                Tier = 1,
                Level = 0
            };
            if (int.Parse(skillInfo[3]) == 1)
            {
                sgtl.Level = 1;
            }
            GameData.ArcherySkillTierLevel.Add(sgtl);
            return sgtl;
        }
        else if (skillInfo[0] == "BASE" && skillInfo[1] == "TEMPLE")
        {
            for (int i = 0; i < GameData.TempleSkillTierLevel.Count; i++)
            {
                if (GameData.TempleSkillTierLevel[i].Des == SkillID)
                    return GameData.TempleSkillTierLevel[i];
            }
            SaveGameTierLevel sgtl = new SaveGameTierLevel
            {
                Des = SkillID,
                Tier = 1,
                Level = 0
            };
            if (int.Parse(skillInfo[3]) == 1)
            {
                sgtl.Level = 1;
            }
            GameData.TempleSkillTierLevel.Add(sgtl);
            return sgtl;
        }
        else if (skillInfo[0] == "BASE" && skillInfo[1] == "FORTRESS")
        {
            for (int i = 0; i < GameData.FortressSkillTierLevel.Count; i++)
            {
                if (GameData.FortressSkillTierLevel[i].Des == SkillID)
                    return GameData.FortressSkillTierLevel[i];
            }
            SaveGameTierLevel sgtl = new SaveGameTierLevel
            {
                Des = SkillID,
                Tier = 1,
                Level = 0
            };
            if (int.Parse(skillInfo[3]) == 1)
            {
                sgtl.Level = 1;
            }
            GameData.FortressSkillTierLevel.Add(sgtl);
            return sgtl;
        }
        return null;
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

    public List<string> GetWeaponSkillID(Elemental elemental)
    {
        switch (elemental)
        {
            case Elemental.None:
                break;
            case Elemental.Wind:
                return new List<string>() { "WEAPON_WIND_SKILL_1", "WEAPON_WIND_SKILL_2", "WEAPON_WIND_SKILL_3", "WEAPON_WIND_SKILL_4" };
            case Elemental.Ice:
                return new List<string>() { "WEAPON_ICE_SKILL_1", "WEAPON_ICE_SKILL_2", "WEAPON_ICE_SKILL_3", "WEAPON_ICE_SKILL_4" };
            case Elemental.Earth:
                return new List<string>() { "WEAPON_EARTH_SKILL_1", "WEAPON_EARTH_SKILL_2", "WEAPON_EARTH_SKILL_3", "WEAPON_EARTH_SKILL_4" };
            case Elemental.Fire:
                return new List<string>() { "WEAPON_FIRE_SKILL_1", "WEAPON_FIRE_SKILL_2", "WEAPON_FIRE_SKILL_3", "WEAPON_FIRE_SKILL_4" };
        }
        return null;
    }

    public List<string> GetArcherySkillID()
    {
        return new List<string>() { "BASE_ARCHERY_SKILL_1", "BASE_ARCHERY_SKILL_2", "BASE_ARCHERY_SKILL_3", "BASE_ARCHERY_SKILL_4" };
    }

    public List<string> GetTempleSkillID()
    {
        return new List<string>() { "BASE_ARCHERY_SKILL_1", "BASE_ARCHERY_SKILL_2", "BASE_ARCHERY_SKILL_3", "BASE_ARCHERY_SKILL_4" };
    }

    public List<string> GetFortressSkillID()
    {
        return new List<string>() { "ARCHERY_SKILL_1", "ARCHERY_SKILL_2", "ARCHERY_SKILL_3", "ARCHERY_SKILL_4" };
    }

    public List<string> GetAllianceSkillID(Elemental elemental)
    {
        switch (elemental)
        {
            case Elemental.None:
                break;
            case Elemental.Wind:
                return new List<string>() { "ALLIANCE_WIND_SKILL_1", "ALLIANCE_WIND_SKILL_2", "ALLIANCE_WIND_SKILL_3", "ALLIANCE_WIND_SKILL_4" };
            case Elemental.Ice:
                return new List<string>() { "ALLIANCE_ICE_SKILL_1", "ALLIANCE_ICE_SKILL_2", "ALLIANCE_ICE_SKILL_3", "ALLIANCE_ICE_SKILL_4" };
            case Elemental.Earth:
                return new List<string>() { "ALLIANCE_EARTH_SKILL_1", "ALLIANCE_EARTH_SKILL_2", "ALLIANCE_EARTH_SKILL_3", "ALLIANCE_EARTH_SKILL_4" };
            case Elemental.Fire:
                return new List<string>() { "ALLIANCE_FIRE_SKILL_1", "ALLIANCE_FIRE_SKILL_2", "ALLIANCE_FIRE_SKILL_3", "ALLIANCE_FIRE_SKILL_4" };
        }
        return null;
    }

    public SaveGameTierLevel GetArcheryGameData()
    {
        return GameData.Archery;
    }

    public SaveGameTierLevel GetTempleGameData()
    {
        return GameData.Temple;
    }

    public SaveGameTierLevel GetFortressGameData()
    {
        return GameData.Fortress;
    }

}




