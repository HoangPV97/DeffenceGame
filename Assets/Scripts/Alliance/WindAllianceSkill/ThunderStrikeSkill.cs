using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ThunderStrikeSkill : MonoBehaviour
{
    public float EffectedTime, Damage, EffectedAoe;
    // Start is called before the first frame update
    public virtual void SetSkillData(float _EffectedTime,  float _Damage, float _EffectedAoe)
    {
        EffectedTime = _EffectedTime;
        Damage = _Damage;
        EffectedAoe = _EffectedAoe;
    }
    protected  void OnTriggerEnter2D(Collider2D Target)
    {
        if (Target.gameObject.tag.Equals("Enemy"))
        {
            EnemyController enemy = Target.GetComponent<EnemyController>();
            if (enemy != null)
            {
                enemy?.DealDamge(Damage, 0);
                enemy.DealEffect(Effect.Stun,enemy.transform.position+new Vector3(0,0.5f,0),EffectedTime);
            }
        }
    }
    
}
