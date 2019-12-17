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
    #endregion

    #region Data Player
    public GameData GameData;
    #endregion

    #region in game
    public StageData StageData;
    public int CurrentSelected;
    public InGameWeapon inGameWeapons;
    public IngameAlliance IngameAlliance1, IngameAlliance2;
    public BaseData InGameBaseData;
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
        },                new GameDataWeapon
        {
            Type = Elemental.Ice,
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
        },                new GameDataWeapon
        {
            Type = Elemental.Fire,
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
        },                new GameDataWeapon
        {
            Type = Elemental.Earth,
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
        }
            },
            gameDataAlliance = new List<GameDataWeapon> {
               new GameDataWeapon
        {
            Type = Elemental.Ice,
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
        }
                ,new GameDataWeapon
        {
            Type = Elemental.Fire,
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
        },
            },
            CurrentSelectedWeapon = Elemental.Wind,
            // Slot1 = Elemental.Ice,
            //  Slot2 = Elemental.Fire,
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
        var slwp = GameData.GetGameDataWeapon(GameData.CurrentSelectedWeapon);
        var wp = WeaponsDatas.GetWeapons(slwp.Type, slwp.WeaponTierLevel.Tier);
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
            var sl1 = GameData.GetGameAlliance(GameData.Slot1);
            var wp1 = AllianceDataBases.GetAlliance(sl1.Type, sl1.WeaponTierLevel.Tier);
            IngameAlliance1 = new IngameAlliance
            {
                Type = wp1.Type,
                Tier = wp1.Tier,
                Level = sl1.WeaponTierLevel.Level,
                ATK = wp1.ATK[sl1.WeaponTierLevel.Level - 1],
                ATKspeed = wp1.ATKspeed[slwp.WeaponTierLevel.Level - 1],
                BulletSpeed = wp1.BulletSpeed
            };
        }

        //Load slot 2
        if (GameData.Slot2 != Elemental.None)
        {
            //load
            var sl2 = GameData.GetGameAlliance(GameData.Slot2);
            var wp2 = AllianceDataBases.GetAlliance(sl2.Type, sl2.WeaponTierLevel.Tier);
            IngameAlliance2 = new IngameAlliance
            {
                Type = wp2.Type,
                Tier = wp2.Tier,
                Level = sl2.WeaponTierLevel.Level,
                ATK = wp2.ATK[sl2.WeaponTierLevel.Level - 1],
                ATKspeed = wp2.ATKspeed[slwp.WeaponTierLevel.Level - 1],
                ATKRange = wp2.ATKRange[slwp.WeaponTierLevel.Level - 1],
                BulletSpeed = wp2.BulletSpeed
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
}



