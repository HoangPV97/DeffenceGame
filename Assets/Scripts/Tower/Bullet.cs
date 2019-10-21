﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    protected Transform Target;
    public float Speed;
    public float Damge=20f;
    public bool SeekTarget=false;
    protected void Start()
    {

    }
    public void Seek(Transform _Target)
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
            float distanceThisFrame = Speed * Time.deltaTime;

            Quaternion lookRotation = Quaternion.LookRotation(dir);
            Vector3 rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * 10f).eulerAngles;
            transform.rotation = Quaternion.Euler(0f, 0f, -rotation.z);
            transform.Translate(dir.normalized * distanceThisFrame, Space.World);
        }
        else
        {
            transform.position += Vector3.up * Speed * Time.deltaTime;
        }
        
    }
    private void OnTriggerEnter2D(Collider2D Target)
    {
        if (Target.gameObject.tag.Equals("Enemy"))
        {
            Enemy enemy = Target.GetComponent<Enemy>();
            enemy?.TakeDamge(Damge);
            if (SeekTarget)
            {
                Destroy(gameObject);
            }
            else
            {
                Destroy(gameObject, 5f);
            }
            
        }
    }
}
