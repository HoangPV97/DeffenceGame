using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowSkill : BulletController
{

    [SerializeField]
    float backSpace;
    // Start is called before the first frame update
    public override void SetDataBullet(float _speed, float _damage)
    {       
        base.SetDataBullet(_speed, _damage);
    }
    protected override void OnTriggerEnter2D(Collider2D _Target)
    {
        base.OnTriggerEnter2D(_Target);
        Identify identify = _Target.gameObject.GetComponent<Identify>();
        if (identify == null)
        {
            identify = _Target.gameObject.AddComponent<Identify>();
        }
        if (_Target.gameObject.tag.Equals(bullet.TargetTag) && !identify.collided)
        {
            
            EnemyController enemy = _Target.GetComponent<EnemyController>();
            
            if (enemy != null )
            {
                _Target.gameObject.GetComponent<Identify>().collided = true;
                IIceEffectable elemental = enemy.GetComponent<IIceEffectable>();
                if (elemental != null)
                {
                    elemental.IceImpactEffect(enemy.transform.position);
                    enemy.DealDamge(bullet.Damage, Mathf.Round(damagePlus * bullet.Damage / 100));
                }
                else
                {
                    enemy.DealDamge(bullet.Damage, 0);
                }
                enemy.DealEffect(Effect.Knockback, new Vector3(0,1f,0), 0);
                return;
            }
        }
    }
    public class Identify : MonoBehaviour
    {
        public bool collided;
        private void Start()
        {
           // collided= false;
        }
    }
}
