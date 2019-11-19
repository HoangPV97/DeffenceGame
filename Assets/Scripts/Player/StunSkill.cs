using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunSkill : BulletController
{
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
        if (Target.gameObject.tag.Equals(bullet.TargetTag))
        {
            EnemyController enemy = Target.GetComponent<EnemyController>();
            if (enemy != null)
            {
                IWindEffectable elemental = enemy.GetComponent<IWindEffectable>();
                if (elemental != null)
                {
                    elemental.WindImpactEffect(enemy.transform.position);
                    enemy?.DealDamge(bullet.Damage, damagePlus);
                }
                else
                {
                    enemy?.DealDamge(bullet.Damage, 0);
                }
                enemy.DealEffect(Effect.Stun,enemy.transform.position+new Vector3(0,0.5f,0),3);
            }
            if (SeekTarget)
            {
                
                gameObject.SetActive(false);
            }
        }
    }
    
}
