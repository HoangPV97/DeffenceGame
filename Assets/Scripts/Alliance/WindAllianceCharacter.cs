using Spine;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindAllianceCharacter : AllianceController
{
    Vector2 direct;
    private void Start()
    {

        skeletonAnimation.AnimationState.Event += OnEvent;
        base.Start();
    }

    private void OnEvent(TrackEntry trackEntry, Spine.Event e)
    {
        bool eventMatch = (e.Data.Name.Equals(eventName));
        if (eventMatch)
        {
            Shoot();
        }
    }

    private void Update()
    {
        base.Update();
        CheckShoot();
    }

    protected void Shoot()
    {
        characterState = CharacterState.Attack;
        GameObject bullet = ObjectPoolManager.Instance.SpawnObject(Alliance.Bullet, Barrel.transform.position, Quaternion.identity);
        WindAllianceBullet allianceBullet = bullet.GetComponent<WindAllianceBullet>();
        if (Alliance.target != null)
        {
            allianceBullet.SetTarget(Alliance.target);
            allianceBullet.elementalBullet = elementalType;
            allianceBullet.SetDataBullet(ATKspeed, ATK);
        }
    }
    void CheckShoot()
    {
        if (Alliance.target != null)
        {
            characterState = CharacterState.Attack;
        }
        else
        {
            characterState = CharacterState.Idle;
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
