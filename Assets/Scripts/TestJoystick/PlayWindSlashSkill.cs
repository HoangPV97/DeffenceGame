﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayWindSlashSkill : Skill
{
    public GameObject arrow;
    //public VariableJoystick variableJoystick;
    private Vector2 direction;
    private float angle;
    ObjectPoolManager poolManager;
    public string bulletName, EffectName;
    float Speed, KnockBack, EffectedAoe;
    [SerializeField]
    SkillWeaponWind1 sww1;
    /// <summary>
    /// get data sww1.ManaCost[Level-1]
    /// </summary>
    int Level;
    protected override void Start()
    {
        arrow.transform.localScale *= (EffectedAoe / 10);
        poolManager = ObjectPoolManager.Instance;
        base.Start();
    }

    public override void SetUpData(int Tier = 1, int Level = 1, VariableJoystick variableJoystick = null)
    {
        var SkilldataSaver = DataController.Instance.GetGameDataWeapon(elemental).GetSkillTierLevel(SkillID);
        Tier = SkilldataSaver.Tier;
        Level = SkilldataSaver.Level;
        sww1 = JsonUtility.FromJson<SkillWeaponWind1>(ConectingFireBase.Instance.GetTextSkill(SkillID));
        this.variableJoystick = variableJoystick;
        manaCost = sww1.GetManaCost(Tier, Level);
        Speed = sww1.GetSkillSpeed(Tier, Level);
        Damage = sww1.GetDamage(Tier, Level);
        KnockBack = sww1.GetSkillAttributes("KnockbackDistance", Tier, Level);
        EffectedAoe = sww1.GetSkillAttributes("EffectedAoe", Tier, Level);
        CountdownTime = sww1.GetCoolDown(Tier, Level);
        variableJoystick.SetUpData(this);
        CountdownGo = variableJoystick.CountDountMask;
        base.SetUpData(Tier, Level);
    }

    public override void Update()
    {
        base.Update();

        if (variableJoystick.Vertical != 0 && TimeLeft <= 0 && Tower.Mana.CurrentMana >= manaCost)
        {
            direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - arrow.transform.position;
            angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            RotateArrow();
        }

        else
        {
            //  LowMana.SetActive(true);
            //float time = ((manaNumber - player.CurrentMana) / player.recoverMana) * player.recoverTime;
            //StartCoroutine(WaitingActiveObject(LowMana, time, false));
        }
    }
    public void RotateArrow()
    {
        arrow.SetActive(true);
        arrow.transform.eulerAngles = new Vector3(0, 0, angle - 90.0f);
    }
    private void Play()
    {
        SlowSkill(direction, angle - 90f);
        CountdownGo?.gameObject.SetActive(true);
        StartCountdown = true;
        TimeLeft = CountdownTime;
        Tower.ReduceMana(manaCost);
        arrow.SetActive(false);
    }

    public override void OnInvokeSkill()
    {
        base.OnInvokeSkill();
        arrow.SetActive(false);
        if (Tower.Mana.CurrentMana >= manaCost && TimeLeft <= 0)
        {
            Play();
        }
    }
    public override void OnCancelSkill()
    {
        arrow.SetActive(false);
    }

    public void SlowSkill(Vector2 _direction, float _rotatioZ)
    {
        GameObject skill_1_player = ObjectPoolManager.Instance.SpawnObject(SkillID, gameObject.transform.position, Quaternion.identity);
        GameObject effectStart = SpawnEffect(EffectName, GameplayController.Instance.PlayerController.transform.position, 0.7f);
        skill_1_player.transform.rotation = Quaternion.Euler(0, 0, _rotatioZ);
        WindSlashSkill WindSlashSkill = skill_1_player.GetComponent<WindSlashSkill>();
        WindSlashSkill.SetDataBullet(Speed, Mathf.RoundToInt(Damage),DamagePlus);
        WindSlashSkill.setDirection(direction);
        WindSlashSkill.setDataSkill(KnockBack, EffectedAoe);
        //Rigidbody2D rigidbody = skill_1_player.GetComponent<Rigidbody2D>();
        //rigidbody.velocity = _direction.normalized * 40 * Speed * Time.deltaTime;
    }

}
