using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InviGiant.Tools;
public class GameplayController : Singleton<GameplayController>
{
    public PlayerController PlayerController;
    public AllianceController Alliance_1, Alliance_2;
    public Tower Tower;
    public GameObject Slot1, Slot2, Hero;
    [Header("Skill button")]
    public bool CancelSkill = false;
    public List<VariableJoystick> SkillButtons = new List<VariableJoystick>();
    public List<Transform> spawnPosition;
    public float GoldEachEnemy;
    public float TotalGoldDrop;
    public List<Skill> SkillList = new List<Skill>();
    private void Start()
    {
        LoadDataGamePlay();
    }
    public void LoadDataGamePlay()
    {
        Application.targetFrameRate = 60;
        ///SetUp Base first
        Tower.SetUpData();
        PlayerController = ObjectPoolManager.Instance.SpawnObject(Resources.Load<GameObject>("Prefabs/" + DataController.Instance.CurrentSelectedWeapon.ToString() + "Hero"), Hero.transform.position, Quaternion.identity).GetComponent<PlayerController>();
        /// Load Weapon
        PlayerController.SetDataWeapon();
        /// Check slot 
        if (DataController.Instance.inGameWeapons.Tier >= 1)
        {
            //set 1 skill button 
            // spawn skill controller
            var SplashSkill = Instantiate(DataController.Instance.DefaultData.GetWeaponSkill(DataController.Instance.inGameWeapons.Type, 0), this.transform);
            SplashSkill.GetComponent<Skill>().SetUpData(1, 1, SkillButtons[0], PlayerController.transform.position);
        }
        if (DataController.Instance.inGameWeapons.Tier >= 2)
        {
            // set 2 skill button
            //var go = Instantiate(DataController.Instance.DefaultData.GetWeaponSkill(DataController.Instance.inGameWeapons.Type, 0), this.transform);
            //go.GetComponent<Skill>().SetUpData(1, 1, SkillButtons[0], PlayerController.transform.position);
            var TornardoSkill = Instantiate(DataController.Instance.DefaultData.GetWeaponSkill(DataController.Instance.inGameWeapons.Type, 1), this.transform);
            var skill = TornardoSkill.GetComponent<Skill>();
            skill.SetUpData(1, 1, SkillButtons[1], PlayerController.transform.position);
            SkillList.Add(skill);
        }
        if (DataController.Instance.inGameWeapons.Tier >= 3)
        {
            var AgileSkill = Instantiate(DataController.Instance.DefaultData.GetWeaponSkill(DataController.Instance.inGameWeapons.Type, 2), this.transform);
            AgileSkill.GetComponent<Skill>().SetUpData(1, 1);
        }
        if (DataController.Instance.inGameWeapons.Tier >= 4)
        {
            var SoulOfWindSkill = Instantiate(DataController.Instance.DefaultData.GetWeaponSkill(DataController.Instance.inGameWeapons.Type,3), this.transform);
            SoulOfWindSkill.GetComponent<Skill>().SetUpData(1, 1);
        }

        /// Load Alliance
        /// Load from resource
        ///Load slot 1
        if (DataController.Instance.ElementalSlot1 != Elemental.None)
        {
            Alliance_1 = ObjectPoolManager.Instance.SpawnObject(Resources.Load<GameObject>("Prefabs/" + DataController.Instance.ElementalSlot1.ToString() + "Alliance"), Slot1.transform.position, Quaternion.identity).GetComponent<AllianceController>();
            Alliance_1.SetDataWeapon(DataController.Instance.IngameAlliance1.Type,
                                    DataController.Instance.IngameAlliance1.ATKspeed,
                                    DataController.Instance.IngameAlliance1.ATK,
                                     DataController.Instance.IngameAlliance1.BulletSpeed);
            // set  skill button
            var go = Instantiate(DataController.Instance.DefaultData.GetAllianceSkill(DataController.Instance.IngameAlliance1.Type), this.transform);
            go.GetComponent<Skill>().SetUpData(1, 1, SkillButtons[2],Alliance_1.transform.position);
        }

        ///Load slot 2
        if (DataController.Instance.ElementalSlot2 != Elemental.None)
        {
            Alliance_2 = ObjectPoolManager.Instance.SpawnObject(Resources.Load<GameObject>("Prefabs/" + DataController.Instance.ElementalSlot2.ToString() + "Alliance"), Slot2.transform.position, Quaternion.identity).GetComponent<AllianceController>();
            //load
            Alliance_2.SetDataWeapon(DataController.Instance.IngameAlliance2.Type,
                                    DataController.Instance.IngameAlliance2.ATKspeed,
                                    DataController.Instance.IngameAlliance2.ATK,
                                     DataController.Instance.IngameAlliance2.BulletSpeed);
            // set  skill button
            var go = Instantiate(DataController.Instance.DefaultData.GetAllianceSkill(DataController.Instance.IngameAlliance2.Type), this.transform);
            go.GetComponent<Skill>().SetUpData(1, 1, SkillButtons[3],Alliance_2.transform.position);
        }

        //spawn Enemy
        var sd = DataController.Instance.StageData;
        for (int i = 0; i < sd.stageEnemyDataBase.stageEnemies.Count; i++)
        {
            StartCoroutine(IESpawnEnemy(i, sd.stageEnemyDataBase.stageEnemies[i].StartTime));
            GameController.Instance.EnemyLive += sd.stageEnemyDataBase.stageEnemies[i].Number;
        }
        GoldEachEnemy = (float)(DataController.Instance.GoldInGame / GameController.Instance.EnemyLive);
    }
    #region Monster
    public IEnumerator IESpawnEnemy(int i, float timeDelay)
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
    public IEnumerator IESpawnEnemyBoss(StageEnemyDataBase stageEnemyDataBase, int i, float timeDelay)
    {
        yield return new WaitForSeconds(timeDelay);
        var se = stageEnemyDataBase.stageEnemies[i];
        se.Number--;
        int level = se.Level;
        if (DataController.Instance.StageData.HardMode == 2)
            level += stageEnemyDataBase.NightMareAddLevel;
        else if (DataController.Instance.StageData.HardMode == 3)
            level += stageEnemyDataBase.HellAddLevel;
        GameObject m_Enemy = ObjectPoolManager.Instance.SpawnObject(se.Type, se.Position == 999 ?
            spawnPosition[UnityEngine.Random.Range(0, 8)].position :
            spawnPosition[se.Position].position, transform.rotation);
        m_Enemy.GetComponent<EnemyController>().SetUpdata(se.Type, level);
        if (se.Number > 0)
        {
            StartCoroutine(IESpawnEnemyBoss(stageEnemyDataBase, i, se.RepeatTime));
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
    public Skill GetSkill (string SkillID)
    {
        for(int i=0;i< SkillList.Count; i++)
        {
            if (SkillList[i].SkillID.Equals(SkillID))
            {
                return SkillList[i];
            }
        }
        return null;
    }
}
