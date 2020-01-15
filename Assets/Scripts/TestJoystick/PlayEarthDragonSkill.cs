using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayEarthDragonSkill : DragAndDropSkill
{
    protected float SlowdownPercent;
    [SerializeField] float EffectRow;
    [SerializeField] SkillWeaponEarth1 Swe1;
    private const int EffectSize = 2;
    protected override void Start()
    {
        circle.transform.localScale = new Vector3(2, EffectRow, 0);
        base.Start();
    }
    public override void Update()
    {
        if (TimeLeft <= 0 && Tower.Mana.CurrentMana >= manaCost && variableJoystick.Vertical != 0)
        {
            float MousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition).y;
            Vector3 Target = new Vector3(0, MousePosition, 0) - circle.transform.position;
            circle.SetActive(true);
            circle.transform.Translate(Target);
        }
        base.Update();
    }
    public override void SetUpData(int Tier = 1, int Level = 1, VariableJoystick variableJoystick = null, Vector3 _position = default)
    {
        var SkilldataSaver = DataController.Instance.GetGameDataWeapon(elemental).GetSkillTierLevel(SkillID);
        Tier = SkilldataSaver.Tier;
        Level = SkilldataSaver.Level;
        base.SetUpData(Level);
        Swe1 = JsonUtility.FromJson<SkillWeaponEarth1>(ConectingFireBase.Instance.GetTextSkill(SkillID));
        this.variableJoystick = variableJoystick;
        manaCost = Swe1.GetManaCost(Tier, Level);
        EffectTime = Swe1.GetSkillAttributes("TimeEffect", Tier, Level);
        EffectedAoe = Swe1.GetSkillAttributes("EffectedAoe", Tier, Level);
        SlowdownPercent = Swe1.GetSkillAttributes("SlowdownPercent", Tier, Level);
        Damage = Swe1.GetDamage(Tier, Level);
        CountdownTime = Swe1.GetCoolDown(Tier, Level);
        variableJoystick.SetUpData(this);
        positonEffect = _position;
        CountdownGo = variableJoystick.CountDountMask;
        EffectRow = Tier + 1;
    }
    public override void PlaySkill(Vector3 _position)
    {
        GameObject effectStart = SpawnEffect(EffectName, positonEffect, 1);
        if (EffectRow % 2 == 0)
        {
            SpawnAllEffect(EffectRow,_position, new Vector3(0, EffectSize / 2));
        }
        else
        {
            SpawnSkill(_position);
            SpawnAllEffect(EffectRow-1, _position, new Vector3(0, EffectSize));
        }
    }
    public void SpawnAllEffect(float EffectRow, Vector3 _position, Vector3 alpha)
    {
        int x = 0, tempt = 0;
        for (int i = 0; i < EffectRow; i++)
        {
            tempt += 1;
            SpawnParity(i, _position, alpha);
            if (tempt == 2)
            {
                x += EffectSize;
                alpha += new Vector3(0, x);
                tempt = 0;
            }
        }
    }
    void SpawnParity(int i, Vector3 _position, Vector3 _alpha)
    {
        if (i % 2 == 0)
        {
            SpawnSkill(_position + _alpha);
        }
        else
        {
            SpawnSkill(_position - _alpha);
        }
    }
    public void SpawnSkill(Vector3 _position)
    {
        GameObject Earth_Dragon_Skill = SpawnEffect(SkillID, _position, 0.5f);
        Earth_Dragon_Skill.GetComponent<SlowSkill>().SetSkillData(EffectTime, SlowdownPercent, Damage, EffectedAoe);
    }
}
