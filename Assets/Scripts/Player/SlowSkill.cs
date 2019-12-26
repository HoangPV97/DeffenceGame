using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowSkill : BulletController
{
    List<GameObject> listCheckColision;
    float KnockBackDistance;
    float effectedAoe;
    [SerializeField] BoxCollider2D collider2D;
    // Start is called before the first frame update
    public void setDataSkill( float _knockback,float _effectAoe )
    {
        this.KnockBackDistance = _knockback;
        this.effectedAoe = _effectAoe;
        //if (effectedAoe != 0)
        //{
        //    collider2D.size = new Vector2(effectedAoe, 1);
        //}
    }
    public override void SetDataBullet(float _speed, float _damage)
    {
        
        listCheckColision = new List<GameObject>();
        base.SetDataBullet(_speed, _damage);
    }
    protected override void OnTriggerEnter2D(Collider2D _Target)
    {
        if (_Target.gameObject.tag.Equals("BlockPoint"))
        {
            Despawn();
        }
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
                    enemy.KnockBack( new Vector3(0, KnockBackDistance, 0));
                    return;
                }

            }
        }
    }

}
