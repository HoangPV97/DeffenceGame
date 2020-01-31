using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skill : MonoBehaviour
{
    public string SkillID;
    protected Tower Tower { get { return GameplayController.Instance.Tower; } }
    public VariableJoystick variableJoystick;
    public Image CountdownGo;
    public float CountdownTime;
    public float manaCost;
    public float Damage;
    public float DamagePlus;
    public Elemental elemental;
    protected bool StartCountdown = false;
    protected float TimeLeft;
    protected Vector3 positonEffect;
    bool isLowedMana;
    //protected PlayerController playerController { get { return GameplayController.Instance.PlayerController; } }
    // Start is called before the first frame update
    protected virtual void Start()
    {
        variableJoystick.txtMana.text = manaCost.ToString();
        SetpositonEffect();
    }

    public virtual void SetUpData(int Tier = 1, int Level = 1, VariableJoystick variableJoystick = null)
    {
        switch (elemental)
        {
            case Elemental.None:
                break;
            case Elemental.Wind:
                Damage*= DataController.Instance.InGameBaseData.achi_AddedDmgSpellWind;
                break;
            case Elemental.Ice:
                Damage *= DataController.Instance.InGameBaseData.achi_AddedDmgSpellIce;
                break;
            case Elemental.Earth:
                Damage *= DataController.Instance.InGameBaseData.achi_AddedDmgSpellEarth;
                break;
            case Elemental.Fire:
                Damage *=DataController.Instance.InGameBaseData.achi_AddedDmgSpellFire;
                break;
        }
        CountdownTime *= DataController.Instance.InGameBaseData.ReduceCooldown;
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if (TimeLeft > 0 && StartCountdown == true && CountdownGo != null)
        {
            CountdownGo?.gameObject.SetActive(true);
            TimeLeft -= Time.deltaTime;
            CountdownGo.fillAmount = TimeLeft / CountdownTime;
        }
        else
        {
            StartCountdown = false;
            CountdownGo?.gameObject.SetActive(false);
        }
        if (Tower.Mana.CurrentMana < manaCost && !isLowedMana)
        {
            isLowedMana = true;
            variableJoystick.LowMana.gameObject.SetActive(true);
            float time = ((manaCost - Tower.Mana.CurrentMana) / Tower.Mana.RecoverManaValue) * Tower.Mana.RecoverManaTime;
            StartCoroutine(WaitingRecoverMana(variableJoystick.LowMana.gameObject, time, false));
            return;
        }
    }
    public IEnumerator WaitingRecoverMana(GameObject _gameObject, float _time, bool status)
    {
        variableJoystick.txtMana.color = Color.red;
        yield return new WaitForSeconds(_time);
        isLowedMana = false;
        _gameObject.SetActive(status);
        variableJoystick.txtMana.color = Color.white;
    }

    public virtual void OnInvokeSkill()
    {
        DataController.Instance.CheckDailyQuest(QUEST_TYPE.QUEST_3, 1);
        switch (elemental)
        {
            case Elemental.None:
                break;
            case Elemental.Wind:
                DataController.Instance.CheckAchievement(ACHIEVEMENT_TYPE.ACHIEVEMENT_3, 1);
                break;
            case Elemental.Ice:
                DataController.Instance.CheckAchievement(ACHIEVEMENT_TYPE.ACHIEVEMENT_5, 1);
                break;
            case Elemental.Earth:
                DataController.Instance.CheckAchievement(ACHIEVEMENT_TYPE.ACHIEVEMENT_4, 1);
                break;
            case Elemental.Fire:
                DataController.Instance.CheckAchievement(ACHIEVEMENT_TYPE.ACHIEVEMENT_6, 1);
                break;
        }
    }

    public virtual void OnCancelSkill()
    {

    }
    public void CheckDestroyEffect(GameObject Obj, float _time)
    {
        if (!Obj.GetComponent<DestroyEffect>())
        {
            Obj.AddComponent<DestroyEffect>()._time = _time;
        }
        else
        {
            Obj.GetComponent<DestroyEffect>().Start();
        }
    }
    public GameObject SpawnEffect(string _effectName, Vector3 _position, float _time)
    {
        GameObject effectObj;
        effectObj = ObjectPoolManager.Instance.SpawnObject(_effectName, _position, Quaternion.identity);
        var de = effectObj.GetComponent<DestroyEffect>();
        if (de != null)
            de.StartWaitingDestroyEffect(_time);
        else
            effectObj.AddComponent<DestroyEffect>()._time = _time;
        return effectObj;
    }
    public virtual void AddDatatAttribute(string _attribute, float _value)
    {

    }
    public GameObject GetAlliance( Elemental _elemental)
    {
        for (int i = 0; i < GameplayController.Instance.AllianceController.Count; i++)
        {
            if (GameplayController.Instance.AllianceController[i].elementalType == _elemental)
            {
                return GameplayController.Instance.AllianceController[i].gameObject;
            }
        }
        return null;
    }
    public void SetpositonEffect()
    {
        string[] strList = SkillID.Split('_');
        string ID = strList[0];
        if (ID.Equals("WEAPON"))
        {
            positonEffect = GameplayController.Instance.PlayerController.transform.position;
            DamagePlus = GameplayController.Instance.PlayerController.ATKplus;
        }
        else
        {
            var alliance = GetAlliance(elemental);
            positonEffect = alliance.transform.position;
            DamagePlus = alliance.GetComponent<AllianceController>().ATKplus;
        }
    }
}
