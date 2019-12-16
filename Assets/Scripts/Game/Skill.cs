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
    public Image CountdownGo;
    public float CountdownTime;
    public float manaCost;
    protected bool StartCountdown = false;
    protected float TimeLeft;
    public float NummberBullet = 10;

    // Start is called before the first frame update
    protected void Start()
    {
    }

    public virtual void SetUpData(int Tier = 1, int Level = 1, VariableJoystick variableJoystick = null)
    {

    }

    // Update is called once per frame
    protected void Update()
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
    }
    public IEnumerator WaitingActiveObject(GameObject _gameObject, float _time, bool status)
    {
        yield return new WaitForSeconds(_time);
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
}
