using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowSkill : BulletController
{
    List<GameObject> listCheckColision;
    [SerializeField]
    float backSpace;
    // Start is called before the first frame update
    public override void SetDataBullet(float _speed, float _damage)
    {
        listCheckColision = new List<GameObject>();
        base.SetDataBullet(_speed, _damage);
    }
    protected override void OnTriggerEnter2D(Collider2D _Target)
    {
        base.OnTriggerEnter2D(_Target);
        if (_Target.gameObject.tag.Equals(bullet.TargetTag))
        {
            if (!listCheckColision.Contains(_Target.gameObject))
            {
                EnemyController enemy = _Target.GetComponent<EnemyController>();
                listCheckColision.Add(_Target.gameObject);
                if (enemy != null)
                {

                    IIceEffectable elemental = enemy.GetComponent<IIceEffectable>();
                    if (elemental != null)
                    {
                        elemental.IceImpactEffect(enemy.transform.position);
                        enemy.DealDamge(bullet.Damage, Mathf.Round(damagePlus * bullet.Damage / 100));
                    }
                    else
                    {
                        enemy.DealDamge(bullet.Damage);
                    }
                    enemy.DealEffect(Effect.Knockback, new Vector3(0, 1f, 0), 0);
                    return;
                }

            }
        }
    }

}
