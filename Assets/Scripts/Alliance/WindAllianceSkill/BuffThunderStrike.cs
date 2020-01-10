using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffThunderStrike : Skill
{
    public float BuffStats;
    [SerializeField] SkillAllianceWind2 SkillAllianceWind2;
    public override void SetUpData(int Tier = 1, int Level = 1, VariableJoystick variableJoystick = null, Vector3 _position = default)
    {
        var SkilldataSaver = DataController.Instance.GetGameAlliance(Elemental.Wind).GetSkillTierLevel("ALLIANCE_WIND_SKILL_2");
        SkillAllianceWind2 = JsonUtility.FromJson<SkillAllianceWind2>(ConectingFireBase.Instance.GetTextSkill(SkillID));
        BuffStats = SkillAllianceWind2.GetSkillAttributes("IncreaseTimeEffect", SkilldataSaver.Tier, SkilldataSaver.Level);
        base.SetUpData(SkilldataSaver.Tier, SkilldataSaver.Level);
    }
    // Start is called before the first frame update
    protected override void Start()
    {
        GameplayController.Instance.GetSkill("ALLIANCE_WIND_SKILL_1").AddDatatAttribute("TimeEffect", BuffStats);
    }
}
