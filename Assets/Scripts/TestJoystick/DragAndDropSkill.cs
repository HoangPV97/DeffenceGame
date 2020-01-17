using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDropSkill : Skill
{
    //public VariableJoystick variableJoystick;
    public GameObject circle;
    protected ObjectPoolManager poolManager;
    public string EffectName;
    public float EffectTime, EffectedAoe;
    // Start is called before the first frame update
    /// <summary>
    /// get data sww1.ManaCost[Level-1]
    /// </summary>
    int Level;
    protected override void Start()
    {
        poolManager = ObjectPoolManager.Instance;
        base.Start();
    }

    // Update is called once per frame
    //public override void Update()
    //{
    //    base.Update();
    //    if (TimeLeft <= 0 && Tower.Mana.CurrentMana >= manaCost && variableJoystick.Vertical != 0)
    //    {
    //        Vector3 MousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //        Vector2 direction = MousePosition - circle.transform.position;
    //        MoveObject(circle, direction);
    //    }
    //    if (Tower.Mana.CurrentMana > manaCost)
    //    {
    //        //  LowMana.SetActive(false);
    //    }
    //    else
    //    {
    //        // LowMana.SetActive(true);
    //    }

    //}
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
        Tower.ReduceMana(manaCost);
        PlaySkill(circle.transform.position);
        circle.SetActive(false);
    }
    public virtual void PlaySkill(Vector3 _position)
    {
        GameObject Skill = ObjectPoolManager.Instance.SpawnObject(SkillID, _position, Quaternion.identity);
        float particleTime = Skill.GetComponentInChildren<ParticleSystem>().main.duration;
        SoundManager.Instance.PlayClipOneShot(SoundManager.Instance.Explosion);
        GameObject effectStart =SpawnEffect(EffectName, positonEffect, 1);
        CheckDestroyEffect(Skill, particleTime);
    }
    public override void OnInvokeSkill()
    {
        base.OnInvokeSkill();
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
