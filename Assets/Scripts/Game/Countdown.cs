using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Countdown : MonoBehaviour
{
    private Image SkillTimer;
    public float CountdownTime;
    bool StartCountdown = false;
    float TimeLeft;
    // Start is called before the first frame update
    void Start()
    {
        TimeLeft = CountdownTime;
        SkillTimer = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (TimeLeft > 0 )
        //{
        //    TimeLeft -= Time.deltaTime;
        //    SkillTimer.fillAmount = TimeLeft / CountdownTime;

        //}
        //else
        //{
        //    TimeLeft = CountdownTime;   
        //    this.gameObject.SetActive(false);
        //}
    }
}
