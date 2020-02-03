using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayBlizzardSkill : DragAndDropSkill
{
    [SerializeField]
    SkillWeaponFire1 Swf1;
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
        var SkilldataSaver = DataController.Instance.GetGameAlliance(elemental).GetSkillTierLevel(SkillID);
        Tier = SkilldataSaver.Tier;
        Level = SkilldataSaver.Level;
        Swf1 = JsonUtility.FromJson<SkillWeaponFire1>(ConectingFireBase.Instance.GetTextSkill(SkillID));
        this.variableJoystick = variableJoystick;
        manaCost = Swf1.GetManaCost(Tier, Level);
        CountdownTime = Swf1.GetCoolDown(Tier, Level);
        EffectTime = Swf1.GetSkillAttributes("TimeEffect", Tier, Level);
        Damage = Swf1.GetDamage(Tier, Level);
        EffectedAoe = Swf1.GetSkillAttributes("EffectedAoe", Tier, Level);
        variableJoystick.SetUpData(this);
        CountdownGo = variableJoystick.CountDountMask;
        base.SetUpData(Tier, Level, variableJoystick);
    }
    public override void PlaySkill(Vector3 _position)
    {
        GameObject BlizzardSkill = ObjectPoolManager.Instance.SpawnObject(SkillID, _position, Quaternion.identity);
        BlizzardSkill.GetComponent<BlizzardSkill>().SetSkillData(EffectTime, Damage, EffectedAoe);
        GameObject effectStart = SpawnEffect(EffectName, positonEffect, 1f);
        CheckDestroyEffect(BlizzardSkill, 1);
    }
    public override void AddDatatAttribute(string _attribute, float _value)
    {
        if (_attribute == "TimeEffect")
        {
            if (_value > 5f) // Percent
                EffectTime += EffectTime * _value / 100;
            else //Value
                EffectTime += _value;
        }
    }
}
