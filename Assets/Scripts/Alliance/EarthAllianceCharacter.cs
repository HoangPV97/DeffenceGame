using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthAllianceCharacter : AllianceController
{
    float StunTime;
    int IncreaseElementDamage;
    // Start is called before the first frame update
    public override void Start()
    {
        int tier = DataController.Instance.GetGameDataWeapon(elementalType).WeaponTierLevel.Tier;
        StunTime = GetAttributeData("StunTime", elementalType, tier);
        IncreaseElementDamage = (int)GetAttributeData("IncreaseElementDamage", elementalType, tier);
        base.Start();
    }
    public override void SetDataWeapon(Elemental elemental, float Atkspeed, float atk, float BulletSpeed, float _range)
    {
        ATK = (int)(atk * DataController.Instance.InGameBaseData.achi_AddedDmgAllianceWind);
        base.SetDataWeapon(elemental, Atkspeed, atk, BulletSpeed, _range);
    }

    public override void Shoot()
    {
        characterState = CharacterState.Attack;
        GameObject bullet = ObjectPoolManager.Instance.SpawnObject(Alliance.Bullet, Barrel.transform.position, Quaternion.identity);
        var EarthAllyBullet = bullet.GetComponent<EarthAllianceBullet>();
        if (EarthAllyBullet != null)
        {
            EarthAllyBullet.elementalBullet = elementalType;
            EarthAllyBullet.SetTarget(Alliance.target);
            EarthAllyBullet.SetDataBullet(BulletSpeed, ATK, StunTime, IncreaseElementDamage);
            EarthAllyBullet.stun = true;
        }
    }
}
