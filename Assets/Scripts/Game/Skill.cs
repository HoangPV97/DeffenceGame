using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skill : MonoBehaviour
{
    public Tower Tower;
    public Image CountdownGo;
    public float CountdownTime;
    public float manaNumber;
    public GameObject LowMana;
    protected bool StartCountdown = false;
    protected float TimeLeft;
    public float NummberBullet = 10;
    Collider2D boxCollider;

    // Start is called before the first frame update
    protected void Start()
    {
        boxCollider = this.GetComponent<Collider2D>();
    }

    // Update is called once per frame
    protected void Update()
    {
        if (TimeLeft > 0 && StartCountdown == true && CountdownGo != null)
        {
            CountdownGo?.gameObject.SetActive(true);
            TimeLeft -= Time.deltaTime;
            CountdownGo.fillAmount = TimeLeft / CountdownTime;
            boxCollider.enabled = false;
        }
        else
        {
            StartCountdown = false;
            CountdownGo?.gameObject.SetActive(false);
            boxCollider.enabled = true;
        }
    }
    public IEnumerator WaitingActiveObject(GameObject _gameObject, float _time,bool status)
    {
        yield return new WaitForSeconds(_time);
        _gameObject.SetActive(status);
    }
}
