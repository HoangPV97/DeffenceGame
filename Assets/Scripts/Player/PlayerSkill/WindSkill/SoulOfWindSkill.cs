﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulOfWindSkill : Skill
{
    public float IncreaseDamage;
    public float IncreaseFireRate;
    public int IncreaseCritical;
    [SerializeField] SkillWeaponWind4 Sww4;
    PlayerController playerController
    {
        get
        {
            return GameplayController.Instance.PlayerController;
        }
    }
    public override void SetUpData(int Tier = 1, int Level = 1, VariableJoystick variableJoystick = null, Vector3 _position = default)
    {
        var SkilldataSaver = DataController.Instance.GetGameDataWeapon(elemental).GetSkillTierLevel(SkillID);
        Tier = SkilldataSaver.Tier;
        Level = SkilldataSaver.Level;
        Sww4 = JsonUtility.FromJson<SkillWeaponWind4>(ConectingFireBase.Instance.GetTextSkill(SkillID));
        IncreaseDamage = Sww4.GetSkillAttributes("IncreaseDamage", Tier, Level);
        IncreaseFireRate = Sww4.GetSkillAttributes("IncreaseFireRate", Tier, Level);
        IncreaseCritical = (int)Sww4.GetSkillAttributes("IncreaseCritical", Tier, Level);
        base.SetUpData(Tier, Level, variableJoystick, _position);
    }
    // Update is called once per frame
    protected override void Start ()
    {
        playerController.SetDataWeaPon((int)IncreaseDamage, IncreaseFireRate);
        playerController.SetCriticalWeaPon(IncreaseCritical);
    }
}
