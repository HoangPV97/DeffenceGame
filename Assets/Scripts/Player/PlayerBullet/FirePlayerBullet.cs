using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePlayerBullet : BulletController
{
    protected override void FixedUpdate()
    {
        if (GameplayController.Instance.PlayerController.currentMode == AutoMode.TurnOff)
            Move(dir);
        else
        {
            base.FixedUpdate();
        }
    }
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
           // gameEffect.SpawnEffect("fireimpact", enemy.transform.position, 0.5f);
            IWindEffectable elemental = enemy.GetComponent<IWindEffectable>();
            if (elemental != null)
            {
                enemy.DealDamge( bullet.Damage, Mathf.Round(damagePlus * bullet.Damage / 100));
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
