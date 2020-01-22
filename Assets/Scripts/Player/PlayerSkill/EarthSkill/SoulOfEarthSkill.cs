using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulOfEarthSkill : Skill
{
    [SerializeField] private float IncreaseFireRate;
    [SerializeField] private int IncreaseDamage;
    [SerializeField] private int IncreaseHealthRecover;
    [SerializeField] SkillWeaponEarth4 Swe4;
    public override void SetUpData(int Tier = 1, int Level = 1, VariableJoystick variableJoystick = null)
    {
        var SkilldataSaver = DataController.Instance.GetGameDataWeapon(elemental).GetSkillTierLevel(SkillID);
        Tier = SkilldataSaver.Tier;
        Level = SkilldataSaver.Level;
        Swe4 = JsonUtility.FromJson<SkillWeaponEarth4>(ConectingFireBase.Instance.GetTextSkill(SkillID));
        IncreaseDamage = (int)Swe4.GetSkillAttributes("IncreaseDamage", Tier, Level);
        IncreaseFireRate = Swe4.GetSkillAttributes("IncreaseFireRate", Tier, Level);
        IncreaseHealthRecover = (int)Swe4.GetSkillAttributes("IncreaseHealthRecover", Tier, Level);
        base.SetUpData(Tier, Level);
    }
    // Update is called once per frame
    protected override void Start()
    {
        GameplayController.Instance.PlayerController.SetDataWeaPon(IncreaseDamage, IncreaseFireRate);
        GameplayController.Instance.Tower.Health.IncreaseRecoverHealthValue(IncreaseHealthRecover);
    }
}
