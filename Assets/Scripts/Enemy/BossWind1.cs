using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BossWindData : MonsterData
{

}
public class BossWind1 : EnemyController
{
    public List<Vector3> pointList;
    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
    public void Attack()
    {
        GameObject EnemyBullet = ObjectPoolManager.Instance.SpawnObject("windenemybullet", transform.position, Quaternion.identity);
        EnemyBullet m_EnemyBullet = EnemyBullet.GetComponent<EnemyBullet>();
        if (m_EnemyBullet != null)
        {
            m_EnemyBullet.SetTarget(Tower.transform);
            m_EnemyBullet.SetDamage(enemy.damage);
        }
    }
    public override void CheckAttack()
    {
        if (!isAttack)
        {
            distancetoTower = Mathf.Abs(transform.position.y - Tower.transform.position.y);
            if (distancetoTower < enemy.range && isLive)
            {
                isAttack = true;
                Rigidbody2D.velocity = Vector2.zero;
                CurrentState = EnemyState.Idle;
                CurrentState = EnemyState.Attack;
                RandomTranslate();
            }
            else
            {
                isAttack = false;
                isMove = true;
            }
        }
        if (enemy.health.CurrentHealth <= enemy.health.health / 2 && enemy.health.CurrentHealth > enemy.health.health / 4)
        {
            StartCoroutine(IEChargeAttack(2));
        }
        else if (enemy.health.CurrentHealth <= enemy.health.health / 4)
        {
            StartCoroutine(IEFrenetic(1));
        }
    }

    IEnumerator IEChargeAttack(float _time)
    {
        yield return new WaitForSeconds(_time);
        //Charge
    }
    public void RandomTranslate()
    {
        isAttack = false;
        isMove = true;
        int newPosition = UnityEngine.Random.Range(0, pointList.Count);
        Vector2 dir = transform.position - pointList[newPosition];
        //gameObject.transform.position = Vector3.MoveTowards();
    }
    IEnumerator IEFrenetic(float _time)
    {
        yield return new WaitForSeconds(_time);
    }
    public void WindImpactEffect(Vector3 _position)
    {
        GameObject effect = ObjectPoolManager.Instance.SpawnObject("windimpact", _position, Quaternion.identity);
        StartCoroutine(WaitingDestroyEffect(effect, 0.3f));
    }
}
