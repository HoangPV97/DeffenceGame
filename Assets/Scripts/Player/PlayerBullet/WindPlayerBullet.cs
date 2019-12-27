using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindPlayerBullet : BulletController, IExplosionBullet
{

    public void explosionBulletEffect()
    {

    }
    protected override void Update()
    {
        if (Target == null || !Target.isLive)
        {
            Target = null;
            StartCoroutine(DelayDespawn(3));
        }
        if (dir == Vector3.zero)
            dir = new Vector3(0, 1, 0);
        Move(dir);

    }
    protected override void OnTriggerEnter2D(Collider2D _Target)
    {
        base.OnTriggerEnter2D(_Target);
        if (_Target.gameObject.tag.Equals(bullet.TargetTag) && !checkCollision)
        {
            checkCollision = true;
            EnemyController enemyController = _Target.GetComponent<EnemyController>();
            //if (enemyController.Equals(GameplayController.Instance.PlayerController.player.target))
            //{
            enemyController.gameEffect.SpawnEffect("windimpact", enemyController.transform.position, 0.5f);
            IIceEffectable elemental = enemyController?.GetComponent<IIceEffectable>();
            if (elemental != null)
            {
                elemental.IceImpactEffect(enemyController.transform.position);
                enemyController?.DealDamge(bullet.Damage, Mathf.Round(damagePlus * bullet.Damage / 100));
            }
            else
            {
                enemyController?.DealDamge(bullet.Damage, 0);
            }
            if (SeekTarget)
            {
                Despawn();
            }
        }
    }
}


