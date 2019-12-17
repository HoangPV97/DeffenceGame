using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindPlayerBullet : BulletController, IExplosionBullet
{
    EnemyController nearEnemy;
    public List<GameObject> EnemyinRange;
    CircleCollider2D circleCollider;
    public void explosionBulletEffect()
    {

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
            if (SeekTarget)
            {
                Despawn();
            }
            //if (explosion)
            //{
            //    ObjectPoolManager.Instance.SpawnObject("explosionBullet", this.transform.position, Quaternion.identity);
            //    Despawn();
            //}
            //if (bounce)
            //{
            //    if (GameplayController.Instance.PlayerController.listEnemies.Count > 0)
            //    {
                    
            //        float shortdistance = Vector3.Distance(transform.position, GameplayController.Instance.PlayerController.listEnemies[0].transform.position);
            //        for (int i = 1; i < GameplayController.Instance.PlayerController.listEnemies.Count; i++)
            //        {
            //            float distance = Vector3.Distance(transform.position, GameplayController.Instance.PlayerController.listEnemies[i].transform.position);
            //            if (distance < 3f && distance <= shortdistance)
            //            {
            //                nearEnemy = GameplayController.Instance.PlayerController.listEnemies[i];
            //                dir = nearEnemy.transform.position - transform.position;
            //                Move(dir);
            //                //Vector3.MoveTowards(transform.position, nearEnemy.gameObject.transform.position, 0f);
            //            }
                        
            //          //  enemy.DealDamge(bullet.Damage, damagePlus);
            //        }
            //    }
            //    else
            //    {
            //        Despawn();
            //    }

            //}
        }
    }
    private IEnumerator BounceToAnother(List<EnemyController> lst)
    {
        EnemyController nearestEnemy;
        float shortdistance = Mathf.Infinity;
        nearestEnemy = lst[0];
        shortdistance = Vector3.Distance(transform.position, nearestEnemy.transform.position);
        yield return new WaitForSeconds(0.3f);
        for (int i = 0; i < lst.Count; i++)
        {
            float distance = Vector3.Distance(transform.position, lst[i].transform.position);
            if (distance < 3f)
            {
                nearestEnemy = lst[i];
            }
            EnemyController _enemy = lst[i].GetComponent<EnemyController>();
            Vector3.MoveTowards(lst[i].GetComponent<EnemyController>().transform.position, lst[i + 1].GetComponent<EnemyController>().transform.position, 0f);
            Target = _enemy;
            _enemy.DealDamge(bullet.Damage, damagePlus);
        }

    }
}


