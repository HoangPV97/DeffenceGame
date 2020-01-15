using Spine;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindAllianceCharacter : AllianceController
{
    [SerializeField] private float bounceRange = 5;
    [SerializeField] private int DecreaseDamageBounce;
    [SerializeField] private int IncreaseElementDamage;
    public int bounceNumber = 3;
    public bool bounce;

    public override void Start()
    {
        int tier = DataController.Instance.GetGameDataWeapon(elementalType).WeaponTierLevel.Tier;
        bounceRange = GetAttributeData("BouceRange", Elemental.Wind, tier);
        bounceNumber = (int)GetAttributeData("BounceNumber", Elemental.Wind, tier);
        DecreaseDamageBounce = (int)GetAttributeData("DecreaseDamageBounce", Elemental.Wind, tier);
        IncreaseElementDamage = (int)GetAttributeData("IncreaseElementDamage", Elemental.Wind, tier);
        base.Start();
    }
    public override void Shoot()
    {
        characterState = CharacterState.Attack;
        GameObject bullet = ObjectPoolManager.Instance.SpawnObject(Alliance.Bullet, Barrel.transform.position, Quaternion.identity);
        var WindallyBullet = bullet.GetComponent<WindAllianceBullet>();
        if (WindallyBullet != null)
        {
            WindallyBullet.elementalBullet = elementalType;
            WindallyBullet.SetTarget(Alliance.target);
            WindallyBullet.SetDataBullet(BulletSpeed, ATK, bounceRange, bounceNumber, IncreaseElementDamage);
        }
    }
    public void StunSkill(Vector3 _position)
    {
        GameObject stunSkill = ObjectPoolManager.Instance.SpawnObject(Alliance.Bullet_Skill, _position, Quaternion.identity);
        float particleTime = stunSkill.GetComponentInChildren<ParticleSystem>().main.duration;
        SoundManager.Instance.PlayClipOneShot(SoundManager.Instance.Explosion);
        GameObject effectStart = ObjectPoolManager.Instance.SpawnObject(Alliance.EffectStart, this.transform.position + new Vector3(0, 1, 0), Quaternion.identity);
        CheckDestroyEffect(effectStart, particleTime);
        CheckDestroyEffect(stunSkill, 1f);
    }
}
