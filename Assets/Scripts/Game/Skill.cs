using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skill : MonoBehaviour
{
    public string SkillID;
    public Tower Tower
    {
        get
        {
            return GameplayController.Instance.Tower;
        }
    }
    public VariableJoystick variableJoystick;
    public Image CountdownGo;
    public float CountdownTime;
    public float manaCost;
    protected bool StartCountdown = false;
    protected float TimeLeft;
    protected Vector3 positonEffect;
    bool isLowedMana;
    // Start is called before the first frame update
    protected virtual void Start()
    {
    }

    public virtual void SetUpData(int Tier = 1, int Level = 1, VariableJoystick variableJoystick = null,Vector3 _position = default )
    {
    }

    // Update is called once per frame
    protected virtual void Update()
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
            Debug.Log("Lowmana");
            isLowedMana = true;
            variableJoystick.LowMana.gameObject.SetActive(true);
            float time = ((manaCost - Tower.Mana.CurrentMana) / Tower.Mana.RecoverManaValue) * Tower.Mana.RecoverManaTime;
            StartCoroutine(WaitingRecoverMana(variableJoystick.LowMana.gameObject, time, false));
            return;
        }
    }
    public IEnumerator WaitingRecoverMana(GameObject _gameObject, float _time, bool status)
    {
        yield return new WaitForSeconds(_time);
        isLowedMana = false;
        _gameObject.SetActive(status);
    }

    public virtual void OnInvokeSkill()
    {
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
}
