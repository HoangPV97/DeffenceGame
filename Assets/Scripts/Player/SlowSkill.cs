using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowSkill : TankBullet
{
    [SerializeField]
    float backSpace;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D _Target)
    {
        if (_Target.gameObject.tag.Equals(bullet.TargetTag))
        {
            EnemyController enemy = _Target.GetComponent<EnemyController>();
            if (enemy != null)
            {
                IIceEffectable elemental = enemy.GetComponent<IIceEffectable>();
                if (elemental != null)
                {
                    elemental.IceImpactEffect(enemy.transform.position);
                    enemy?.DealDamge(bullet.Damage, damagePlus);
                }
                else
                {
                    enemy?.DealDamge(bullet.Damage, 0);
                }
                enemy.DealEffect(Effect.Slow, new Vector3(0,0.5f,0), 0);
            }
            if (SeekTarget)
            {

                gameObject.SetActive(false);
            }
        }
    }
}
