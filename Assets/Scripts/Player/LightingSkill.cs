using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightingSkill : Bullet
{
    public ParticleSystem particleSystem;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D Target)
    {
        if (Target.gameObject.tag.Equals(TargetTag))
        {
            Enemy enemy = Target.GetComponent<Enemy>();
            //if (!particleSystem.isPlaying)
            //{
            //    gameObject.SetActive(false);
            //}
            enemy?.TakeDamage(elementalBullet,Damge,damagePlus);
            if (SeekTarget)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
