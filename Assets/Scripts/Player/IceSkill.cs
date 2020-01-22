using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceSkill : BulletController
{
    public ParticleSystem ParticleSystem;
    public ParticleScaler ParticleScaler;
    public float EffectTime=3, EffectedAoe=10;
    // Start is called before the first frame update
    protected override void Start()
    {
        ParticleScaler.ScaleByTransform(ParticleSystem, EffectedAoe/10, true);
        elementalBullet = Elemental.Ice;   
    }


    protected override void OnTriggerEnter2D(Collider2D Target)
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
                    enemy?.DealDamge(bullet.Damage,Mathf.Round(bullet.ATKplus * bullet.Damage / 100));
                }
                else
                {
                    enemy?.DealDamge(bullet.Damage, 0);
                }
                enemy.DealEffect(Effect.Freeze, enemy.transform.position, EffectTime);
            }
            if (SeekTarget)
            {

                Despawn();
            }
        }
    }
}
