using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySkill4 : Skill
{
    public VariableJoystick variableJoystick;
    public GameObject circle;
    ObjectPoolManager poolManager;
    public string bulletName, EffectName;
    [SerializeField]
    SkillWeaponFire1 Swf1; //Dùng tạm thời//
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
    public override void SetUpData(int Tier = 1, int Level = 1, VariableJoystick variableJoystick = null)
    {
        base.SetUpData(Level);
        this.Level = Level;
        Swf1 = JsonUtility.FromJson<SkillWeaponFire1>(ConectingFireBase.Instance.GetTextWeaponSkill(SkillID));
        this.variableJoystick = variableJoystick;
        manaCost = Swf1.GetManaCost(Tier, Level);
        CountdownTime = Swf1.GetCoolDown(Tier, Level);
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
        GameObject Poison_Skill = ObjectPoolManager.Instance.SpawnObject(SkillID, _position, Quaternion.identity);
        float particleTime = Poison_Skill.GetComponentInChildren<ParticleSystem>().main.duration;
        SoundManager.Instance.PlayClipOneShot(SoundManager.Instance.Explosion);
        GameObject effectStart = ObjectPoolManager.Instance.SpawnObject(EffectName, this.transform.position + new Vector3(0, 1, 0), Quaternion.identity);
        CheckDestroyEffect(effectStart, particleTime);
        CheckDestroyEffect(Poison_Skill, particleTime);
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
