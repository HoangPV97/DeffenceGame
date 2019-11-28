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
    #endregion

    #region Data Player
    public GameData GameData;
    #endregion

    #region in game
    public StageData StageData;
    public int CurrentSelected;
    public List<InGameWeapon> inGameWeapons;
    #endregion

    private string dataPath = "";
    private void Start()
    {
        dataPath = Path.Combine(Application.persistentDataPath, "data.dat");
        DontDestroyOnLoad(gameObject);
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
            ResetData();
    }

    private void Save()
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
            BaseLevel = 1,
            CurrentStage = 1,
            gameDataWeapons = new List<GameDataWeapon> {
                new GameDataWeapon{
                    Type = Elemental.Wind,
                    Tier =1,
                    Level =1
                }
            },
            EquipedWeapon = new List<Elemental> {
                Elemental.Wind
            }
        };
    }

    public void SetUpData()
    {
        ///Set up database
        WeaponsDatas = JsonUtility.FromJson<WeaponDatabase>(ConectingFireBase.Instance.GetTextWeaponDatabase());

        ///Load data 
        Load();

        //LoadScene Menu
        SceneManager.LoadScene(1);
    }

    public void LoadIngameStage()
    {

    }
}



