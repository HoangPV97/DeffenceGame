using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Bullet
{
    [SerializeField]
    private float _speed;
    [SerializeField]
    private float _damage;
    [SerializeField]
    private string _targetTag;
    [SerializeField]
    private float _knockbackDistance;
    [SerializeField]
    private float _critical_Damage;

    public float Speed { get => _speed; set => _speed = value; }
    public float Damage { get => _damage; set => _damage = value; }
    public string TargetTag { get => _targetTag; set => _targetTag = value; }
    public float KnockbackDistance { get => _knockbackDistance; set => _knockbackDistance = value; }
    public float CriticalDamage { get => _critical_Damage; set => _critical_Damage = value; }
}
