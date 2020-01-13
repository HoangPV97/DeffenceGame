﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgileSkill : Skill
{
    public int LevelSkill;
    public float FireRate;
    [SerializeField]
    SkillWeaponWind3 sww3;

    public override void SetUpData(int Tier = 1, int Level = 1, VariableJoystick variableJoystick = null, Vector3 _position = default)
    {
        this.LevelSkill = Level;
        base.SetUpData(Tier, Level);
        sww3 = JsonUtility.FromJson<SkillWeaponWind3>(ConectingFireBase.Instance.GetTextSkill(SkillID));
        FireRate = sww3.GetSkillAttributes("InscreaFireRate", Tier, LevelSkill);
        Debug.Log("FireRate :" + FireRate);
    }
    // Start is called before the first frame update
    protected override void Start()
    {
        GameplayController.Instance.PlayerController.SetFireRateWeaPon( FireRate);
    }
}
