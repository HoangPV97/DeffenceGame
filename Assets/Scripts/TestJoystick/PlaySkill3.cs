using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySkill3 : DragAndDropSkill
{
    public float EffectTime, EffectedAoe,Damage;
    [SerializeField]
    SkillWeaponFire1 Swf1;

    public override void SetUpData(int Tier = 1, int Level = 1, VariableJoystick variableJoystick = null, Vector3 _position = default)
    {
        base.SetUpData( Level);
        Swf1 = JsonUtility.FromJson<SkillWeaponFire1>(ConectingFireBase.Instance.GetTextSkill(SkillID));
        this.variableJoystick = variableJoystick;
        manaCost = Swf1.GetManaCost(Tier, Level);
        CountdownTime = Swf1.GetCoolDown(Tier, Level);
        EffectTime = Swf1.GetSkillAttributes("TimeEffect", Tier, Level);
        Damage = Swf1.GetDamage(Tier, Level);
        EffectedAoe = Swf1.GetSkillAttributes("EffectedAoe", Tier, Level);
        variableJoystick.SetUpData(this);
        positonEffect = _position;
        CountdownGo = variableJoystick.CountDountMask;
    }
    public override void PlaySkill(Vector3 _position)
    {
        GameObject stunSkill = ObjectPoolManager.Instance.SpawnObject(SkillID, _position, Quaternion.identity);
        stunSkill.GetComponent<StunSkill>().SetSkillData(EffectTime, Damage, EffectedAoe);    
        float particleTime = stunSkill.GetComponentInChildren<ParticleSystem>().main.duration;
        GameObject effectStart = SpawnEffect(EffectName, positonEffect, 1f);
        CheckDestroyEffect(stunSkill, particleTime);
    }
}
