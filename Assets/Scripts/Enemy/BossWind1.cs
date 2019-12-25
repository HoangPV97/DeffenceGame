﻿using System;
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
    float timeDelayAttack=1f;
    // Update is called once per frame
    protected override void Start()
    {
        InvokeRepeating("RandomPosition", 0, timeDelayAttack+3);
    }
    protected override void Update()
    {
        //if (isAttack && countdown <= 0)
        //{
        //    RandomPosition();
        //    countdown = 4f;
        //}
       // countdown -= Time.deltaTime;
        if (!isAttack && isMove && gameEffect.CurrentEffect == Effect.None)
        {
            Move(enemy.speed);
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
    public override void DealEffect(Effect _effect, Vector3 _position, float _time)
    {
        gameEffect.CurrentEffect = _effect;
        StartCoroutine(WaitingEffect(_time, () =>
        {
            if (_effect.Equals(Effect.Slow))
            {
                Debug.Log("Slow");
            }
            else if (_effect.Equals(Effect.Stun) || _effect.Equals(Effect.Freeze))
            {
                isMove = false;
                if (isAttack)
                {
                    isAttack = false;
                }
                Move(enemy.speed);

                effectObj = gameEffect.GetEffect(_effect, _position, _time);
            }
            else if (_effect.Equals(Effect.Poiton))
            {
                effectObj = gameEffect.GetEffect(_effect, _position, _time);
            }
        }, () =>
        {
            isMove = true;
            if (_effect.Equals(Effect.Freeze))
            {
                gameEffect.GetEffect(Effect.destroyFreeze, _position, _time);
                if (!isAttack)
                {
                    isAttack = true;
                }
            }
            if (!isAttack)
            {
                Move(enemy.speed);
            }
            gameEffect.CurrentEffect = Effect.None;
        }));
    }
    public override void CheckAttack()
    {
        if (enemy.health.CurrentHealth <= enemy.health.health / 2 && enemy.health.CurrentHealth > enemy.health.health / 4 && !frenetic_50)
        {
            float ChargeRatio = UnityEngine.Random.Range(0, 100);
            SpawnEnemy.Instance.SpawnEnemyBoss_Wind_1();
            frenetic_50 = true;
            if (ChargeRatio > 20 && frenetic_50)
            {
                timeDelayAttack = 2;
            }
        }
        else if (enemy.health.CurrentHealth <= enemy.health.health / 4 && !frenetic_25)
        {
            frenetic_25 = true;
            int HardMode = DataController.Instance.StageData.HardMode;
            var bd = DataController.Instance.BossDataBase_Wind.GetWaveEnemyBoss_Wind_1(HardMode);
            enemy.speed += bd.SpeedPlus;
            enemy.damage *= bd.DamagePlus;
            timeDelayAttack = bd.DelayAttack;

        }
    }
    private void RandomPosition()
    {
        CurrentState = EnemyState.Attack;
        if (gameObject.activeSelf)
        {
            StartCoroutine(IEMove());
        }
    }
    IEnumerator IEMove()
    {
        yield return new WaitForSeconds(enemy.rateOfFire/100);
        int newPosition = UnityEngine.Random.Range(0, pointList.Count);
        newpposition = pointList[newPosition];
        isAttack = false;
        isMove = true;
    }
    public override void Move(float _speed,float _percentSlow=100f)
    {
        isMove = true;
        CurrentState = EnemyState.Run;
        gameObject.transform.position = Vector3.MoveTowards(transform.position, newpposition, enemy.speed / 5 * Time.deltaTime);
        if (transform.position == newpposition)
        {
            CurrentState = EnemyState.Idle;
            isAttack = true;
            isMove = false;
            return;
        }
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
        gameEffect.SpawnEffect("windimpact", _position, 0.3f);
    }
}