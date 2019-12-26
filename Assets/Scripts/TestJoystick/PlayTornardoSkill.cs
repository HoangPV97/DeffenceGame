using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayTornardoSkill : Skill
{
    public VariableJoystick variableJoystick;
    public GameObject circle;
    ObjectPoolManager poolManager;
    public string bulletName, EffectName;
    [SerializeField]
    SkillWeaponWind2 Sww2;
    float TimeEffect,EffectedAoe;
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
        base.SetUpData(Level);
        this.Level = Level;
        Sww2 = JsonUtility.FromJson<SkillWeaponWind2>(ConectingFireBase.Instance.GetTextSkill(SkillID));
        this.variableJoystick = variableJoystick;
        manaCost = Sww2.GetManaCost(Tier, Level);
        TimeEffect = Sww2.GetSkillAttributes("TimeEffect", Tier, Level);
        EffectedAoe = Sww2.GetSkillAttributes("EffectedAoe", Tier, Level);
        CountdownTime = Sww2.GetCoolDown(Tier, Level);
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
    public void Play()
    {
        TimeLeft = CountdownTime;
        StartCountdown = true;
        CountdownGo?.gameObject.SetActive(true);
        Tower.Mana.ConsumeMana(manaCost);
        StunSkill(circle.transform.position);
        circle.SetActive(false);
    }

    public void StunSkill(Vector3 _position)
    {
        GameObject Poison_Skill = SpawnEffect(SkillID, _position, TimeEffect);
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
