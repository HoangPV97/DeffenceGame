using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FireAllianceCharacter : AllianceController 
{

    private void Start()
    {
        base.Start();
    }
    private void Update()
    {
        //UpdateEnemy(Alliance.target, Alliance.range);
        base.Update();
    }
    public void Shoot()
    {
        if (Alliance.target != null)
        {
            characterState = CharacterState.Attack;
            GameObject bullet = SpawnBullet("firealliancebullet", Barrel.transform.position);
            FireAllianceBullet allianceBullet = bullet.GetComponent<FireAllianceBullet>();
            if (allianceBullet != null)
            {
                allianceBullet.elementalBullet = elementalType;
                allianceBullet.SetTarget(Alliance.target);
            }
            
        }
        else
        {
            characterState = CharacterState.Idle;
        }
        //base.Shoot();
    }
}
