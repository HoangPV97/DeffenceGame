using Spine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FireAllianceCharacter : AllianceController
{
    public override void Start()
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
    public override void SetDataWeapon(Elemental elemental, float Atkspeed, float atk, float BulletSpeed, float _range)
    {
        ATK = (int)(atk* DataController.Instance.InGameBaseData.achi_AddedDmgAllianceFire);
        base.SetDataWeapon(elemental, Atkspeed, atk, BulletSpeed, _range);
    }
    public override void Shoot()
    {
        characterState = CharacterState.Attack;
        GameObject bullet = ObjectPoolManager.Instance.SpawnObject(Alliance.Bullet, Barrel.transform.position, Quaternion.identity);
        FireAllianceBullet allianceBullet = bullet.GetComponent<FireAllianceBullet>();
        if (allianceBullet != null)
        {
            allianceBullet.SetDataBullet(BulletSpeed, ATK);
            allianceBullet.elementalBullet = elementalType;
            allianceBullet.SetTarget(Alliance.target);
        }
    }
    public override void CheckShoot()
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
