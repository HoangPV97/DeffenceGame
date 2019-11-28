using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceSkill : BulletController
{
    public ParticleSystem particleSystem;
    [SerializeField]
    private float stunTime=3;
    // Start is called before the first frame update
    void Start()
    {
        elementalBullet = Elemental.Ice;   
    }


    private void OnTriggerEnter2D(Collider2D Target)
    {
        if (Target.gameObject.tag.Equals(bullet.TargetTag))
        {
            EnemyController enemy = Target.GetComponent<EnemyController>();
            if (enemy != null)
            {
                IFireEffectable elemental = enemy.GetComponent<IFireEffectable>();
                if (elemental != null)
                {
                    elemental.FireImpactEffect(enemy.transform.position);
                    enemy?.DealDamge(bullet.Damage,Mathf.Round( damagePlus * bullet.Damage / 100));
                }
                else
                {
                    enemy?.DealDamge(bullet.Damage, 0);
                }
                enemy.DealEffect(Effect.Freeze, enemy.transform.position,3);
            }
            if (SeekTarget)
            {

                Despawn();
            }
        }
    }
}
