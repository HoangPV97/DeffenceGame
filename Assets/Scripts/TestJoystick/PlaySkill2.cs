using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySkill2 : Skill
{
    public VariableJoystick variableJoystick;
    public GameObject Circle;
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected void Update()
    {
        base.Update();
        if (TimeLeft <= 0 && variableJoystick.Vertical != 0)
        {
            Circle.SetActive(true);
            Vector3 MousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 offset = MousePosition - Circle.transform.position;
            Circle.transform.Translate(offset);
        }
    }
    public void FixedUpdate()
    {
        
    }
    public void Play()
    {
        TimeLeft = CountdownTime;
        StartCountdown = true;
        CountdownGo?.gameObject.SetActive(true);
        Debug.Log("SHOOT!!!!!");
    }
    private void OnMouseUp()
    {
        Circle.SetActive(false);
        Play();
    }
}
