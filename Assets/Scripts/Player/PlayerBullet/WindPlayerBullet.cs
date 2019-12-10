using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindPlayerBullet : BulletController
{
    public void Start()
    {
        base.Start();
    }
    private void Update()
    {
        //if (Target == null || !Target.isLive)
        //{
        //    Despawn();
        //    return;
        //}
    }
    // Update is called once per frame
    public void DirectShooting(Vector2 _direction)
    {
        Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
        GetComponent<Rigidbody2D>().velocity = _direction.normalized * 50 * bullet.Speed * Time.deltaTime;
    }
    protected void OnTriggerEnter2D(Collider2D Target)
    {
        if (Target.gameObject.tag.Equals("BlockPoint"))
        {
            Despawn();
        }
        if (Target.gameObject.tag.Equals(bullet.TargetTag))
        {
            EnemyController enemy = Target.GetComponent<EnemyController>();
            gameEffect.SpawnEffect("windimpact", enemy.transform.position, 0.5f);
            //IIceEffectable elemental = enemy?.GetComponent<IIceEffectable>();
            IWindEffectable elemental = enemy?.GetComponent<IWindEffectable>();
            if (elemental!=null)
            {
                enemy.DealDamge(bullet.Damage, Mathf.Round(damagePlus * bullet.Damage / 100));
            }
            else
            {
                enemy.DealDamge( bullet.Damage, 0);
            }
            if (SeekTarget)
            {
                Despawn();
            }
        }
    }
}
