using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Player
{
    public string name { get => _name; set => _name = value; }
    public float Level { get => _level; set => _level = value; }
    public float range { get => _range; set => _range = value; }
    public Transform target { get => _target; set => _target = value; }
    public float rateOfFire { get => _rateOfFire; set => _rateOfFire = value; }
    public float Armor { get => _armor; set => _armor = value; }
    public string Bullet { get => bullet; set => bullet = value; }

    [SerializeField]
    private string bullet;
    [SerializeField]
    private string _name;
    [SerializeField]
    private float _level;
    [SerializeField]
    private float _range;
    [SerializeField]
    private Transform _target;
    [SerializeField]
    private float _rateOfFire;
    [SerializeField]
    private float _armor;
}
