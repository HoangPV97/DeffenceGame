using Spine;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceRangeEnemy : EnemyController
{
    private bool tranfigure,checkCoroutine;
    [SerializeField] private string BulletName;
    [SerializeField] private GameObject Barrel;
    // Update is called once per frame
    protected override void Update()
    {
        if (previousState != CurrentState)
        {
            ChangeState();
            previousState = CurrentState;
        }
        CheckAttack();
    }
    public void Attack()
    {
        GameObject EnemyBullet = ObjectPoolManager.Instance.SpawnObject(BulletName,Barrel.transform.position, Quaternion.identity);
        EnemyBullet m_EnemyBullet = EnemyBullet.GetComponent<EnemyBullet>();
        if (m_EnemyBullet != null)
        {
            m_EnemyBullet.SetTarget(Tower.transform);
            m_EnemyBullet.SetDamage(enemy.damage);
        }
    }
    public override void CheckAttack()
    {
        distancetoTower = Mathf.Abs(transform.position.y - Tower.transform.position.y);

        if ((distancetoTower <= 1.7f || distancetoTower <= enemy.range) && isLive)
        {
            isAttack = true;
            isMove = false;
            Rigidbody2D.velocity = Vector2.zero;
            if (!tranfigure && !checkCoroutine)
            {
                StartCoroutine(IEWaitingAttack());
            }
            if (!Check_Stun_Freeze() && tranfigure)
            {
                CurrentState = EnemyState.Attack;
            }
        }
        else
        {
            isAttack = false;
        }
    }
    IEnumerator IEWaitingAttack()
    {
        checkCoroutine = true;
        CurrentState = EnemyState.Idle;
        yield return new WaitForSeconds(1);
        skeletonAnimation.AnimationState.SetAnimation(0, skill, false);
        yield return new WaitForSeconds(1);
        tranfigure = true;
    }
}
