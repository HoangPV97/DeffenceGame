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
    public  float TimeLeft;
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
            this.GetComponent<BoxCollider2D>().enabled = true;
        }

    }
    public void Skill1(Vector2 _direction, float _rotatioZ)
    {
        Player player = FindObjectOfType<Player>();
        player.ShootToDirection(_direction, _rotatioZ,BulletOfSkill);
        Countdown.SetActive(true);
        StartCountdown = true;
        TimeLeft = CountdownTime;
        this.GetComponent<BoxCollider2D>().enabled = false;
    }
    public void Skill2()
    {
        StartCoroutine(PlaySkill2());
        Countdown.SetActive(true);
        StartCountdown = true;
        TimeLeft = CountdownTime;
        this.GetComponent<BoxCollider2D>().enabled = false;
    }
    public IEnumerator  PlaySkill2()
    {
        for(int i = 0; i < 10; i++)
        {
            yield return new WaitForSeconds(0.3f);
            InstanceBullet();
        }
    }
    public void InstanceBullet()
    {
        if(BulletOfSkill!=null || Barrel!=null)
        Instantiate(BulletOfSkill, Barrel.position, Quaternion.identity);
    }

}
