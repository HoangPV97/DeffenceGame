using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    protected Transform Target;
    public float Speed;
    public float Damge;
    public string TargetTag;
    public bool SeekTarget=false;
    private Vector2 Direction;
    private float RotationZ;
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
        if (Target == null)
        {
            Destroy(gameObject);
            return;
        }
        Vector3 dir = Target.position - transform.position;
        transform.up = dir;
        transform.Translate(dir.normalized * Speed * Time.deltaTime, Space.World);
        
    }

}
