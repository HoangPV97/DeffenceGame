using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayBarrierSkill : DragAndDropSkill
{
    protected float HealthRecover;
    [SerializeField] SkillWeaponEarth2 Swe2;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        circle.transform.localScale = new Vector3((EffectTime - 1) / 2, 1, 0); //time=3s ~ scale=1//time=4s ~ scale=1.5
    }
    public override void Update()
    {
        base.Update();
        if (TimeLeft <= 0 && Tower.Mana.CurrentMana >= manaCost && variableJoystick.Vertical != 0)
        {
            float MousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
            Vector3 Target = new Vector3(MousePosition, -4, 0) - circle.transform.position;
            circle.SetActive(true);
            circle.transform.Translate(Target);
        }
    }
    public override void SetUpData(int Tier = 1, int Level = 1, VariableJoystick variableJoystick = null)
    {
        var SkilldataSaver = DataController.Instance.GetGameDataWeapon(elemental).GetSkillTierLevel(SkillID);
        Tier = SkilldataSaver.Tier;
        Level = SkilldataSaver.Level;
        Swe2 = JsonUtility.FromJson<SkillWeaponEarth2>(ConectingFireBase.Instance.GetTextSkill(SkillID));
        this.variableJoystick = variableJoystick;
        manaCost = Swe2.GetManaCost(Tier, Level);
        EffectTime = Swe2.GetSkillAttributes("TimeEffect", Tier, 1);
        EffectedAoe = Swe2.GetSkillAttributes("EffectedAoe", Tier, 1);
        HealthRecover = Swe2.GetSkillAttributes("HealthRecover", Tier, Level);
        CountdownTime = Swe2.GetCoolDown(Tier, Level);
        variableJoystick.SetUpData(this);
        CountdownGo = variableJoystick.CountDountMask;
        base.SetUpData(Tier, Level);
    }
    // Update is called once per framef
    public override void PlaySkill(Vector3 _position)
    {
        GameObject Skill = SpawnEffect(SkillID, _position-new Vector3(0,1,0), EffectTime + 1.2f);
        BarrierSkill barrierSkill = Skill.GetComponent<BarrierSkill>();
        barrierSkill.SetDataBarrierSkill(EffectTime, HealthRecover, EffectedAoe);
        barrierSkill.InvokeSkill();
        positonEffect = GetAlliance(elemental).transform.position;
        GameObject effectStart = SpawnEffect(EffectName, positonEffect, 1);
    }
}
