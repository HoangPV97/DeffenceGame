﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayEarthDragonSkill : DragAndDropSkill
{
    protected float SlowdownPercent;
    float EffectRow;
    [SerializeField] SkillWeaponEarth1 Swe1;
    protected override void Start()
    {
        poolManager = ObjectPoolManager.Instance;
    }
    public override void Update()
    {
        if (TimeLeft <= 0 && Tower.Mana.CurrentMana >= manaCost && variableJoystick.Vertical != 0)
        {
            float MousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition).y;
            Vector3 Target = new Vector3(0, MousePosition,0)-circle.transform.position;
            circle.SetActive(true);
            circle.transform.Translate(Target);
            //circle.transform.position = Vector3.MoveTowards(transform.position, Target, 100 * Time.deltaTime);
        }
        if (Tower.Mana.CurrentMana > manaCost)
        {
            //  LowMana.SetActive(false);
        }
        else
        {
            // LowMana.SetActive(true);
        }
        base.Update();
    }
    public override void SetUpData(int Tier = 1, int Level = 1, VariableJoystick variableJoystick = null, Vector3 _position = default)
    {
        //var SkilldataSaver = DataController.Instance.GetGameAlliance(elemental).GetSkillTierLevel(SkillID);
        //Tier = SkilldataSaver.Tier;
        //Level = SkilldataSaver.Level;
        base.SetUpData(Level);
        Swe1 = JsonUtility.FromJson<SkillWeaponEarth1>(ConectingFireBase.Instance.GetTextSkill(SkillID));
        this.variableJoystick = variableJoystick;
        manaCost = Swe1.GetManaCost(Tier, Level);
        EffectTime = Swe1.GetSkillAttributes("TimeEffect", Tier, Level);
        EffectedAoe = Swe1.GetSkillAttributes("EffectedAoe", Tier, Level);
        SlowdownPercent = Swe1.GetSkillAttributes("SlowdownPercent", Tier, Level);
        Damage = Swe1.GetDamage(Tier, Level);
        CountdownTime = Swe1.GetCoolDown(Tier, Level);
        variableJoystick.SetUpData(this);
        positonEffect = _position;
        CountdownGo = variableJoystick.CountDountMask;
        EffectRow = Tier + 1;
    }
    public override void PlaySkill(Vector3 _position)
    {
        switch (EffectRow)
        {
            case 2:
                SpawnSkill(_position + new Vector3(0, 1, 0));
                SpawnSkill(_position + new Vector3(0, -1, 0));
                break;
            case 3:
                SpawnSkill(_position);
                SpawnSkill(_position + new Vector3(0, 1.5f, 0));
                SpawnSkill(_position + new Vector3(0, -1.5f, 0));
                break;
            case 4:
                SpawnSkill(_position + new Vector3(0, 1f, 0));
                SpawnSkill(_position + new Vector3(0, -1f, 0));
                SpawnSkill(_position + new Vector3(0, 2.5f, 0));
                SpawnSkill(_position + new Vector3(0, -2.5f, 0));
                break;
        }
    }
    public void SpawnSkill(Vector3 _position)
    {
        GameObject Earth_Dragon_Skill = SpawnEffect(SkillID, _position, 0.5f);
        Earth_Dragon_Skill.GetComponent<SlowSkill>().SetSkillData(EffectTime, SlowdownPercent, Damage, EffectedAoe);
        float particleTime = Earth_Dragon_Skill.GetComponentInChildren<ParticleSystem>().main.duration;
        GameObject effectStart = SpawnEffect(EffectName, positonEffect, particleTime);
    }
}
