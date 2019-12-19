using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindPlayerBullet : BulletController, IExplosionBullet
{
    //EnemyController nearEnemy;
    //public List<GameObject> EnemyinRange;
    //private float bounceRange = 3f;
    //private int numberBounce = 3;
    //private float timePoiton = 3f;
    //private float damagePoiton = 3f;
    //private float percent_Slow = 20;
    public void explosionBulletEffect()
    {

    }
    public override void SetDataBullet(float _speed, float _damage)
    {
        base.SetDataBullet(_speed, _damage);
        //bounceRange = 5f;
        //numberBounce = 3;
    }
    protected override void Update()
    {
        if (GameplayController.Instance.PlayerController.currentMode == AutoMode.TurnOff)
            Move(dir);
        else
        {
            base.Update();
        }
    }
    protected override void OnTriggerEnter2D(Collider2D _Target)
    {
        base.OnTriggerEnter2D(_Target);
        if (_Target.gameObject.tag.Equals(bullet.TargetTag))
        {
            EnemyController enemyController = _Target.GetComponent<EnemyController>();
            //enemy.CurrentState = EnemyState.Hurt;
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
            #region Explosion Bullet
            //if (explosion)
            //{
            //    ObjectPoolManager.Instance.SpawnObject("explosionBullet", this.transform.position, Quaternion.identity);
            //    Despawn();
            //}
            #endregion
            //#region BounceBullet
            //if (bounce)
            //{
            //    if (GameplayController.Instance.PlayerController.listEnemies.Count > 0)
            //    {
            //        nearEnemy = null;
            //        int index = 0;
            //        float shortdistance = Mathf.Infinity;
            //        for (int i = 0; i < GameplayController.Instance.PlayerController.listEnemies.Count; i++)
            //        {
            //            float distance = Vector3.Distance(_Target.gameObject.transform.position, GameplayController.Instance.PlayerController.listEnemies[i].transform.position);
            //            if (distance < shortdistance && GameplayController.Instance.PlayerController.listEnemies[i].gameObject != _Target.gameObject)
            //            {
            //                shortdistance = distance;
            //                index = i;
            //            }
            //        }
            //        if (shortdistance <= bounceRange)
            //        {
            //            nearEnemy = GameplayController.Instance.PlayerController.listEnemies[index];
            //        }
            //        if (numberBounce > 0 && nearEnemy != null && nearEnemy.isLive)
            //        {
            //            SetTarget(nearEnemy);
            //            dir = nearEnemy.transform.position - transform.position;
            //            Move(dir);
            //            //nearEnemy.DealDamge(bullet.Damage, damagePlus);
            //            bullet.Damage = Mathf.Round( bullet.Damage * 90 / 100);
            //            numberBounce--;
            //            if (numberBounce < 0 || !nearEnemy.isLive)
            //            {
            //                Despawn();
            //            }
            //        }
            //        else
            //        {
            //            Despawn();
            //        }
            //    }
            //}
            //#endregion
            //if (slow)
            //{
            //    enemyController.DealEffect(Effect.Slow, enemyController.transform.position, 2f);
            //    enemyController.Move(enemyController.enemy.speed,percent_Slow);
            //}
        }
    }
}


