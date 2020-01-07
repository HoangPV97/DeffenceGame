using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffThunderStrike : Skill
{
    public float BuffStats;
    SkillAllianceWind2 SkillAllianceWind2;
    public override void SetUpData(int Tier = 1, int Level = 1, VariableJoystick variableJoystick = null, Vector3 _position = default)
    {
        SkillAllianceWind2 = JsonUtility.FromJson<SkillAllianceWind2>(ConectingFireBase.Instance.GetTextSkill(SkillID));
        BuffStats = SkillAllianceWind2.GetSkillAttributes("IncreaseTimeEffect", Tier, Level);
        base.SetUpData(Tier, Level);
    }
    // Start is called before the first frame update
    void Start()
    {
        GameplayController.Instance.GetSkill("ALLIANCE_WIND_SKILL_1").AddDatatAttribute("IncreaseTimeEffect", BuffStats);
    }
}
