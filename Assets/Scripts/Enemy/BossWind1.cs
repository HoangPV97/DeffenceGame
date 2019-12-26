using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class BossWind1 : EnemyController
{
    [SerializeField] private GameObject Barrel;
    public List<Vector3> pointList;
    bool frenetic_50, frenetic_25;
    Vector3 newPosition;
    float timeDelayAttack = 1f;
    public GameObject LeftHand, RightHand;
    public GameObject boss_Fx;
    string BulletBoss;
    // Update is called once per frame
    protected override void Start()
    {
        BulletBoss = "BossBullet";
        timeDelayAttack = DataController.Instance.BossDataBase_Wind.GetWaveEnemyBoss_Wind_1(DataController.Instance.StageData.HardMode).DelayAttack;
        //InvokeRepeating("RandomPosition", 0, timeDelayAttack);
    }
    protected override void Update()
    {
        if ( countdown <= 0)
        {
            RandomPosition();
            countdown = timeDelayAttack;
        }
        countdown -= Time.deltaTime;
        if (!isAttack && isMove && gameEffect.CurrentEffect == Effect.None)
        {
            Move(enemy.speed);
        }
        base.Update();
    }
    public void Attack()
    {
        GameObject EnemyBullet = ObjectPoolManager.Instance.SpawnObject("BossBullet", Barrel.transform.position, Quaternion.identity);
        EnemyBullet m_EnemyBullet = EnemyBullet.GetComponent<EnemyBullet>();
        if (m_EnemyBullet != null)
        {
            m_EnemyBullet.SetTarget(Tower.transform);
            m_EnemyBullet.SetDamage(enemy.damage);
            m_EnemyBullet.SetSpeed(enemy.bulletSpeed);
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

            isMove = false;
            //newPosition = transform.position;
            //CurrentState = EnemyState.Idle;
            CurrentState = EnemyState.Skill;
            StartCoroutine(IEChargeAttack(1.5f));
            Instantiate(boss_Fx, LeftHand.transform.position, Quaternion.identity);
            Instantiate(boss_Fx, RightHand.transform.position, Quaternion.identity);
            frenetic_25 = true;
            int HardMode = DataController.Instance.StageData.HardMode;
            var bd = DataController.Instance.BossDataBase_Wind.GetWaveEnemyBoss_Wind_1(HardMode);
            enemy.speed += bd.SpeedPlus;
            enemy.damage *= bd.DamagePlus;
            timeDelayAttack = bd.DelayAttack - 1;
            BulletBoss = "BulletSkillBoss";
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
        yield return new WaitForSeconds(enemy.rateOfFire / 100);
        int index = UnityEngine.Random.Range(0, pointList.Count);
        newPosition = pointList[index];
        isAttack = false;
        isMove = true;
    }
    public override void Move(float _speed, float _percentSlow = 100f)
    {
        isMove = true;
        CurrentState = EnemyState.Run;
        gameObject.transform.position = Vector3.MoveTowards(transform.position, newPosition, enemy.speed / 3 * Time.deltaTime);
        if (transform.position == newPosition)
        {
            CurrentState = EnemyState.Idle;
            isAttack = true;
            //isMove = false;
            StartCoroutine(IEChargeAttack(timeDelayAttack));
            return;
        }
    }
    IEnumerator IEChargeAttack(float _time)
    {
        isMove = false;
        yield return new WaitForSeconds(_time);
        isMove = true;
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
