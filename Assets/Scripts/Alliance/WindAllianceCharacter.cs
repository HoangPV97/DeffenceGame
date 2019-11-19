using Spine;
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
            GameObject bullet = SpawnBullet("windalliancebullet", Barrel.transform.position);
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

}
