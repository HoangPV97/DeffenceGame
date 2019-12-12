using System;
using System.Collections;
using System.Collections.Generic;
using Spine;
using UnityEngine;

public class IceAllianceCharacter : AllianceController
{

    public void IceSkill(Vector3 _position)
    {
        GameObject iceskill = ObjectPoolManager.Instance.SpawnObject(Alliance.Bullet_Skill, _position, Quaternion.identity);
        float particleTime = iceskill.GetComponentInChildren<ParticleSystem>().main.duration;
        SoundManager.Instance.PlayClipOneShot(SoundManager.Instance.Explosion);
        GameObject effectStart = ObjectPoolManager.Instance.SpawnObject(Alliance.EffectStart, this.transform.position + new Vector3(0, 1, 0), Quaternion.identity);
        CheckDestroyEffect(effectStart, particleTime);
        CheckDestroyEffect(iceskill, particleTime);
    }
}
