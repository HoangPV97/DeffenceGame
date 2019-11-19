using System;
using System.Collections;
using System.Collections.Generic;
using Spine;
using UnityEngine;

public class IceAllianceCharacter : AllianceController
{

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
            GameObject bullet = SpawnBullet("icealliancebullet", Barrel.transform.position);
            IceAllianceBullet alianceBullet = bullet.GetComponent<IceAllianceBullet>();
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
