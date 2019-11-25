using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Player
{
    public string name { get => _name; set => _name = value; }
    public float Level { get => _level; set => _level = value; }
    public float range { get => _range; set => _range = value; }
    public EnemyController target { get => _target; set => _target = value; }
    public float rateOfFire { get => _rateOfFire; set => _rateOfFire = value; }
    public float Armor { get => _armor; set => _armor = value; }
    public string Bullet { get => bullet; set => bullet = value; }
    public string Bullet_Skill_1 { get => bulletskill1; set => bulletskill1 = value; }
    public string effectStart { get => _effectStart; set => _effectStart = value; }
    public void Skill1() { }
    public void Skill2() { }

    [SerializeField]
    private string bullet;
    [SerializeField]
    private string _name;
    [SerializeField]
    private float _level;
    [SerializeField]
    private float _range;
    [SerializeField]
    private EnemyController _target;
    [SerializeField]
    private float _rateOfFire;
    [SerializeField]
    private float _armor;
    [SerializeField]
    private string bulletskill1;
    [SerializeField]
    private string _effectStart;
}
