using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMeteorSkill : DragAndDropSkill
{

    [SerializeField] private float MeteorNumber;
    [SerializeField] private SkillAllianceEarth1 Sae1;
    [SerializeField] private List<Vector3> positionList_9 = new List<Vector3> {new Vector2(-3,3),new Vector2(0,3), new Vector2(3,3),
                                                                             new Vector2(-3,0),new Vector2(0,0), new Vector2(3,0),
                                                                             new Vector2(-3,-3),new Vector2(0,-3), new Vector2(3,-3),};
    [SerializeField]
    private List<Vector3> positionList_6= new List<Vector3> {   new Vector2(-1.5f,2),new Vector2(1.5f,2),
                                                                new Vector2(-1,0),new Vector2(1,0),
                                                                new Vector2(-1.5f,-2), new Vector2(1.5f,-2),};
    [SerializeField]
    private List<Vector3> positionList_4 = new List<Vector3> {  new Vector2(-1,1),new Vector2(1,1),
                                                                new Vector2(-1,-1),new Vector2(1,-1),};
    List<int> positionSaver = new List<int>();
    // Start is called before the first frame update
    protected override void Start()
    {
        circle.transform.localScale *= EffectedAoe ;
        base.Start();
    }
    public override void Update()
    {
        base.Update();
        if (TimeLeft <= 0 && Tower.Mana.CurrentMana >= manaCost && variableJoystick.Vertical != 0)
        {
            Vector3 MousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = MousePosition - circle.transform.position;
            MoveObject(circle, direction);
        }
    }
    public override void SetUpData(int Tier = 1, int Level = 1, VariableJoystick variableJoystick = null, Vector3 _position = default)
    {
        var SkilldataSaver = DataController.Instance.GetGameDataWeapon(elemental).GetSkillTierLevel(SkillID);
        Tier = SkilldataSaver.Tier;
        Level = SkilldataSaver.Level;
        base.SetUpData(Tier, Level);
        Sae1 = JsonUtility.FromJson<SkillAllianceEarth1>(ConectingFireBase.Instance.GetTextSkill(SkillID));
        this.variableJoystick = variableJoystick;
        manaCost = Sae1.GetManaCost(Tier, Level);
        CountdownTime = Sae1.GetCoolDown(Tier, Level);
        EffectTime = Sae1.GetSkillAttributes("TimeEffect", Tier, Level);
        Damage = Sae1.GetDamage(Tier, Level);
        EffectedAoe = Sae1.GetSkillAttributes("EffectedAoe", Tier, Level);
        MeteorNumber = Sae1.GetSkillAttributes("MeteorNumber", Tier, Level);
        variableJoystick.SetUpData(this);
        positonEffect = _position;
        CountdownGo = variableJoystick.CountDountMask;
    }
    public override void PlaySkill(Vector3 _position)
    {
        switch (MeteorNumber)
        {
            case 4:
                Spawn_Effect_At_Random_Position(positionList_4);
                break;
            case 6:
                Spawn_Effect_At_Random_Position(positionList_6);
                break;
            case 9:
                Spawn_Effect_At_Random_Position(positionList_9);
                break;
        }
        GameObject effectStart = SpawnEffect(EffectName, positonEffect, 1f);
    }
    IEnumerator IESpawnMeteor(float _meteorNumber,List<Vector3> _positionList)
    {
        
        int randomPos;
        do
        {
            randomPos = Random.Range(0, _positionList.Count);
        }
        while (positionSaver.Contains(randomPos));
        positionSaver.Add(randomPos);
        GameObject stunSkill = SpawnEffect(SkillID, _positionList[randomPos],1.1f);
        stunSkill.GetComponent<MeteorSkill>().SetSkillData(EffectTime, Damage, EffectedAoe);
        _meteorNumber--;
        yield return new WaitForSeconds(0.2f);
        if (_meteorNumber > 0)
        {
            StartCoroutine(IESpawnMeteor(_meteorNumber, _positionList));
        }
        else
        {
            positionSaver.Clear();
        }
    }
    public List<Vector3> AddVector(List<Vector3> _positiontList)
    {
        List<Vector3> newPositionList = new List<Vector3>();
        for (int i = 0; i < _positiontList.Count; i++)
        {
            _positiontList[i] += circle.transform.position;
        }
        return newPositionList;
    }
    public void Spawn_Effect_At_Random_Position(List<Vector3> _positiontList)
    {
        List<Vector3> newPositionList = new List<Vector3>();
        for (int i = 0; i < _positiontList.Count; i++)
        {
            newPositionList.Add( _positiontList[i] + circle.transform.position);
        }
        StartCoroutine(IESpawnMeteor(MeteorNumber,newPositionList));
    }

}
