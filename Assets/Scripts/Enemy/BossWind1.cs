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
        isAttack = false;
        BulletBoss = "BossBullet";
        timeDelayAttack = DataController.Instance.BossDataBase_Wind.GetWaveEnemyBoss_Wind_1(DataController.Instance.StageData.HardMode).DelayAttack;
        InvokeRepeating("RandomPosition", 0, timeDelayAttack+1);
    }
    protected override void Update()
    {
        //if ( countdown <= 0)
        //{
        //    RandomPosition();
        //    countdown = timeDelayAttack;
        //}
        //countdown -= Time.deltaTime;
        if (!isAttack && isMove && gameEffect.CurrentEffect == Effect.None)
        {
            Move(enemy.speed);
        }
        if (previousState != CurrentState)
        {
            ChangeState();
            previousState = CurrentState;
        }

        CheckAttack();
    }
    public override void DealEffect(Effect _effect, Vector3 _position, float _time)
    {
        if (!Coroutine_running || _effect != gameEffect.CurrentEffect)
        {
            StartCoroutine(WaitingEffect(_time, () =>
            {
                Coroutine_running = true;
                if (_effect.Equals(Effect.Slow))
                {
                    Debug.Log("Slow");
                }
                else if (_effect.Equals(Effect.Stun) || _effect.Equals(Effect.Freeze))
                {
                    skeletonAnimation.timeScale = 0;
                    isMove = false;
                    isAttack = false;
                    Rigidbody2D.velocity = Vector2.zero;
                    CurrentState = EnemyState.Idle; 
                    if (effectObj == null || _effect != gameEffect.CurrentEffect)
                    {
                        effectObj = gameEffect.GetEffect(_effect, _position+new Vector3(0,1,0), _time);
                        effectObj.transform.localScale = new Vector3(3, 1, 3);
                    }
                       
                }
                gameEffect.SetEffect(_effect);
            }, () =>
            {
                
                gameEffect.SetEffect(Effect.None);
                if (effectObj != null)
                {
                    Despawn(effectObj);
                    effectObj = null;
                }
                isMove = true;
                if (_effect.Equals(Effect.Freeze))
                {
                    gameEffect.GetEffect(Effect.destroyFreeze, _position, _time);
                }
                skeletonAnimation.timeScale = 1;
                Coroutine_running = false;
            }));
        }
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
            newPosition = transform.position;
            CurrentState = EnemyState.Idle;
            CurrentState = EnemyState.Skill;
            GameObject effectleftHand= Instantiate(boss_Fx, LeftHand.transform.position, Quaternion.identity);
            effectleftHand.transform.SetParent(LeftHand.transform);
            GameObject effectrightHand= Instantiate(boss_Fx, RightHand.transform.position, Quaternion.identity);
            effectrightHand.transform.SetParent(RightHand.transform);
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
        if (isAttack && !isMove)
        {
            CurrentState = EnemyState.Attack;
        }
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
