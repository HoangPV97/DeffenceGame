using Spine;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindRangeEnemy : EnemyController,IWindEffectable
{
    public SkeletonAnimation skeletonAnimation;
    [SpineEvent(dataField: "skeletonAnimation", fallbackToTextField: true)]
    public string eventName;
    void Start()
    {
        skeletonAnimation.AnimationState.Event += OnEvent;
        base.Start();
    }

    private void OnEvent(TrackEntry trackEntry, Spine.Event e)
    {
        bool eventMatch = (e.Data.Name.Equals(eventName));
        if (eventMatch && isAttack)
        {
            Attack();
        }
    }

    // Update is called once per frame
    void Update()
    {
        CheckAttack();
        base.Update();
    }
    public void Attack()
    {


        GameObject EnemyBullet = PoolManager.SpawnObject("enemybullet", transform.position, Quaternion.identity);
        EnemyBullet m_EnemyBullet = EnemyBullet.GetComponent<EnemyBullet>();
        if (m_EnemyBullet != null)
        {
            m_EnemyBullet.SetTarget(Tower.transform);
            m_EnemyBullet.SetDamage(enemy.damage);
        }
    }
    public void CheckAttack()
    {
        distancetoPlayer = Vector3.Distance(transform.position, Tower.transform.position);
        if (distancetoPlayer < enemy.range)
        {
            isAttack = true;

            isMove = false;
            Move();
            if (countdown <= 0f)
            {
                CurrentState = EnemyState.Attack;
                countdown = enemy.rateOfFire;
            }
            countdown -= Time.deltaTime;
        }
    }
}
