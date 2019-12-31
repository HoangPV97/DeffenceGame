using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region IceExplosionData
[System.Serializable]
public class SlowSkillData
{
    public float EffectedTime;
    public float EffectedAoe;
    public float SLowdownPercent;
    public float Damage;
    public SlowSkillData(float _effectedTime, float _EffectedAoe, float _SlowdownPercent, float _Damage)
    {
        this.EffectedTime = _effectedTime;
        this.EffectedAoe = _EffectedAoe;
        this.SLowdownPercent = _SlowdownPercent;
        this.Damage = _Damage;
    }
}
#endregion
public class SlowSkill : MonoBehaviour
{
    public SlowSkillData SlowSkillData;
    public CircleCollider2D circleCollider2D;
    //List<EnemyController> enemies;
    // Start is called before the first frame update
    void Start()
    {
        circleCollider2D.radius = SlowSkillData.EffectedAoe;
    }
    public virtual void SetSkillData(float EffectedTime, float SlownDownPercent, float Damage, float EffectedAoe)
    {
        SlowSkillData.EffectedTime = EffectedTime;
        SlowSkillData.EffectedAoe = EffectedAoe;
        SlowSkillData.SLowdownPercent = SlownDownPercent;
        SlowSkillData.Damage = Damage;
    }
    private void OnTriggerEnter2D(Collider2D _target)
    {
        if (_target.gameObject.tag.Equals("Enemy"))
        {
            EnemyController enemyController = _target.gameObject.GetComponent<EnemyController>();
            enemyController.Deal_Slow_Effect( SlowSkillData.EffectedTime, SlowSkillData.SLowdownPercent);
            //enemyController.Move(enemyController.enemy.speed, SlowSkillData.SLowdownPercent);
            enemyController.DealDamge(SlowSkillData.Damage);
        }
        ObjectPoolManager.Instance.DespawnObJect(this.gameObject);
    }
}
