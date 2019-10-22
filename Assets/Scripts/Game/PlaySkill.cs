using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlaySkill : MonoBehaviour
{
    public GameObject BulletOfSkill;
    public Transform Barrel;
    public GameObject Countdown;
    public float CountdownTime;
    bool StartCountdown = false;
    float TimeLeft;
    public float NummberBullet=10;
    bool StopInvoke = false;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (TimeLeft > 0&&StartCountdown==true)
        {
            TimeLeft -= Time.deltaTime;
            Countdown.GetComponent<Image>().fillAmount = TimeLeft / CountdownTime;

        }
        else
        {
            StartCountdown = false;
            Countdown.gameObject.SetActive(false);
            this.GetComponent<Image>().raycastTarget = true;
        }

    }
    public void Skill1()
    {
        Instantiate(BulletOfSkill, Barrel.position, Quaternion.identity);
        Countdown.SetActive(true);
        StartCountdown = true;
        TimeLeft = CountdownTime;
        this.GetComponent<Image>().raycastTarget = false;
    }
    public void Skill2()
    {
        StartCoroutine(PlaySkill2());
        Countdown.SetActive(true);
        StartCountdown = true;
        TimeLeft = CountdownTime;
        this.GetComponent<Image>().raycastTarget = false;
    }
    public IEnumerator  PlaySkill2()
    {
        for(int i = 0; i < 10; i++)
        {
            yield return new WaitForSeconds(0.3f);
            InstanceBullet();
        }
        
        //if (StopInvoke==false)
        //{
        //    CancelInvoke("InstanceObject");
        //}
        
    }
    public void InstanceBullet()
    {
        Instantiate(BulletOfSkill, Barrel.position, Quaternion.identity);
    }

}
