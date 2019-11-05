using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skill : MonoBehaviour
{
    public Player player;
    public GameObject directionGO;
    public GameObject BulletOfSkill;
    public Transform Barrel;
    public Image CountdownGo;
    public float CountdownTime;
    protected bool StartCountdown = false;
    protected float TimeLeft;
    public float NummberBullet = 10;
    BoxCollider2D boxCollider;
    // Start is called before the first frame update
    protected void Start()
    {
        boxCollider = this.GetComponent<BoxCollider2D>();
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
}
