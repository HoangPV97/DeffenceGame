using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Spine;
using UnityEngine;
using UnityEngine.Events;

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
    public UnityEvent EventOnFx;
    bool isChargeAttack;
    // Update is called once per frame
    protected override void Start()
    {
        isAttack = false;
        BulletBoss = "BOSS_WIND_BULLET";
        timeDelayAttack = DataController.Instance.BossStageDataBase.GetWaveEnemyBoss(DataController.Instance.StageData.Level).DelayAttack;
        //InvokeRepeating("RandomPosition", 0, timeDelayAttack+1);\
        skeletonAnimation.AnimationState.Event += OnEventChargeAttack;
        base.Start();
    }
    private void OnEventChargeAttack(TrackEntry trackEntry, Spine.Event e)
    {
        if (e.Data.Name.Equals("onfx") && EventOnFx != null)
        {
            EventOnFx.Invoke();
        }
    }
    protected override void Update()
    {
        if (!isAttack && isMove && gameEffect.CurrentEffect == Effect.None && !isChargeAttack && isLive)
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
    public override void Deal_Slow_Effect(float _time, float _slowdown_percent)
    {
        
    }
    public override void DealEffect(Effect _effect, Vector3 _position, float _time)
    {
        if ((!Coroutine_running || _effect != gameEffect.CurrentEffect ) && _effect!=Effect.StunBullet)
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
                    Debug.Log("STUN");
                    skeletonAnimation.timeScale = 0;
                    isMove = false;
                    isAttack = false;
                    Rigidbody2D.velocity = Vector2.zero;
                    CurrentState = EnemyState.Idle;
                    if (effectObj == null || _effect != gameEffect.CurrentEffect)
                    {
                        effectObj = gameEffect.GetEffect(_effect, _position + new Vector3(0, 1, 0), _time);
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

    public void ChargeAttack()
    {
        GameObject effectleftHand = gameEffect.SpawnEffect(boss_Fx, LeftHand.transform.position, 1.3f);
        effectleftHand.transform.SetParent(LeftHand.transform);
        GameObject effectrightHand = gameEffect.SpawnEffect(boss_Fx, RightHand.transform.position, 1.3f);
        effectrightHand.transform.SetParent(RightHand.transform);
    }
    public void Attack()
    {
        GameObject EnemyBullet = ObjectPoolManager.Instance.SpawnObject(BulletBoss, Barrel.transform.position, Quaternion.identity);
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
            int LevelBoss = DataController.Instance.StageData.Level;
            var bd = DataController.Instance.BossStageDataBase.GetWaveEnemyBoss(LevelBoss);
            for (int i = 0; i < bd.stageEnemyDataBase.stageEnemies.Count; i++)
            {
                StartCoroutine(IESpawnEnemyBoss(bd.stageEnemyDataBase, i, bd.stageEnemyDataBase.stageEnemies[i].StartTime));
            }
            enemy.speed *= bd.SpeedPlus;
            frenetic_50 = true;
        }
        else if (enemy.health.CurrentHealth <= enemy.health.health / 4 && !frenetic_25)
        {
            frenetic_25 = true;
            int LevelBoss = DataController.Instance.StageData.Level;
            var bd = DataController.Instance.BossStageDataBase.GetWaveEnemyBoss(LevelBoss);
            enemy.speed *= bd.SpeedPlus;
            timeDelayAttack = bd.DelayAttack;
        }
    }
    private void AttackAndMove()
    {
        if (gameObject.activeSelf)
        {
            if (isAttack)
            {
                CurrentState = EnemyState.Attack;
            }
            StartCoroutine(IEMove());
        }
    }
    public void RandomPosition()
    {
        int index = UnityEngine.Random.Range(0, pointList.Count);
        if (pointList[index] != newPosition)
        {
            newPosition = pointList[index];
            return;
        }
        else
            RandomPosition();
    }
    IEnumerator IEMove()
    {
        yield return new WaitForSeconds(0.8f);
        CurrentState = EnemyState.Idle;
        yield return new WaitForSeconds(timeDelayAttack);
        RandomPosition();
        isAttack = false;
        isMove = true;
    }
    public override void Move(float _speed, float _percentSlow = 100f)
    {
        isMove = true;
        CurrentState = EnemyState.Run;
        gameObject.transform.position = Vector3.MoveTowards(transform.position, newPosition, enemy.speed / 3 * (_percentSlow / 100) * Time.deltaTime);
        if (transform.position == newPosition)
        {
            CurrentState = EnemyState.Idle;
            isAttack = true;
            isMove = false;
            //RandomPosition();
            float ChargeRatio = UnityEngine.Random.Range(0, 100);
            if (enemy.health.CurrentHealth <= enemy.health.health / 4 && ChargeRatio < 20)
            {
                isChargeAttack = true;
                BulletBoss = "BOSS_WIND_SKILL";
                int LevelBoss = DataController.Instance.StageData.Level;
                var bd = DataController.Instance.BossStageDataBase;
                //enemy.speed *= bd.GetWaveEnemyBoss(LevelBoss).SpeedPlus;
                enemy.damage *= (int)bd.GetWaveEnemyBoss(LevelBoss).DamagePlus;
                timeDelayAttack = bd.GetWaveEnemyBoss(LevelBoss).DelayAttack;
                CurrentState = EnemyState.Skill;
                StartCoroutine(IEChargeAttack(2f));
            }
            else
            {
                AttackAndMove();
            }
        }
    }
    IEnumerator IEChargeAttack(float _time)
    {
        yield return new WaitForSeconds(_time);
        CurrentState = EnemyState.Idle;
        int LevelBoss = DataController.Instance.StageData.Level;
        var bd = DataController.Instance.BossStageDataBase;
        enemy.damage /=(int) bd.GetWaveEnemyBoss(LevelBoss).DamagePlus;
        isMove = true;
        isAttack = false;
        RandomPosition();
        isChargeAttack = false;
        BulletBoss = "BOSS_WIND_BULLET";

    }
    public void WindImpactEffect(Vector3 _position)
    {
        gameEffect.SpawnEffect("windimpact", _position, 0.3f);
    }
    public override IEnumerator Die()
    {
        StopCoroutine("IESpawnEnemyBoss");
        return base.Die();
    }
    public IEnumerator IESpawnEnemyBoss(StageEnemyDataBase stageEnemyDataBase, int i, float timeDelay)
    {
        yield return new WaitForSeconds(timeDelay);
        var se = stageEnemyDataBase.stageEnemies[i];
        se.Number--;
        int level = se.Level;
        if (DataController.Instance.StageData.HardMode == 2)
            level += stageEnemyDataBase.NightMareAddLevel;
        else if (DataController.Instance.StageData.HardMode == 3)
            level += stageEnemyDataBase.HellAddLevel;
        GameObject m_Enemy = ObjectPoolManager.Instance.SpawnObject(se.Type, se.Position == 999 ?
            GameplayController.Instance.spawnPosition[UnityEngine.Random.Range(0, 8)].position :
            GameplayController.Instance.spawnPosition[se.Position].position, transform.rotation);
        m_Enemy.GetComponent<EnemyController>().SetUpdata(se.Type, level);
        GameController.Instance.EnemyLive += 1;
        if (se.Number > 0)
        {
            StartCoroutine(IESpawnEnemyBoss(stageEnemyDataBase, i, se.RepeatTime));
        }
    }
    public override void DealDamge(int _damage, float _damageplus = 0)
    {
        base.DealDamge(_damage, _damageplus);
    }
}
