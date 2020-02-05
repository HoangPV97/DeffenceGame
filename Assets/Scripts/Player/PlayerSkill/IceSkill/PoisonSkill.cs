using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonSkill : Skill
{
    public int IncreaseDamage;
    [SerializeField]
    SkillAllianceWind3 swe3;
    // Start is called before the first frame update
    public override void SetUpData(int Tier = 1, int Level = 1, VariableJoystick variableJoystick = null)
    {
        var SkilldataSaver = DataController.Instance.GetGameAlliance(elemental).GetSkillTierLevel(SkillID);
        Tier = SkilldataSaver.Tier;
        Level = SkilldataSaver.Level;
        swe3 = JsonUtility.FromJson<SkillAllianceWind3>(ConectingFireBase.Instance.GetTextSkill(SkillID));
        IncreaseDamage = Mathf.RoundToInt(swe3.GetSkillAttributes("IncreaseDamage", SkilldataSaver.Tier, SkilldataSaver.Level));
        base.SetUpData(Tier, Level);
    }
    // Start is called before the first frame update
    protected override void Start()
    {
        GameplayController.Instance.PlayerController.Poison = true;
        GameplayController.Instance.PlayerController.PoisonDamage = IncreaseDamage;
    }
}
