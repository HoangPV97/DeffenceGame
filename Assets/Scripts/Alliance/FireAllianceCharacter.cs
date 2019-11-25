using Spine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FireAllianceCharacter : AllianceController
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
        //UpdateEnemy(Alliance.target, Alliance.range);
        base.Update();
    }
    public void Shoot()
    {
        characterState = CharacterState.Attack;
        GameObject bullet = ObjectPoolManager.Instance.SpawnObject(Alliance.Bullet, Barrel.transform.position, Quaternion.identity);
        FireAllianceBullet allianceBullet = bullet.GetComponent<FireAllianceBullet>();
        if (allianceBullet != null)
        {
            allianceBullet.elementalBullet = elementalType;
            allianceBullet.SetTarget(Alliance.target);
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
}
