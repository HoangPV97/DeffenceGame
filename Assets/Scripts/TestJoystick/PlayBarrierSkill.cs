using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayBarrierSkill : DragAndDropSkill
{
    protected float  HealthRecover;
    [SerializeField] SkillWeaponEarth2 Swe2;
    // Start is called before the first frame update
    public override void SetUpData(int Tier = 1, int Level = 1, VariableJoystick variableJoystick = null, Vector3 _position = default)
    {
        base.SetUpData(Tier, Level);
        Swe2 = JsonUtility.FromJson<SkillWeaponEarth2>(ConectingFireBase.Instance.GetTextSkill(SkillID));
        this.variableJoystick = variableJoystick;
        manaCost = Swe2.GetManaCost(Tier, Level);
        EffectTime = Swe2.GetSkillAttributes("TimeEffect", Tier, 1);
        EffectedAoe = Swe2.GetSkillAttributes("EffectedAoe", Tier, 1);
        HealthRecover = Swe2.GetSkillAttributes("HealthRecover", Tier, Level);
        CountdownTime = Swe2.GetCoolDown(Tier, Level);
        variableJoystick.SetUpData(this);
        CountdownGo = variableJoystick.CountDountMask;
        positonEffect = _position;
    }
    // Update is called once per frame
    public override void PlaySkill(Vector3 _position)
    {
        GameObject Skill =SpawnEffect(SkillID, _position, EffectTime);
        Skill.GetComponent<BarrierSkill>().SetDataBarrierSkill(EffectTime, HealthRecover, EffectedAoe);
        SoundManager.Instance.PlayClipOneShot(SoundManager.Instance.Explosion);
        GameObject effectStart = SpawnEffect(EffectName, positonEffect, 1);
    }
}
