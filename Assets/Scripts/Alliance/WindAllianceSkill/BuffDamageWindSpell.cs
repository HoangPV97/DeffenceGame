using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffDamageWindSpell : Skill
{
    private int BuffStats;
    [SerializeField] SkillAllianceWind3 SkillAllianceWind3;
    public override void SetUpData(int Tier = 1, int Level = 1, VariableJoystick variableJoystick = null, Vector3 _position = default)
    {
        var SkilldataSaver = DataController.Instance.GetGameAlliance(elemental).GetSkillTierLevel(SkillID);
        SkillAllianceWind3 = JsonUtility.FromJson<SkillAllianceWind3>(ConectingFireBase.Instance.GetTextSkill(SkillID));
        BuffStats = (int)SkillAllianceWind3.GetSkillAttributes("IncreaseDamage", SkilldataSaver.Tier, SkilldataSaver.Level);
        base.SetUpData(SkilldataSaver.Tier, SkilldataSaver.Level);
    }
    // Start is called before the first frame update
    protected override void Start()
    {
        List<Skill> WindSkills = GameplayController.Instance.GetElementSkills(Elemental.Wind);
        for (int i = 0; i < WindSkills.Count; i++)
        {
            WindSkills[i].Damage += (int)BuffStats * WindSkills[i].Damage / 100;
        }
    }

}
