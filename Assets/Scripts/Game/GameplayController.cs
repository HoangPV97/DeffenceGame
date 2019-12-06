using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InviGiant.Tools;
public class GameplayController : Singleton<GameplayController>
{
    public PlayerController PlayerController;
    public GameObject Slot1,Slot2;
    public Tower Tower;
    [Header("Skill button")]
    public bool CancelSkill = false;
    public List<VariableJoystick> SkillButtons = new List<VariableJoystick>();
    public List<Transform> spawnPosition;

    private void Start()
    {
        LoadDataGamePlay();
    }
    public void LoadDataGamePlay()
    {
        
        var Alliance1 = ObjectPoolManager.Instance.SpawnObject(Resources .Load<GameObject>("Prefabs/IceAlliance"), Slot1.transform.position, Quaternion.identity);
        var Alliance2 = ObjectPoolManager.Instance.SpawnObject(Resources.Load<GameObject>("Prefabs/WindAlliance"), Slot2.transform.position, Quaternion.identity);
        ///SetUp Base first
        Tower.SetUpData();

        /// Load Weapon
        PlayerController.SetDataWeapon();
        /// Check slot 
        if (DataController.Instance.inGameWeapons.Tier == 1)
        {
            //set 1 skill button 
            // spawn skill controller
            var go = Instantiate(DataController.Instance.DefaultData.GetWeaponSkill(DataController.Instance.inGameWeapons.Type, 0), this.transform);
            go.GetComponent<Skill>().SetUpData(1, SkillButtons[0]);
        }
        else
        {
            // set 2 skill button
        }

        /// Load Alliance
        /// Load from resource
        ///Load slot 1
        if (DataController.Instance.GameData.Slot1 != Elemental.None)
        {
            //Load
            Alliance1.GetComponent<IceAllianceCharacter>().SetDataWeapon();
            // set  skill button
            var go = Instantiate(DataController.Instance.DefaultData.GetAllianceSkill(DataController.Instance.IngameAlliance1.Type), this.transform);
            go.GetComponent<Skill>().SetUpData(1, SkillButtons[1]);
        }

        ///Load slot 2
        if (DataController.Instance.GameData.Slot2 != Elemental.None)
        {
            //load
            Alliance2.GetComponent<WindAllianceCharacter>().SetDataWeapon();
            // set  skill button
            var go = Instantiate(DataController.Instance.DefaultData.GetAllianceSkill(DataController.Instance.IngameAlliance2.Type), this.transform);
            go.GetComponent<Skill>().SetUpData(1, SkillButtons[2]);
        }

        //spawn Enemy
        var sd = DataController.Instance.StageData;
        for (int i = 0; i < sd.stageEnemyDataBase.stageEnemies.Count; i++)
        {
            //StartCoroutine(IESpawnEnemy(i, sd.stageEnemyDataBase.stageEnemies[i].StartTime));
            StartCoroutine(IESpawnEnemy(i, 5));
            EnemyController.EnemyLive += sd.stageEnemyDataBase.stageEnemies[i].Number;
        }
        //voi

    }

    #region Monster
    IEnumerator IESpawnEnemy(int i, float timeDelay)
    {
        yield return new WaitForSeconds(timeDelay);
        var se = DataController.Instance.StageData.stageEnemyDataBase.stageEnemies[i];
        se.Number--;
        int level = se.Level;
        if (DataController.Instance.StageData.HardMode == 2)
            level += DataController.Instance.StageData.stageEnemyDataBase.NightMareAddLevel;
        else if (DataController.Instance.StageData.HardMode == 3)
            level += DataController.Instance.StageData.stageEnemyDataBase.HellAddLevel;
        // spawnEnemy
        GameObject m_Enemy = ObjectPoolManager.Instance.SpawnObject(se.Type, se.Position == 999 ? spawnPosition[Random.Range(0, 8)].position : spawnPosition[se.Position].position, transform.rotation);
        m_Enemy.GetComponent<EnemyController>().SetUpdata(se.Type, level);
        //repeat
        if (se.Number > 0)
        {
            StartCoroutine(IESpawnEnemy(i, se.RepeatTime));
        }

    }
    #endregion

    #region SkillAction

    public void OnCancelSkill(bool cancel)
    {
        CancelSkill = cancel;
    }

    public void OnCalcelSkillClick()
    {
        Debug.Log("OnCalcelSkillClick()");
        CancelSkill = true;
    }
    #endregion
}
