using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffBlizzardSkill : Skill
{
    public float BuffStats;
    [SerializeField] SkillAllianceWind2 SkillAllianceWind2;
    public override void SetUpData(int Tier = 1, int Level = 1, VariableJoystick variableJoystick = null)
    {
        var SkilldataSaver = DataController.Instance.GetGameAlliance(elemental).GetSkillTierLevel(SkillID);
        Tier = SkilldataSaver.Tier;
        Level = SkilldataSaver.Level;
        SkillAllianceWind2 = JsonUtility.FromJson<SkillAllianceWind2>(ConectingFireBase.Instance.GetTextSkill(SkillID));
        BuffStats = SkillAllianceWind2.GetSkillAttributes("IncreaseTimeEffect", Tier, Level);
        base.SetUpData(Tier, Level);
    }
    // Start is called before the first frame update
    protected override void Start()
    {
        GameplayController.Instance.GetSkill("ALLIANCE_WIND_SKILL_1").AddDatatAttribute("TimeEffect", BuffStats);
    }
}
