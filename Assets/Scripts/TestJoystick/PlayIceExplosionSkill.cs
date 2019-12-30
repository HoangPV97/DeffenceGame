using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayIceExplosionSkill : Skill
{
    public VariableJoystick variableJoystick;
    public GameObject circle;
    ObjectPoolManager poolManager;
    public string EffectName;
    [SerializeField]
    SkillWeaponIce1 Swi1;
    protected float TimeEffect, SlowdownPercent, EffectedAoe, Damage;
    // Start is called before the first frame update
    /// <summary>
    /// get data sww1.ManaCost[Level-1]
    /// </summary>
    int Level;
    void Start()
    {
        poolManager = ObjectPoolManager.Instance;
        base.Start();
    }
    public override void SetUpData(int Tier = 1, int Level = 1, VariableJoystick variableJoystick = null, Vector3 _position = default)
    {
        base.SetUpData(Tier, Level);
        this.Level = Level;
        Swi1 = JsonUtility.FromJson<SkillWeaponIce1>(ConectingFireBase.Instance.GetTextSkill(SkillID));
        this.variableJoystick = variableJoystick;
        manaCost = Swi1.GetManaCost(Tier, Level);
        TimeEffect = Swi1.GetSkillAttributes("TimeEffect", Tier, Level);
        EffectedAoe = Swi1.GetSkillAttributes("EffectedAoe", Tier, Level);
        SlowdownPercent = Swi1.GetSkillAttributes("SlowdownPercent", Tier, Level);
        Damage = Swi1.GetDamage(Tier, Level);
        CountdownTime = Swi1.GetCoolDown(Tier, Level);
        variableJoystick.SetUpData(this);
        CountdownGo = variableJoystick.CountDountMask;
    }
    // Update is called once per frame
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

    public virtual void PlaySkill(Vector3 _position)
    {
        GameObject Poison_Skill = SpawnEffect(SkillID, _position, TimeEffect);
        Poison_Skill.GetComponent<SlowSkill>().SetSkillData(TimeEffect, SlowdownPercent, Damage, EffectedAoe);
        float particleTime = Poison_Skill.GetComponentInChildren<ParticleSystem>().main.duration;
        SoundManager.Instance.PlayClipOneShot(SoundManager.Instance.Explosion);
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
