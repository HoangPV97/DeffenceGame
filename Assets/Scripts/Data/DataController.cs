using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InviGiant.Tools;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;
using System;
using Newtonsoft.Json;
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
    public BossStageDataBase BossStageDataBase;
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
            if (value < GameData.Gold)
                CheckAchievement(ACHIEVEMENT_TYPE.ACHIEVEMENT_1, GameData.Gold - value);
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
                    GameData = JsonConvert.DeserializeObject<GameData>(data);
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

        CheckDailyQuest();
    }

    public void CheckDailyQuest()
    {
        float offlineTotalSeconds = (float)((DateTime.Now - GameData.timeStamp).TotalSeconds);
        Debug.Log(GameData.timeStamp);
        Debug.Log(offlineTotalSeconds);
        if (offlineTotalSeconds > 0)
            if ((float)((DateTime.Now - GameData.timeStamp).TotalSeconds) >= 24 * 60 * 60 || DateTime.Now.Hour < GameData.timeStamp.Hour)
            {
                GameData.ResetDailyQuest();
                Save();
            }
    }

    public void Save()
    {
        Debug.Log("Save():" + DateTime.Now);
        GameData.timeStamp = DateTime.Now;
        string origin = JsonConvert.SerializeObject(GameData);
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
        BossStageDataBase = JsonUtility.FromJson<BossStageDataBase>(ConectingFireBase.Instance.GetTextBossStageDatabase());
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
            stageDataBase = GameStageDataBase.GetStageDataBase(CurrentSelected).Clone(),
            stageEnemyDataBase = GameEnemyDataBase.GetStageEnemyDataBase(CurrentSelected).Clone()
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
            ATK = (wp.GetATK(slwp.WeaponTierLevel.Level) + InGameBaseData.Damage) * InGameBaseData.achi_AddedDmgWeaponAlliance * InGameBaseData.achi_AddedDmgWeapon,
            ATKspeed = wp.GetATKspeed(slwp.WeaponTierLevel.Level),
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
                ATK = wp1.weapons.GetATK(sl1.WeaponTierLevel.Level) * InGameBaseData.achi_AddedDmgWeaponAlliance,
                ATKspeed = wp1.weapons.GetATKspeed(slwp.WeaponTierLevel.Level),
                ATKRange = wp1.GetATKRange(slwp.WeaponTierLevel.Level),
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
                ATK = wp2.weapons.GetATK(sl2.WeaponTierLevel.Level) * InGameBaseData.achi_AddedDmgWeaponAlliance,
                ATKspeed = wp2.weapons.GetATKspeed(slwp.WeaponTierLevel.Level),
                ATKRange = wp2.GetATKRange(slwp.WeaponTierLevel.Level),
                BulletSpeed = wp2.weapons.BulletSpeed
            };
        }

        // load base
        InGameBaseData = new BaseData();
        SaveGameTierLevel sgd, sgd1, sgd2, sgd3, sgd4;
        BaseDatabase baseData;
        List<string> Skills;
        SkillData skillData1, skillData2, skillData3, skillData4;
        /// Caculate ingame data
        /// Fortress
        sgd = GetFortressGameData();
        baseData = BaseDatabases.GetBaseFortressData(sgd.Tier);
        InGameBaseData.HP = (int)baseData.GetAttributeValue("HP", sgd.Level);
        InGameBaseData.ShieldBlockValue = (int)baseData.GetAttributeValue("Shield", sgd.Level);
        InGameBaseData.HPRegen = (int)baseData.GetAttributeValue("HPRegen", sgd.Level);

        Skills = GetFortressSkillID();
        sgd1 = GetGameSkillData(Skills[0]);
        skillData1 = ConectingFireBase.Instance.GetSkillData(Skills[0]);
        if (sgd1.Level > 0)
        {
            InGameBaseData.ShieldBlockValue += (int)skillData1.GetSkillAttributes("AddedShield", sgd1.Tier, sgd1.Level);
        }

        sgd2 = GetGameSkillData(Skills[1]);
        skillData2 = ConectingFireBase.Instance.GetSkillData(Skills[1]);
        if (sgd2.Level > 0)
        {
            InGameBaseData.HPRegen += (int)skillData2.GetSkillAttributes("AddedHPRegen", sgd2.Tier, sgd2.Level);
        }

        sgd3 = GetGameSkillData(Skills[2]);
        skillData3 = ConectingFireBase.Instance.GetSkillData(Skills[2]);
        if (sgd3.Level > 0)
        {
            InGameBaseData.ShieldBlockChance += (int)skillData3.GetSkillAttributes("Block100PercentDamageChance", sgd3.Tier, sgd3.Level);
        }

        sgd4 = GetGameSkillData(Skills[3]);
        skillData4 = ConectingFireBase.Instance.GetSkillData(Skills[3]);
        if (sgd4.Level > 0)
        {
            InGameBaseData.HP += (int)skillData4.GetSkillAttributes("AddedHP", sgd4.Tier, sgd4.Level);

            InGameBaseData.ShieldBlockValue += (int)skillData4.GetSkillAttributes("AddedShield", sgd4.Tier, sgd4.Level);
        }

        /// Archery
        sgd = GetArcheryGameData();
        baseData = BaseDatabases.GetBaseArcheryData(sgd.Tier);
        InGameBaseData.Damage = (int)baseData.GetAttributeValue("Damage", sgd.Level);
        Skills = GetArcherySkillID();
        sgd1 = GetGameSkillData(Skills[0]);
        skillData1 = ConectingFireBase.Instance.GetSkillData(Skills[0]);
        if (sgd1.Level > 0)
        {
            InGameBaseData.Critical += (int)skillData1.GetSkillAttributes("CriticalChance", sgd1.Tier, sgd1.Level);
        }

        sgd2 = GetGameSkillData(Skills[1]);
        skillData2 = ConectingFireBase.Instance.GetSkillData(Skills[1]);
        if (sgd2.Level > 0)
        {
            InGameBaseData.KnockBackChance += (int)skillData2.GetSkillAttributes("KnockBackChance", sgd2.Tier, sgd2.Level);
        }

        sgd3 = GetGameSkillData(Skills[2]);
        skillData3 = ConectingFireBase.Instance.GetSkillData(Skills[2]);
        if (sgd3.Level > 0)
        {
            InGameBaseData.QuickHand = (int)skillData3.GetSkillAttributes("QuickHandChance", sgd3.Tier, sgd3.Level);
            InGameBaseData.QuickHandDamagePercent = (int)skillData3.GetSkillAttributes("DamagePercent", sgd3.Tier, sgd3.Level);
        }

        sgd4 = GetGameSkillData(Skills[3]);
        skillData4 = ConectingFireBase.Instance.GetSkillData(Skills[3]);
        if (sgd4.Level > 0)
        {
            InGameBaseData.MultiShot = true;
            InGameBaseData.MultiShotDamage = (int)skillData4.GetSkillAttributes("DamagePercent", sgd4.Tier, sgd4.Level);
            InGameBaseData.MultiShotAddedAttributePercent = InGameBaseData.MultiShotDamage;
        }
        else
        {
            InGameBaseData.MultiShot = false;
        }

        ///Temple
        sgd = GetTempleGameData();
        baseData = BaseDatabases.GetBaseTempleData(sgd.Tier);
        InGameBaseData.AllianceDamage = (int)baseData.GetAttributeValue("AllianceDamage", sgd.Level);
        InGameBaseData.Mana = (int)baseData.GetAttributeValue("Mana", sgd.Level);
        InGameBaseData.ManaRegen = (int)baseData.GetAttributeValue("ManaRegen", sgd.Level);
        Skills = GetArcherySkillID();
        sgd1 = GetGameSkillData(Skills[0]);
        skillData1 = ConectingFireBase.Instance.GetSkillData(Skills[0]);
        if (sgd1.Level > 0)
        {
            InGameBaseData.Mana += (int)skillData1.GetSkillAttributes("AddedMana", sgd1.Tier, sgd1.Level);
        }

        sgd2 = GetGameSkillData(Skills[1]);
        skillData2 = ConectingFireBase.Instance.GetSkillData(Skills[1]);
        if (sgd2.Level > 0)
        {
            InGameBaseData.ManaRegen += (int)skillData2.GetSkillAttributes("AddedManaRegen", sgd2.Tier, sgd2.Level);
        }

        sgd3 = GetGameSkillData(Skills[2]);
        skillData3 = ConectingFireBase.Instance.GetSkillData(Skills[2]);
        if (sgd3.Level > 0)
        {
            InGameBaseData.ReduceCooldown = (int)skillData3.GetSkillAttributes("ReduceCoolDownRate", sgd3.Tier, sgd3.Level);
        }

        sgd4 = GetGameSkillData(Skills[3]);
        skillData4 = ConectingFireBase.Instance.GetSkillData(Skills[3]);
        if (sgd4.Level > 0)
        {
            InGameBaseData.IncreaseSpellDamage = (int)skillData4.GetSkillAttributes("IncreaseSpellDamageRate", sgd4.Tier, sgd4.Level);
        }
        InGameBaseData.achi_AddedGoldKilled += (int)GetAchivementReward(ACHIEVEMENT_TYPE.ACHIEVEMENT_1);
        InGameBaseData.achi_AddedDmgWeaponAlliance += GetAchivementReward(ACHIEVEMENT_TYPE.ACHIEVEMENT_2) / 100;
        InGameBaseData.achi_AddedDmgSpellWind += GetAchivementReward(ACHIEVEMENT_TYPE.ACHIEVEMENT_3) / 100;
        InGameBaseData.achi_AddedDmgSpellEarth += GetAchivementReward(ACHIEVEMENT_TYPE.ACHIEVEMENT_4) / 100;
        InGameBaseData.achi_AddedDmgSpellIce += GetAchivementReward(ACHIEVEMENT_TYPE.ACHIEVEMENT_5) / 100;
        InGameBaseData.achi_AddedDmgSpellFire += GetAchivementReward(ACHIEVEMENT_TYPE.ACHIEVEMENT_6) / 100;
        InGameBaseData.achi_AddedDmgAllianceWind += GetAchivementReward(ACHIEVEMENT_TYPE.ACHIEVEMENT_7) / 100;
        InGameBaseData.achi_AddedDmgAllianceEarth += GetAchivementReward(ACHIEVEMENT_TYPE.ACHIEVEMENT_8) / 100;
        InGameBaseData.achi_AddedDmgAllianceIce += GetAchivementReward(ACHIEVEMENT_TYPE.ACHIEVEMENT_9) / 100;
        InGameBaseData.achi_AddedDmgAllianceFire += GetAchivementReward(ACHIEVEMENT_TYPE.ACHIEVEMENT_10) / 100;
        InGameBaseData.achi_AddedDmgWeapon += GetAchivementReward(ACHIEVEMENT_TYPE.ACHIEVEMENT_11) / 100 + GetAchivementReward(ACHIEVEMENT_TYPE.ACHIEVEMENT_13) / 100;
        InGameBaseData.achi_AddedDmgAlliance += GetAchivementReward(ACHIEVEMENT_TYPE.ACHIEVEMENT_12) / 100 + GetAchivementReward(ACHIEVEMENT_TYPE.ACHIEVEMENT_14) / 100;
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
        if (gdw.WeaponTierLevel.Tier == 3)
        {
            CheckAchievement(ACHIEVEMENT_TYPE.ACHIEVEMENT_11, 1);
        }
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
        if (gdw.WeaponTierLevel.Tier == 3)
        {
            CheckAchievement(ACHIEVEMENT_TYPE.ACHIEVEMENT_12, 1);
        }
    }

    public void AddSkillLevel(string SkillID)
    {
        GetGameSkillData(SkillID).Level++;
        CheckDailyQuest(QUEST_TYPE.QUEST_4, 1);
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
        CheckDailyQuest(QUEST_TYPE.QUEST_2, AddLevel);
        if (gdw.WeaponTierLevel.Level == 100)
            CheckAchievement(ACHIEVEMENT_TYPE.ACHIEVEMENT_13, 1);
    }

    public void AddAllianceLevel(Elemental elemental, int AddLevel, int CurrentEXP)
    {
        var gdw = GameData.GetGameDataAlliance(elemental);
        gdw.EXP = CurrentEXP;
        gdw.WeaponTierLevel.Level += AddLevel;
        CheckDailyQuest(QUEST_TYPE.QUEST_2, AddLevel);
        if (gdw.WeaponTierLevel.Level == 100)
            CheckAchievement(ACHIEVEMENT_TYPE.ACHIEVEMENT_14, 1);
    }

    public void AddArcheryLevel()
    {
        GetArcheryGameData().Level++;
    }

    public void AddArcheryTier()
    {
        var sgd = GetArcheryGameData();
        sgd.Tier++;
        sgd.Level = 1;
        var SkillList = GetArcherySkillID();
        string skillID = SkillList[sgd.Tier - 1];
        var sgl = GetGameSkillData(skillID);
        sgl.Level = 1;
    }

    public void AddTempleLevel()
    {
        GetTempleGameData().Level++;
    }

    public void AddTempleTier()
    {
        var sgd = GetTempleGameData();
        sgd.Tier++;
        sgd.Level = 1;
        var SkillList = GetTempleSkillID();
        string skillID = SkillList[sgd.Tier - 1];
        var sgl = GetGameSkillData(skillID);
        sgl.Level = 1;
    }
    public void AddFortressLevel()
    {
        GetFortressGameData().Level++;
    }

    public void AddFortressTier()
    {
        var sgd = GetFortressGameData();
        sgd.Tier++;
        sgd.Level = 1;
        var SkillList = GetFortressSkillID();
        string skillID = SkillList[sgd.Tier - 1];
        var sgl = GetGameSkillData(skillID);
        sgl.Level = 1;
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
        SetStringUNLOCK_UI("Alliance_" + elemental.ToString());
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
        SetStringUNLOCK_UI("Weapon_" + elemental.ToString());
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
        return new List<string>() { "BASE_TEMPLE_SKILL_1", "BASE_TEMPLE_SKILL_2", "BASE_TEMPLE_SKILL_3", "BASE_TEMPLE_SKILL_4" };
    }

    public List<string> GetFortressSkillID()
    {
        return new List<string>() { "BASE_FORTRESS_SKILL_1", "BASE_FORTRESS_SKILL_2", "BASE_FORTRESS_SKILL_3", "BASE_FORTRESS_SKILL_4" };
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
    public GameDataAchievement GetGameDataAchievement(ACHIEVEMENT_TYPE _TYPE)
    {
        return GameData.GetGameDataAchievement(_TYPE);
    }
    public AchievementDatabase GetAchievementDatabase(ACHIEVEMENT_TYPE _TYPE)
    {
        return DefaultData.GetAchievementDatabase(_TYPE);
    }

    public DailyQuestDatabase GetDailyQuestDatabase(QUEST_TYPE _TYPE)
    {
        return DefaultData.GetDailyQuestDatabase(_TYPE);
    }

    public List<GameDataQuest> GetGameDataQuests()
    {
        return GameData.gameDataQuests;
    }

    public void CheckDailyQuest(QUEST_TYPE _TYPE, int value)
    {
        for (int i = 0; i < GameData.gameDataQuests.Count; i++)
        {
            if (GameData.gameDataQuests[i].QUEST_TYPE == _TYPE)
            {
                GameData.gameDataQuests[i].AddCurrent(value);
            }
        }
    }

    public void CheckAchievement(ACHIEVEMENT_TYPE _TYPE, int value)
    {
        GetGameDataAchievement(_TYPE).AddCurrent(value);
    }

    public void ClaimDailyQuestReward(QUEST_TYPE _TYPE)
    {
        for (int i = 0; i < GameData.gameDataQuests.Count; i++)
        {
            if (GameData.gameDataQuests[i].QUEST_TYPE == _TYPE)
            {
                GameData.gameDataQuests[i].ClaimReward();
            }
        }
        GameData.gameDataQuestLevels[(int)_TYPE]++;
    }

    public float GetAchivementReward(ACHIEVEMENT_TYPE _TYPE)
    {
        var gda = GetGameDataAchievement(_TYPE);
        var adb = GetAchievementDatabase(_TYPE);
        return adb.GetReward(gda.Level);
    }

    public void SetStringUNLOCK_UI(string str)
    {
        GameData.UNLOCK_UI = str;
    }

    public string GetStringUNLOCK_UI()
    {
        return GameData.UNLOCK_UI;
    }
}




