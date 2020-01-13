using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllianceIceSkill : DragAndDropSkill
{
    [SerializeField]
    SkillWeaponIce1 Swi1;
    protected override void Start()
    {
        circle.transform.localScale *= EffectedAoe / 10;
        base.Start();
    }
    public override void Update()
    {
        base.Update();
        if (TimeLeft <= 0 && Tower.Mana.CurrentMana >= manaCost && variableJoystick.Vertical != 0)
        {
            Vector3 MousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = MousePosition - circle.transform.position;
            MoveObject(circle, direction);
        }
        if (Tower.Mana.CurrentMana > manaCost)
        {
            //  LowMana.SetActive(false);
        }
        else
        {
            // LowMana.SetActive(true);
        }
    }
    public override void SetUpData(int Tier = 1, int Level = 1, VariableJoystick variableJoystick = null, Vector3 _position = default)
    {
        base.SetUpData(Level);
        Swi1 = JsonUtility.FromJson<SkillWeaponIce1>(ConectingFireBase.Instance.GetTextSkill(SkillID));
        this.variableJoystick = variableJoystick;
        manaCost = Swi1.GetManaCost(Tier, Level);
        Damage = Swi1.GetDamage(Tier, Level);
        EffectTime = Swi1.GetSkillAttributes("TimeEffect", Tier, Level);
        EffectedAoe = Swi1.GetSkillAttributes("EffectedAoe", Tier, Level);
        CountdownTime = Swi1.GetCoolDown(Tier, Level);
        variableJoystick.SetUpData(this);
        CountdownGo = variableJoystick.CountDountMask;
    }
    public override void PlaySkill (Vector3 _position)
    {
        GameObject iceskill = SpawnEffect(SkillID, _position, EffectTime);

        SoundManager.Instance.PlayClipOneShot(SoundManager.Instance.Explosion);
        GameObject effectStart = SpawnEffect(EffectName, this.transform.position + new Vector3(0, 1, 0), 1f);

    }
}
