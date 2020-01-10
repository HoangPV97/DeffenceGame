using Spine;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindAllianceCharacter : AllianceController
{
    public float bounceRange=5;
    public int numberBounce=3;
    public bool bounce;
    public override void Shoot()
    {
        characterState = CharacterState.Attack;
        GameObject bullet = ObjectPoolManager.Instance.SpawnObject(Alliance.Bullet, Barrel.transform.position, Quaternion.identity);
        var WindallyBullet = bullet.GetComponent<WindAllianceBullet>();
        if (WindallyBullet != null)
        {
            WindallyBullet.elementalBullet = elementalType;
            WindallyBullet.SetTarget(Alliance.target);
            WindallyBullet.SetDataBullet(BulletSpeed, ATK,bounceRange,numberBounce);
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
