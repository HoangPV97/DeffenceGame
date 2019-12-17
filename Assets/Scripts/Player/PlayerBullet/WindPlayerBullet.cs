using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindPlayerBullet : BulletController, IExplosionBullet
{
    EnemyController nearEnemy;
    public List<GameObject> EnemyinRange;
    private float bounceRange = 3f;
    private int numberBounce = 3;
    private float timePoiton = 3f;
    private float damagePoiton = 3f;
    public void explosionBulletEffect()
    {

    }
    public override void SetDataBullet(float _speed, float _damage)
    {
        base.SetDataBullet(_speed, _damage);
        bounceRange = 5f;
        numberBounce = 3;
    }
    public void Update()
    {
        if (GameplayController.Instance.PlayerController.currentMode == AutoMode.TurnOff)
            Move(dir);
        else
        {
            base.Update();
        }
    }
    protected void OnTriggerEnter2D(Collider2D _Target)
    {
        if (_Target.gameObject.tag.Equals("BlockPoint"))
        {
            gameObject.SetActive(false);
        }
        if (_Target.gameObject.tag.Equals(bullet.TargetTag))
        {
            EnemyController enemy = _Target.GetComponent<EnemyController>();
            //enemy.CurrentState = EnemyState.Hurt;

            IIceEffectable elemental = enemy?.GetComponent<IIceEffectable>();
            if (elemental != null)
            {
                elemental.IceImpactEffect(enemy.transform.position);
                enemy?.DealDamge(bullet.Damage, Mathf.Round(damagePlus * bullet.Damage / 100));
            }
            else
            {
                enemy?.DealDamge(bullet.Damage, 0);
            }
            //if (SeekTarget)
            //{
            //    Despawn();
            //}
            #region Explosion Bullet
            //if (explosion)
            //{
            //    ObjectPoolManager.Instance.SpawnObject("explosionBullet", this.transform.position, Quaternion.identity);
            //    Despawn();
            //}
            #endregion
            #region BounceBullet
            if (bounce)
            {
                Debug.Log("Triger");
                if (GameplayController.Instance.PlayerController.listEnemies.Count > 0)
                {
                    nearEnemy = null;
                    int index = 0;
                    float shortdistance = Mathf.Infinity;
                    for (int i = 0; i < GameplayController.Instance.PlayerController.listEnemies.Count; i++)
                    {
                        float distance = Vector3.Distance(_Target.gameObject.transform.position, GameplayController.Instance.PlayerController.listEnemies[i].transform.position);
                        if (distance < shortdistance && GameplayController.Instance.PlayerController.listEnemies[i].gameObject != _Target.gameObject)
                        {
                            shortdistance = distance;
                            index = i;
                            Debug.Log("Exist NearEnemy");
                        }
                    }
                    if (shortdistance <= bounceRange)
                    {
                        nearEnemy = GameplayController.Instance.PlayerController.listEnemies[index];
                    }
                    if (numberBounce > 0 && nearEnemy != null && nearEnemy.isLive)
                    {
                        SetTarget(nearEnemy);
                        dir = nearEnemy.transform.position - transform.position;
                        Move(dir);
                        //nearEnemy.DealDamge(bullet.Damage, damagePlus);
                        bullet.Damage = bullet.Damage * 90 / 100;
                        numberBounce--;
                        if (numberBounce < 0)
                        {
                            Despawn();
                        }
                    }
                    else
                    {
                        Despawn();
                    }
                }
            }
            #endregion
            if (poison)
            {

            }
        }
    }
}


