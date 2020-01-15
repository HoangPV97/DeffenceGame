using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ThunderStrikeSkill : MonoBehaviour
{
    public ParticleScaler ParticleScaler;
    public ParticleSystem ParticleSystem;
    public float EffectedTime, Damage, EffectedAoe;
    private void Start()
    {
        ParticleScaler.ScaleByTransform(ParticleSystem, (EffectedAoe/10), true);
    }
    // Start is called before the first frame update
    public virtual void SetSkillData(float _EffectedTime, float _Damage, float _EffectedAoe)
    {
        EffectedTime = _EffectedTime;
        Damage = _Damage;
        EffectedAoe = _EffectedAoe;
    }
    protected void OnTriggerEnter2D(Collider2D Target)
    {
        if (Target.gameObject.tag.Equals("Enemy"))
        {
            EnemyController enemy = Target.GetComponent<EnemyController>();
            if (enemy != null)
            {
                enemy?.DealDamge((int)Damage, 0);
                enemy.DealEffect(Effect.Stun, enemy.transform.position + new Vector3(0, 0.5f, 0), EffectedTime);
            }
        }
    }

}
