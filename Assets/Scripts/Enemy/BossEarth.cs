using Spine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BossEarth : EnemyController
{
    public List<Vector3> pointList;
    bool frenetic_75, frenetic_50, frenetic_25;
    Vector3 newPosition;
    float timeDelayAttack = 1f;
    public GameObject LeftHand, RightHand;
    public GameObject boss_Fx;
    public UnityEvent EventOnFx;
    bool isChargeAttack;
    // Update is called once per frame
    protected override void Start()
    {
        isAttack = false;
        timeDelayAttack = DataController.Instance.BossDataBase_Wind.GetWaveEnemyBoss_Wind_1(DataController.Instance.StageData.HardMode).DelayAttack;
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
        if (!isAttack && isMove && gameEffect.CurrentEffect == Effect.None && !isChargeAttack)
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
        if (Tower != null && isAttack && !disableAttack)
        {
            gameEffect.SpawnEffect("WIND_MELEE_IMPACT", gameObject.transform.position - new Vector3(0, 1, 0), 0.5f);
            Tower.TakeDamage(enemy.damage);
            if (isChargeAttack)
            {
                DealEffect(Effect.Stun, transform.position + new Vector3(0, 0.5f, 0), 2);
                isChargeAttack = false;   
            }
        }
    }
    public override void CheckAttack()
    {
        if (enemy.health.CurrentHealth <= enemy.health.health * 0.75f && !frenetic_75)
        {
            isChargeAttack = true;
            frenetic_75 = true;
            enemy.speed += enemy.speed * 0.2f;
        }
        if (enemy.health.CurrentHealth <= enemy.health.health / 2 && enemy.health.CurrentHealth > enemy.health.health / 4 && !frenetic_50)
        {
            isChargeAttack = true;
            float ChargeRatio = UnityEngine.Random.Range(0, 100);
            int HardMode = DataController.Instance.StageData.HardMode;
            var sd = DataController.Instance.BossDataBase_Wind.GetWaveEnemyBoss_Wind_1(HardMode);
            for (int i = 0; i < sd.stageEnemyDataBase.stageEnemies.Count; i++)
            {
                StartCoroutine(IESpawnEnemyBoss(sd.stageEnemyDataBase, i, sd.stageEnemyDataBase.stageEnemies[i].StartTime));
            }
            enemy.speed += enemy.speed * 0.2f;
            frenetic_50 = true;
        }
        else if (enemy.health.CurrentHealth <= enemy.health.health / 4 && !frenetic_25)
        {
            isChargeAttack = true;
            enemy.speed += enemy.speed * 0.2f;
            frenetic_25 = true;

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
        //Rigidbody2D.velocity = DirectionMove * (distance / enemy.speed);
        gameObject.transform.position = Vector3.MoveTowards(transform.position, newPosition, enemy.speed / 3 * (_percentSlow / 100) * Time.deltaTime);
        if (transform.position == newPosition && isAttack)
        {
            CurrentState = EnemyState.Attack;
        }
        ////RandomPosition();
        //float ChargeRatio = UnityEngine.Random.Range(0, 100);
        //if (enemy.health.CurrentHealth <= enemy.health.health / 4 && ChargeRatio < 20)
        //{
        //    isChargeAttack = true;
        //    enemy.damage *= 2;
        //    CurrentState = EnemyState.Skill;
        //    StartCoroutine(IEChargeAttack(2f));
        //}
        else if (transform.position == newPosition && isChargeAttack)
        {
            enemy.damage *= 2;
            CurrentState = EnemyState.Skill;
            StartCoroutine(IEChargeAttack(2f));
        }
    }
    IEnumerator IEChargeAttack(float _time)
    {
        enemy.speed *= 2;
        yield return new WaitForSeconds(_time);
        newPosition = Tower.transform.position;
        enemy.damage /= 2;
        enemy.speed /= 2;
        isMove = true;
        isAttack = false;
    }
    public void WindImpactEffect(Vector3 _position)
    {
        gameEffect.SpawnEffect("windimpact", _position, 0.3f);
    }
    public override IEnumerator Die()
    {
        GameplayController.Instance.StopCoroutine("IESpawnEnemyBoss");
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
            GameplayController.Instance.spawnPosition[Random.Range(0, 8)].position :
            GameplayController.Instance.spawnPosition[se.Position].position, transform.rotation);
        m_Enemy.GetComponent<EnemyController>().SetUpdata(se.Type, level);
        GameController.Instance.EnemyLive += 1;
        if (se.Number > 0)
        {
            StartCoroutine(IESpawnEnemyBoss(stageEnemyDataBase, i, se.RepeatTime));
        }
    }
}
