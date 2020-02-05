using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayIceExplosionSkill : DragAndDropSkill
{
    [SerializeField]
    SkillWeaponIce1 Swi1;
    protected float SlowdownPercent;
    protected override void Start()
    {
        circle.transform.localScale *= EffectedAoe / 10;
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
    public override void SetUpData(int Tier = 1, int Level = 1, VariableJoystick variableJoystick = null)
    {
        var SkilldataSaver = DataController.Instance.GetGameDataWeapon(elemental).GetSkillTierLevel(SkillID);
        Tier = SkilldataSaver.Tier;
        Level = SkilldataSaver.Level;
        Swi1 = JsonUtility.FromJson<SkillWeaponIce1>(ConectingFireBase.Instance.GetTextSkill(SkillID));
        this.variableJoystick = variableJoystick;
        manaCost = Swi1.GetManaCost(Tier, Level);
        EffectTime = Swi1.GetSkillAttributes("TimeEffect", Tier, Level);
        EffectedAoe = Swi1.GetSkillAttributes("EffectedAoe", Tier, Level);
        SlowdownPercent = Swi1.GetSkillAttributes("SlowdownPercent", Tier, Level);
        Damage = Swi1.GetDamage(Tier, Level);
        CountdownTime = Swi1.GetCoolDown(Tier, Level);
        variableJoystick.SetUpData(this);
        CountdownGo = variableJoystick.CountDountMask;
        base.SetUpData(Tier, Level);
    }

    public override void PlaySkill(Vector3 _position)
    {
        GameObject Poison_Skill = SpawnEffect(SkillID, _position, 1);
        Poison_Skill.GetComponent<IceExplosionSkill>().SetSkillData(EffectTime, SlowdownPercent, Damage, EffectedAoe);
        GameObject effectStart = SpawnEffect(EffectName, positonEffect, 1);
    }
}
