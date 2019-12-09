using Spine;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindAllianceCharacter : AllianceController
{
    //public float ATK;
    //public float ATKspeed;
    //public void SetDataWeapon()
    //{
    //    this.elementalType = DataController.Instance.IngameAlliance2.Type;
    //    ///
    //    /// set file spine/
    //    ///
    //    ATK = DataController.Instance.IngameAlliance2.ATK;
    //    ATKspeed = DataController.Instance.IngameAlliance2.ATKspeed;
    //}
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
        GameObject effectStart = ObjectPoolManager.Instance.SpawnObject(Alliance.EffectStart, this.transform.position+new Vector3(0,1,0), Quaternion.identity);
        CheckDestroyEffect(effectStart, particleTime);
        CheckDestroyEffect(stunSkill, 1f);
    }
}
