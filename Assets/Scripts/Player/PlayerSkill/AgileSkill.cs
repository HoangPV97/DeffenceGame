using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgileSkill : Skill
{
    public int LevelSkill;
    public float FireRate;
    [SerializeField]
    SkillWeaponWind3 sww3;
    PlayerController playerController
    {
        get
        {
            return GameplayController.Instance.PlayerController;
        }
    }
    public override void SetUpData(int Tier = 1, int Level = 1, VariableJoystick variableJoystick = null, Vector3 _position = default)
    {
        this.LevelSkill = Level;
        base.SetUpData(Tier, Level);
        sww3 = JsonUtility.FromJson<SkillWeaponWind3>(ConectingFireBase.Instance.GetTextSkill(SkillID));
        FireRate = sww3.GetSkillAttributes("InscreaFireRate", Tier, LevelSkill);
        Debug.Log("FireRate :" + FireRate);
    }
    // Start is called before the first frame update
    void Start()
    {
        playerController.SetDataWeaPon( FireRate);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
