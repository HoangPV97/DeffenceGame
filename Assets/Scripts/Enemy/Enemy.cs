using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Enemy
{
    [SerializeField]
    private float _speed;
    [SerializeField]
    private string _name;
    [SerializeField]
    private float _damage;
    [SerializeField]
    private float _range;
    [SerializeField]
    private float _price;
    [SerializeField]
    private float _armor;
    [SerializeField]
    private float _rateOfFire;
    [SerializeField]
    private Health _health;
    [SerializeField]
    private Elemental _elemental;
    [SerializeField]
    private float _bulletSpeed;

    public float bulletSpeed { get => _bulletSpeed; set => _bulletSpeed = value; }
    public string name { get => _name; set => _name = value; }
    public float speed { get => _speed; set => _speed = value; }
    public float damage { get => _damage; set => _damage = value; }
    public float range { get => _range; set => _range = value; }
    public float price { get => _price; set => _price = value; }
    public float armor { get => _armor; set => _armor = value; }
    public float rateOfFire { get => _rateOfFire; set => _rateOfFire = value; }
    public Health health { get => _health; set => _health = value; }
    public Elemental elemental { get => _elemental; set => _elemental = value; }
}
