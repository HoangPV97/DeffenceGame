using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindAllianceBullet : BulletController
{
    private void Start()
    {
        elementalBullet = Elemental.Wind;
        base.Start();
    }
    private void Update()
    {
        base.Update();
    }
    private void OnTriggerEnter2D(Collider2D Target)
    {
        if (Target.gameObject.tag.Equals(bullet.TargetTag))
        {
            EnemyController enemy = Target.GetComponent<EnemyController>();
            IIceEffectable elemental = enemy.GetComponent<IIceEffectable>();
            if (elemental != null)
            {
                elemental.IceImpactEffect(enemy.transform.position);
                enemy?.DealDamge(bullet.Damage, Mathf.Round(damagePlus * bullet.Damage / 100));
            }
            else
            {
                enemy?.DealDamge(bullet.Damage, 0);
            }
            Despawn();
        }
    }
}
