using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IIceEffectable
{
    void IceImpactEffect( Vector3 _position);
}
public interface IWindEffectable
{
    void WindImpactEffect(Vector3 _position);
}public interface IEarthEffectable
{
    void EarthImpactEffect(Vector3 _position);
}
public interface IFireEffectable
{
    void FireImpactEffect(Vector3 _position);
}
public interface IEffectSkill
{
    void DealEffect(Effect _effect,Vector3 _position,float _time);
}
public interface IExplosionBullet
{
    void explosionBulletEffect();
}