using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcePlayerBullet : BulletController
{
    public void Start()
    {
        base.Start();
    }
    private void Update()
    {

        //base.Update();
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
            gameObject.SetActive(false);
        }
        if (Target.gameObject.tag.Equals(bullet.TargetTag))
        {
            EnemyController enemy = Target.GetComponent<EnemyController>();
            gameEffect.SpawnEffect("iceimpact", enemy.transform.position, 0.5f);
            IFireEffectable elemental = enemy.GetComponent<IFireEffectable>();
            if (elemental != null)
            {
                enemy.DealDamge( bullet.Damage, Mathf.Round(damagePlus * bullet.Damage / 100));
            }
            else
            {
                enemy.DealDamge(bullet.Damage, 0);
            }
            if (SeekTarget)
            {
                Despawn();
            }
        }
    }
}
