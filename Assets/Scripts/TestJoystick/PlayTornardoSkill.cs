using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayTornardoSkill : DragAndDropSkill
{
    [SerializeField]
    SkillWeaponWind2 Sww2;
    float InflictedTime;
    protected override void Start()
    {
        circle.transform.localScale *= EffectedAoe *0.3f;
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
        Sww2 = JsonUtility.FromJson<SkillWeaponWind2>(ConectingFireBase.Instance.GetTextSkill(SkillID));
        this.variableJoystick = variableJoystick;
        manaCost = Sww2.GetManaCost(Tier, Level);
        EffectTime = Sww2.GetSkillAttributes("TimeEffect", Tier, Level);
        EffectedAoe = Sww2.GetSkillAttributes("EffectedAoe", Tier, Level);
        InflictedTime = Sww2.GetSkillAttributes("InflictedTime", Tier, Level);
        Damage = Sww2.GetDamage(Tier, Level);
        CountdownTime = Sww2.GetCoolDown(Tier, Level);
        variableJoystick.SetUpData(this);
        CountdownGo = variableJoystick.CountDountMask;
        base.SetUpData(Tier, Level);
    }

    public override void PlaySkill(Vector3 _position)
    {
        GameObject Poison_Skill = SpawnEffect(SkillID, _position, EffectTime);
        Poison_Skill.GetComponent<TornardoSkill>().SetTornardoData(EffectTime, InflictedTime, Damage, EffectedAoe);
        float particleTime = Poison_Skill.GetComponentInChildren<ParticleSystem>().main.duration;
        SoundManager.Instance.PlayClipOneShot(SoundManager.Instance.Explosion);
        GameObject effectStart = SpawnEffect(EffectName, positonEffect, particleTime);
    }
}
