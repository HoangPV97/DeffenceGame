using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceBullet : BulletController
{
    GameObject nearestEnemy;
    public List<GameObject> EnemyinRange;
    protected override void Start()
    {
        EnemyinRange = new List<GameObject>();
        EnemyinRange.Clear();
    }
    protected override void Update()
    {
        //if (EnemyinRange.Count == 0 || !Target.isLive)
        //{
        //    Target = null;
        //    Despawn();
        //}

        if (EnemyinRange.Count > 1)
        {
            float shortdistance = Mathf.Infinity;
            nearestEnemy = EnemyinRange[0];
            shortdistance = Vector3.Distance(transform.position, nearestEnemy.transform.position);
            for (int i = 1; i < EnemyinRange.Count; i++)
            {
                float distance = Vector3.Distance(transform.position, EnemyinRange[i].transform.position);
                if (distance < shortdistance)
                {
                    nearestEnemy = EnemyinRange[i];
                }

                //Vector3.MoveTowards(transform.position, nearestEnemy.transform.position, 0f);
                StartCoroutine(IEmove(dir));
                // transform.Translate(dir.normalized * bullet.Speed * Time.deltaTime, Space.World);
            }
            //dir = nearestEnemy.transform.position - transform.position;

        }


    }
    IEnumerator IEmove(Vector2 _dir)
    {
        yield return new WaitForSeconds(0.5f);
        transform.Translate(dir.normalized * bullet.Speed * Time.deltaTime, Space.World);
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals(bullet.TargetTag) && !EnemyinRange.Contains(collision.gameObject)
            && collision.GetComponent<EnemyController>().isLive)
        {
            EnemyinRange.Add(collision.gameObject);
            StartCoroutine(DelayDespawn(0.5f));
        }
    }
    private void OnDisable()
    {
        EnemyinRange.Clear();
    }
}
