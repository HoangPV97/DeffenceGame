using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulofIceSkill : Skill
{
    public float IncreaseDamage;
    public float IncreaseFireRate;
    public int IncreaseManaRecover;
    [SerializeField] SkillWeaponWind4 Sww4;
    PlayerController playerController
    {
        get
        {
            return GameplayController.Instance.PlayerController;
        }
    }
    public override void SetUpData(int Tier = 1, int Level = 1, VariableJoystick variableJoystick = null)
    {
        var SkilldataSaver = DataController.Instance.GetGameDataWeapon(elemental).GetSkillTierLevel(SkillID);
        Tier = SkilldataSaver.Tier;
        Level = SkilldataSaver.Level;
        Sww4 = JsonUtility.FromJson<SkillWeaponWind4>(ConectingFireBase.Instance.GetTextSkill(SkillID));
        IncreaseDamage = Sww4.GetSkillAttributes("IncreaseDamage", Tier, Level);
        IncreaseFireRate = Sww4.GetSkillAttributes("IncreaseFireRate", Tier, Level);
        IncreaseManaRecover = (int)Sww4.GetSkillAttributes("IncreaseManaRecover", Tier, Level);
        base.SetUpData(Tier, Level, variableJoystick);
    }
    // Update is called once per frame
    protected override void Start()
    {
        GameplayController.Instance.PlayerController.SetDataWeaPon((int)IncreaseDamage, IncreaseFireRate);
        GameplayController.Instance.Tower.Mana.IncreaseManaRecoverValue(IncreaseManaRecover);
    }
}
