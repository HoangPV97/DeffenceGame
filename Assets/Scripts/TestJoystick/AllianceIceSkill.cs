﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllianceIceSkill : Skill
{
    public VariableJoystick variableJoystick;
    public GameObject circle;
    public string bulletName, EffectName;
    [SerializeField]
    SkillWeaponIce1 Swi1;
    // Start is called before the first frame update
    /// <summary>
    /// get data sww1.ManaCost[Level-1]
    /// </summary>
    int Level;
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
    }
    public override void SetUpData(int Tier = 1, int Level = 1, VariableJoystick variableJoystick = null)
    {
        base.SetUpData(Level);
        this.Level = Level;
        Swi1 = JsonUtility.FromJson<SkillWeaponIce1>(ConectingFireBase.Instance.GetTextWeaponSkill(SkillID));
        this.variableJoystick = variableJoystick;
        manaCost = Swi1.GetManaCost(Tier, Level);
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
            //   LowMana.SetActive(false);
        }
        else
        {
            // LowMana.SetActive(true);
        }

    }
    public void MoveObject(GameObject _object, Vector3 _postion)
    {
        //if (!_object.activeSelf)
        _object.SetActive(true);
        _object.transform.Translate(_postion);
    }

    public void Play()
    {
        TimeLeft = CountdownTime;
        StartCountdown = true;
        CountdownGo?.gameObject.SetActive(true);
        Tower.Mana.ConsumeMana(manaCost);
        IceSkill(circle.transform.position);
        circle.SetActive(false);
    }
    public override void OnInvokeSkill()
    {
        circle.SetActive(false);
        if (Tower.Mana.CurrentMana >= manaCost && TimeLeft <= 0)
        {
            Play();
        }
    }
    //private void OnMouseUp()
    //{
    //    circle.SetActive(false);
    //    if (Tower.Mana.CurrentMana >= manaCost)
    //    {
    //        Play();
    //    }
    //}
    public override void OnCancelSkill()
    {
        circle.SetActive(false);
    }
    public void IceSkill(Vector3 _position)
    {
        GameObject iceskill = ObjectPoolManager.Instance.SpawnObject(SkillID, _position, Quaternion.identity);
        float particleTime = iceskill.GetComponentInChildren<ParticleSystem>().main.duration;
        SoundManager.Instance.PlayClipOneShot(SoundManager.Instance.Explosion);
        GameObject effectStart = ObjectPoolManager.Instance.SpawnObject(EffectName, this.transform.position + new Vector3(0, 1, 0), Quaternion.identity);
        CheckDestroyEffect(effectStart, particleTime);
        CheckDestroyEffect(iceskill, particleTime);
    }
}
