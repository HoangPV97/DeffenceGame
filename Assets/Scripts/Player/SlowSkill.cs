using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowSkill : BulletController
{

    [SerializeField]
    float backSpace;
    // Start is called before the first frame update
    public void SetDataSkill(float _speed,float _damage,float _knockback)
    {

    }
    private void OnTriggerEnter2D(Collider2D _Target)
    {
        if (_Target.gameObject.tag.Equals("BlockPoint"))
        {
            ObjectPoolManager.Instance.DespawnObJect(gameObject);
        }
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
                enemy.DealEffect(Effect.Slow, new Vector3(0,1f,0), 0);
                return;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        
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
