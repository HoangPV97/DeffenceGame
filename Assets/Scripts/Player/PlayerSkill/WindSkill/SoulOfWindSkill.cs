using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulOfWindSkill : Skill
{
    public float FireRate;
    public int Critical;
    [SerializeField] SkillWeaponWind4 Sww4;
    PlayerController playerController
    {
        get
        {
            return GameplayController.Instance.PlayerController;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public override void SetUpData(int Tier = 1, int Level = 1, VariableJoystick variableJoystick = null, Vector3 _position = default)
    {
        Sww4 = JsonUtility.FromJson<SkillWeaponWind4>(ConectingFireBase.Instance.GetTextSkill(SkillID));
        base.SetUpData(Tier, Level, variableJoystick, _position);
        Damage = (int)Sww4.GetSkillAttributes("IncreaseDamage", Tier, Level);
        FireRate = Sww4.GetSkillAttributes("IncreaseFireRate", Tier, Level);
        Critical = (int)Sww4.GetSkillAttributes("IncreaseCritical", Tier, Level);
    }
    // Update is called once per frame
    void Update()
    {
        playerController.SetDataWeaPon(Damage, FireRate, Critical);
    }
}
