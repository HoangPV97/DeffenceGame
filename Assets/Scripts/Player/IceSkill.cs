using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceSkill : Bullet
{
    public ParticleSystem particleSystem;
    [SerializeField]
    private float stunTime=3;
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
            if (enemy != null)
            {
                enemy.TakeDamage(elementalBullet, Damge, damagePlus);
                enemy.TakeEffect(Effect.Stun, 5);
            }
            if (SeekTarget)
            {
                
                gameObject.SetActive(false);
            }
        }
    }
    
}
