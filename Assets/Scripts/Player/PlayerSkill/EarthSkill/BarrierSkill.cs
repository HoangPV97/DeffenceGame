using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierSkill : MonoBehaviour
{
    public SkeletonAnimation skeletonAnimation;
    [SerializeField] float TimeEffect, HealthRecover, EffectedAoe;
    [SerializeField] SkillWeaponEarth2 Swe2;
    protected Tower Tower { get { return GameplayController.Instance.Tower; } }
    public BoxCollider2D BoxCollider2D;
    float countdown;
    bool IsRecover;
    public void SetDataBarrierSkill(float _time, float _health, float _aoe)
    {
        TimeEffect = _time;
        HealthRecover = _health;
        EffectedAoe = _aoe;
    }
    private void Update()
    {
        if (countdown < 0 && IsRecover)
        {
            RecoverHealthTower();
            countdown = 1;
        }
        countdown -= Time.deltaTime;
    }
    public void InvokeSkill()
    {
        StartCoroutine(IEinvokeSkill());
    }
    IEnumerator IEinvokeSkill()
    {
        BoxCollider2D.enabled = false;
        skeletonAnimation.AnimationState.SetAnimation(0, "start", false);
        yield return new WaitForSeconds(0.7f);
        BoxCollider2D.enabled = true;
        IsRecover = true;
        skeletonAnimation.AnimationState.SetAnimation(0, "idle", true);
        yield return new WaitForSeconds(TimeEffect);
        IsRecover = false;
        skeletonAnimation.AnimationState.SetAnimation(0, "end", false);
    }
    public void RecoverHealthTower()
    {
        Tower.Health.CurrentHealth += (int)HealthRecover;
        Tower.Health.healthBar.fillAmount += HealthRecover / Tower.Health.health;
        if (Tower.Health.CurrentHealth > Tower.Health.health)
        {
            Tower.Health.CurrentHealth = Tower.Health.health;
        }
        Tower.Health.UpdateValueText();
    }
    private void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.gameObject.tag.Equals("Enemy"))
        {
            EnemyController enemyController = collider2D.gameObject.GetComponent<EnemyController>();
            enemyController.DisableAttackIntimeInterval(TimeEffect);
        }
    }
}
