﻿using Spine;
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
        int tier = DataController.Instance.GetGameAlliance(elementalType).WeaponTierLevel.Tier;
        bounceRange = GetAttributeData("BouceRange", Elemental.Wind, tier);
        bounceNumber = (int)GetAttributeData("BounceNumber", Elemental.Wind, tier);
        DecreaseDamageBounce = (int)GetAttributeData("DecreaseDamageBounce", Elemental.Wind, tier);
        IncreaseElementDamage = (int)GetAttributeData("IncreaseElementDamage", Elemental.Wind, tier);
        base.Start();
    }
    public override void SetDataWeapon(Elemental elemental, float Atkspeed, float atk, float BulletSpeed, float _range)
    {
        ATK =Mathf.RoundToInt(atk * DataController.Instance.InGameBaseData.achi_AddedDmgAllianceWind);
        base.SetDataWeapon(elemental, Atkspeed, atk, BulletSpeed, _range);
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
            WindallyBullet.SetDataBullet(BulletSpeed, ATK, bounceRange, bounceNumber, IncreaseElementDamage,DecreaseDamageBounce);
        }
    }
}
