using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffDamageEarthSpell : Skill
{
    private int BuffStats;
    [SerializeField] SkillAllianceWind3 SkillAllianceWind3;
    public override void SetUpData(int Tier = 1, int Level = 1, VariableJoystick variableJoystick = null)
    {
        var SkilldataSaver = DataController.Instance.GetGameAlliance(elemental).GetSkillTierLevel(SkillID);
        Tier = SkilldataSaver.Tier;
        Level = SkilldataSaver.Level;
        SkillAllianceWind3 = JsonUtility.FromJson<SkillAllianceWind3>(ConectingFireBase.Instance.GetTextSkill(SkillID));
        BuffStats = (int)SkillAllianceWind3.GetSkillAttributes("IncreaseDamage", Tier, Level);
        base.SetUpData(Tier, Level);
    }
    // Start is called before the first frame update
    protected override void Start()
    {
        List<Skill> EarthSpells = GameplayController.Instance.GetElementSkills(Elemental.Earth);
        for (int i = 0; i < EarthSpells.Count; i++)
        {
            EarthSpells[i].Damage *= (1+ (int)BuffStats / 100);
        }
    }
}
