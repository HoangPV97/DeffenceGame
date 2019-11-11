using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBullet : Bullet
{
    Transform Taget;
    private void Start()
    {
        base.Start();
    }
    private void Update()
    {
        base.Update();
    }
    private void OnTriggerEnter2D(Collider2D Target)
    {
        if (Target.gameObject.tag.Equals(TargetTag))
        {
            Enemy enemy = Target.GetComponent<Enemy>();
            enemy?.TakeDamage(elementalBullet,Damge,damagePlus);
            if (SeekTarget)
            {
                gameObject.SetActive(false);
            }
        }
    }
}

