using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayEarthDragonSkill : Skill
{
    public VariableJoystick variableJoystick;
    public GameObject circle;
    ObjectPoolManager poolManager;
    public string EffectName;
    protected float TimeEffect, SlowdownPercent, EffectedAoe, Damage;
    [SerializeField] SkillWeaponEarth1 Swe1;
    // Start is called before the first frame update
    void Start()
    {

    }
    public override void SetUpData(int Tier = 1, int Level = 1, VariableJoystick variableJoystick = null, Vector3 _position = default)
    {
        base.SetUpData(Tier, Level);
        Swe1 = JsonUtility.FromJson<SkillWeaponEarth1>(ConectingFireBase.Instance.GetTextSkill(SkillID));
        this.variableJoystick = variableJoystick;
        manaCost = Swe1.GetManaCost(Tier, Level);
        TimeEffect = Swe1.GetSkillAttributes("TimeEffect", Tier, Level);
        EffectedAoe = Swe1.GetSkillAttributes("EffectedAoe", Tier, Level);
        SlowdownPercent = Swe1.GetSkillAttributes("SlowdownPercent", Tier, Level);
        Damage = Swe1.GetDamage(Tier, Level);
        CountdownTime = Swe1.GetCoolDown(Tier, Level);
        variableJoystick.SetUpData(this);
        CountdownGo = variableJoystick.CountDountMask;
    }
    protected void Update()
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
    public void MoveObject(GameObject _object, Vector3 _postion)
    {
        _object.SetActive(true);
        _object.transform.Translate(_postion);
    }
    public void FixedUpdate()
    {

    }
    public virtual void Play()
    {
        TimeLeft = CountdownTime;
        StartCountdown = true;
        CountdownGo?.gameObject.SetActive(true);
        Tower.Mana.ConsumeMana(manaCost);
        PlaySkill(circle.transform.position);
        circle.SetActive(false);
    }
    public void PlaySkill(Vector3 _position)
    {
        GameObject Earth_Dragon_Skill = SpawnEffect(SkillID, _position, TimeEffect);
        Earth_Dragon_Skill.GetComponent<SlowSkill>().SetSkillData(TimeEffect, SlowdownPercent, Damage, EffectedAoe);
        float particleTime = Earth_Dragon_Skill.GetComponentInChildren<ParticleSystem>().main.duration;
        GameObject effectStart = SpawnEffect(EffectName, positonEffect, particleTime);
    }
    public override void OnInvokeSkill()
    {
        circle.SetActive(false);
        if (Tower.Mana.CurrentMana >= manaCost && TimeLeft <= 0)
        {
            Play();
        }
    }
    public override void OnCancelSkill()
    {
        circle.SetActive(false);
    }
}
