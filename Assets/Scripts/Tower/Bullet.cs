using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    protected Transform Target;
    public float Speed;
    public float Damge=20f;
    public string TargetTag;
    public bool SeekTarget=false;
    protected void Start()
    {
        Destroy(this.gameObject, 7.0f);
    }
    public void SetTarget(Transform _Target)
    {
        Target = _Target;
    }
    // Update is called once per frame
    protected void Update()
    {
        if (Target == null&&SeekTarget==true)
        {
            Destroy(gameObject);
            return;
        }
        if (SeekTarget)
        {
            Vector3 dir = Target.position - transform.position;

            transform.up = dir;
            transform.Translate(dir.normalized * Speed * Time.deltaTime, Space.World);
        }
        else
        {
            transform.position += Vector3.up * Speed * Time.deltaTime;
            
        }
        
    }
   
}
