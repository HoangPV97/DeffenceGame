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
        InvokeRepeating("RandomPosition", 0, 4f);
    }
    protected override void Update()
    {
        //if (isAttack && countdown <= 0)
        //{
        //    RandomPosition();
        //    countdown = 4f;
        //}
        //countdown -= Time.deltaTime;
        if (!isAttack  && gameEffect.CurrentEffect == Effect.None)
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

        //else
        //{
        //    isAttack = false;
        //    isMove = true;
        //}

        if (enemy.health.CurrentHealth <= enemy.health.health / 2 && enemy.health.CurrentHealth > enemy.health.health / 4 && !frenetic_50)
        {
            float ChargeRatio = UnityEngine.Random.Range(0, 100);
            int hardmode = DataController.Instance.StageData.HardMode;
            var se = JsonUtility.FromJson<SpawnEnemyBoss>(ConectingFireBase.Instance.GetTexSpawnEnemyBoss(hardmode));
            var sd = DataController.Instance.StageData;
            for (int i = 0; i < sd.stageEnemyDataBase.stageEnemies.Count; i++)
            {
                StartCoroutine(IESpawnEnemy(i, sd.stageEnemyDataBase.stageEnemies[i].StartTime));
                GameController.Instance.EnemyLive += sd.stageEnemyDataBase.stageEnemies[i].Number;
            }
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
        isAttack = true;
        skeletonAnimation.AnimationState.SetAnimation(0, attack, true);
        int newPosition = UnityEngine.Random.Range(0, pointList.Count);
        newpposition = pointList[newPosition];
    }
    private void MoveRandom()
    {
        isAttack = false;
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
    public IEnumerator IESpawnEnemy(int i, float timeDelay)
    {
        yield return new WaitForSeconds(timeDelay);
        var se= JsonUtility.FromJson<SpawnEnemyBoss>(ConectingFireBase.Instance.GetTexSpawnEnemyBoss(i));
        se.stageEnemies[i].Number--;
        int level = se.stageEnemies[i].Level;
        if (DataController.Instance.StageData.HardMode == 2)
            level += DataController.Instance.StageData.stageEnemyDataBase.NightMareAddLevel;
        else if (DataController.Instance.StageData.HardMode == 3)
            level += DataController.Instance.StageData.stageEnemyDataBase.HellAddLevel;
        // spawnEnemy
        GameObject m_Enemy = ObjectPoolManager.Instance.SpawnObject(se.stageEnemies[i].Type, se.stageEnemies[i].Position == 999 ? 
            GameplayController.Instance.spawnPosition[UnityEngine.Random.Range(0, 8)].position : 
            GameplayController.Instance.spawnPosition[se.stageEnemies[i].Position].position, transform.rotation);
        m_Enemy.GetComponent<EnemyController>().SetUpdata(se.stageEnemies[i].Type, level);
        //repeat
        if (se.stageEnemies[i].Number > 0)
        {
            StartCoroutine(IESpawnEnemy(i, se.stageEnemies[i].RepeatTime));
        }

    }
}
