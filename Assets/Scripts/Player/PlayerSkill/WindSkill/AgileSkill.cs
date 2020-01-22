using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgileSkill : Skill
{
    public int LevelSkill;
    public float FireRate;
    [SerializeField]
    SkillWeaponWind3 sww3;

    public override void SetUpData(int Tier = 1, int Level = 1, VariableJoystick variableJoystick = null)
    {
        var SkilldataSaver = DataController.Instance.GetGameDataWeapon(elemental).GetSkillTierLevel(SkillID);
        Tier = SkilldataSaver.Tier;
        Level = SkilldataSaver.Level;
        sww3 = JsonUtility.FromJson<SkillWeaponWind3>(ConectingFireBase.Instance.GetTextSkill(SkillID));
        FireRate = sww3.GetSkillAttributes("InscreaFireRate", Tier, LevelSkill);
        base.SetUpData(Tier, Level);
    }
    // Start is called before the first frame update
    protected override void Start()
    {
        GameplayController.Instance.PlayerController.SetFireRateWeaPon( FireRate);
    }
}
