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
    [SerializeField] private GameObject Barrel;
    public List<Vector3> pointList;
    bool frenetic_50, frenetic_25;
    Vector3 newpposition;

    // Update is called once per frame
    protected override void Start()
    {
    }
    protected override void Update()
    {
        if (!isAttack && countdown <= 0)
        {
            RandomPosition();
            countdown = 4f;
        }
        countdown -= Time.deltaTime;
        if (isAttack && isMove && gameEffect.CurrentEffect == Effect.None)
        {
            MoveRandom();
        }
        base.Update();
    }
    public void Attack()
    {
        GameObject EnemyBullet = ObjectPoolManager.Instance.SpawnObject("windenemybullet", Barrel.transform.position, Quaternion.identity);
        EnemyBullet m_EnemyBullet = EnemyBullet.GetComponent<EnemyBullet>();
        if (m_EnemyBullet != null)
        {
            m_EnemyBullet.SetTarget(Tower.transform);
            m_EnemyBullet.SetDamage(enemy.damage);
        }
    }
    public IEnumerator DelayAttack(float _time)
    {
        yield return new WaitForSeconds(_time);
        if (isLive && isAttack && gameEffect.CurrentEffect == Effect.None)
        {
            CurrentState = EnemyState.Attack;
        }
    }
    public override void CheckAttack()
    {
        distancetoTower = Mathf.Abs(transform.position.y - Tower.transform.position.y);
        if (distancetoTower < enemy.range && isLive)
        {
            isAttack = true;
            Rigidbody2D.velocity = Vector2.zero;
            CurrentState = EnemyState.Idle;
            CurrentState = EnemyState.Attack;
        }
        else
        {
            isAttack = false;
            isMove = true;
        }

        if (enemy.health.CurrentHealth <= enemy.health.health / 2 && enemy.health.CurrentHealth > enemy.health.health / 4 && !frenetic_50)
        {
            float ChargeRatio = UnityEngine.Random.Range(0, 100);
            if (ChargeRatio > 20 && frenetic_50)
            {
                frenetic_50 = true;
            }
        }
        else if (enemy.health.CurrentHealth <= enemy.health.health / 4 && !frenetic_25)
        {
            frenetic_25 = true;
            enemy.speed += 20;
            enemy.damage *= 3;
            enemy.rateOfFire -= 20;
        }
    }
    private void RandomPosition()
    {
        int newPosition = UnityEngine.Random.Range(0, pointList.Count);
        newpposition = pointList[newPosition];
    }
    private void MoveRandom()
    {
        gameObject.transform.position = Vector3.MoveTowards(transform.position, newpposition, enemy.speed / 5 * Time.deltaTime);
    }
    IEnumerator IEChargeAttack(float _time)
    {
        yield return new WaitForSeconds(_time);

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
