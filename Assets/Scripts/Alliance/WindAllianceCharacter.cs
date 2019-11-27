﻿using Spine;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindAllianceCharacter : AllianceController
{
    //public SkeletonAnimation skeletonAnimation;
    //public AnimationReferenceAsset attack,idle;
    //[SpineEvent(dataField: "skeletonAnimation", fallbackToTextField: true)]

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
        CheckShoot();
        base.Update();
    }

    protected void Shoot()
    {
        if (Alliance.target != null)
        {
            characterState = CharacterState.Attack;
            GameObject bullet = ObjectPoolManager.Instance.SpawnObject(Alliance.Bullet, Barrel.transform.position, Quaternion.identity);
            WindAllianceBullet alianceBullet = bullet.GetComponent<WindAllianceBullet>();
            if (alianceBullet != null)
            {
                alianceBullet.elementalBullet = elementalType;
                alianceBullet.SetTarget(Alliance.target);
            }

        }
        else
        {
            characterState = CharacterState.Idle;
        }
    }
    void CheckShoot()
    {
        if (Alliance.target != null)
        {
            characterState = CharacterState.Attack;
        }
    }
    public void StunSkill(Vector3 _position)
    {
        GameObject stunSkill = ObjectPoolManager.Instance.SpawnObject(Alliance.Bullet_Skill, _position, Quaternion.identity);
        float particleTime = stunSkill.GetComponentInChildren<ParticleSystem>().main.duration;
        SoundManager.Instance.PlayClipOneShot(SoundManager.Instance.Explosion);
        GameObject effectStart = ObjectPoolManager.Instance.SpawnObject(Alliance.EffectStart, this.transform.position, Quaternion.identity);
        CheckDestroyEffect(effectStart, particleTime);
        CheckDestroyEffect(stunSkill, 0.5f);
    }
}
