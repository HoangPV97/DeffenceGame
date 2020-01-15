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
            var WeaponSkill_1 = Instantiate(DataController.Instance.DefaultData.GetWeaponSkill(DataController.Instance.inGameWeapons.Type, 0), this.transform).GetComponent<Skill>();
            SetUpSkill(WeaponSkill_1, DataController.Instance.inGameWeapons.Type,SkillButtons[0],PlayerController.transform.position);
        }
        if (DataController.Instance.inGameWeapons.Tier >= 2)
        {
            // set 2 skill button
            //var go = Instantiate(DataController.Instance.DefaultData.GetWeaponSkill(DataController.Instance.inGameWeapons.Type, 0), this.transform);
            //go.GetComponent<Skill>().SetUpData(1, 1, SkillButtons[0], PlayerController.transform.position);
            var WeaponSkill_2 = Instantiate(DataController.Instance.DefaultData.GetWeaponSkill(DataController.Instance.inGameWeapons.Type, 1), this.transform).GetComponent<Skill>();
            SetUpSkill(WeaponSkill_2, DataController.Instance.inGameWeapons.Type,SkillButtons[1],PlayerController.transform.position);
        }
        if (DataController.Instance.inGameWeapons.Tier >= 3)
        {
            var WeaponSkill_3 = Instantiate(DataController.Instance.DefaultData.GetWeaponSkill(DataController.Instance.inGameWeapons.Type, 2), this.transform).GetComponent<Skill>();
            SetUpSkill(WeaponSkill_3, DataController.Instance.inGameWeapons.Type);
        }
        if (DataController.Instance.inGameWeapons.Tier >= 4)
        {
            var SoulOfWeaponSkill = Instantiate(DataController.Instance.DefaultData.GetWeaponSkill(DataController.Instance.inGameWeapons.Type, 3), this.transform).GetComponent<Skill>();
            SetUpSkill(SoulOfWeaponSkill, DataController.Instance.inGameWeapons.Type);
        }

        /// Load Alliance
        /// Load from resource
        ///Load slot 1
        if (DataController.Instance.ElementalSlot1 != Elemental.None)
        {
            Alliance_1 = ObjectPoolManager.Instance.SpawnObject(Resources.Load<GameObject>("Prefabs/" + DataController.Instance.ElementalSlot1.ToString() + "Alliance"), Slot1.transform.position, Quaternion.identity).GetComponent<AllianceController>();
            var Alliance1 = DataController.Instance.IngameAlliance1;
            Alliance_1.SetDataWeapon(Alliance1.Type, Alliance1.ATKspeed,Alliance1.ATK,Alliance1.BulletSpeed, Alliance1.ATKRange);
            
            // set  skill button
            var go = Instantiate(DataController.Instance.DefaultData.GetAllianceSkill(DataController.Instance.IngameAlliance1.Type,0), this.transform).GetComponent<Skill>();
            SetUpSkill(go, DataController.Instance.IngameAlliance1.Type, SkillButtons[2], Alliance_1.transform.position);
            if (DataController.Instance.IngameAlliance1.Tier >= 2)
            {
                var Skill2 = Instantiate(DataController.Instance.DefaultData.GetAllianceSkill(DataController.Instance.IngameAlliance1.Type,1), this.transform).GetComponent<Skill>();
                SetUpSkill(Skill2, DataController.Instance.IngameAlliance1.Type);
            }
            if(DataController.Instance.IngameAlliance1.Tier >= 3)
            {
                var Skill3 = Instantiate(DataController.Instance.DefaultData.GetAllianceSkill(DataController.Instance.IngameAlliance1.Type,2), this.transform).GetComponent<Skill>();
                SetUpSkill(Skill3, DataController.Instance.IngameAlliance1.Type);
            }
        }

        ///Load slot 2
        if (DataController.Instance.ElementalSlot2 != Elemental.None)
        {
            Alliance_2 = ObjectPoolManager.Instance.SpawnObject(Resources.Load<GameObject>("Prefabs/" + DataController.Instance.ElementalSlot2.ToString() + "Alliance"), Slot2.transform.position, Quaternion.identity).GetComponent<AllianceController>();
            //load
            var Alliance2 = DataController.Instance.IngameAlliance2;
            Alliance_2.SetDataWeapon(Alliance2.Type, Alliance2.ATKspeed, Alliance2.ATK, Alliance2.BulletSpeed, Alliance2.ATKRange);
            // set  skill button
            var go = Instantiate(DataController.Instance.DefaultData.GetAllianceSkill(DataController.Instance.IngameAlliance2.Type,0), this.transform).GetComponent<Skill>();
            SetUpSkill(go, DataController.Instance.IngameAlliance2.Type, SkillButtons[3], Alliance_2.transform.position);
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
    public void SetUpSkill( Skill skill, Elemental _element, VariableJoystick variableJoystick=null, Vector3 position=default)
    {
        skill.elemental = _element;
        skill.SetUpData(1, 1, variableJoystick, position);
        SkillList.Add(skill);
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
    public Skill GetSkill(string SkillID)
    {
        for (int i = 0; i < SkillList.Count; i++)
        {
            if (SkillList[i].SkillID.Equals(SkillID))
            {
                return SkillList[i];
            }
        }
        return null;
    }
    public List<Skill> GetElementSkills(Elemental _elemental)
    {
        List<Skill> elementSkill = new List<Skill>();
        for(int i = 0; i < SkillList.Count; i++)
        {
            if (SkillList[i].elemental.Equals(_elemental))
            {
                elementSkill.Add(SkillList[i]);
            }
        }
        return elementSkill;
    }
}
