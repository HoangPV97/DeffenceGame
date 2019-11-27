using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Alliance
{
    public string name { get => _name; set => _name = value; }
    public float Level { get => _level; set => _level = value; }
    public float range { get => _range; set => _range = value; }
    public EnemyController target { get => _target; set => _target = value; }
    public float rateOfFire { get => _rateOfFire; set => _rateOfFire = value; }
    public float Armor { get => _armor; set => _armor = value; }
    public string Bullet { get => bullet; set => bullet = value; }
    public string Bullet_Skill { get => _bulletSkill; set => _bulletSkill = value; }
    public string EffectStart { get => _effectStart; set => _effectStart = value; }

    [SerializeField] private string bullet;
    [SerializeField] private string _name;
    [SerializeField] private float _level;
    [SerializeField] private float _range;
    [SerializeField] private EnemyController _target;
    [SerializeField] private float _rateOfFire;
    [SerializeField] private float _armor;
    [SerializeField] private string _bulletSkill;
    [SerializeField] private string _effectStart;

}
public class AllianceController : MonoBehaviour
{
    public Alliance Alliance;
    public GameObject Barrel;
    public Elemental elementalType;
    float shortestDistance, _2ndShortestDistance;
    EnemyController nearestEnemy, _2ndEnemy;
    //public PlaySkeletonAnimationState playSkeletonAnimation;
    public CharacterState characterState, preCharacterState;
    public SkeletonAnimation skeletonAnimation;
    public AnimationReferenceAsset attack, idle;
    [SpineEvent(dataField: "skeletonAnimation", fallbackToTextField: true)]
    public string eventName;
    protected void Start()
    {
    }
    protected void Update()
    {
        UpdateEnemy();
        if (preCharacterState != characterState)
        {
            ChangeCharacterState();
            preCharacterState = characterState;
        }
        if (Alliance.target == null)
        {
            return;
        }

        //AutoShoot();

    }
    private void ChangeCharacterState()
    {
        if (characterState.Equals(CharacterState.Attack))
        {
            skeletonAnimation.AnimationState.SetAnimation(0, attack, true);

        }
        else if (characterState.Equals(CharacterState.Idle))
        {
            skeletonAnimation.AnimationState.SetAnimation(0, idle, true);
        }

    }
    public void UpdateEnemy()
    {
        shortestDistance = Mathf.Infinity;
        _2ndShortestDistance = Mathf.Infinity;
        nearestEnemy = null;
        _2ndEnemy = null;
        if (Enemies.listEnemies.Count > 0)
        {
            for (int i = 0; i < Enemies.listEnemies.Count; i++)
            {
                float distancetoEnemy = Vector3.Distance(transform.position, Enemies.listEnemies[i].transform.position);
                if (distancetoEnemy < shortestDistance)
                {
                    _2ndShortestDistance = shortestDistance;
                    shortestDistance = distancetoEnemy;
                    _2ndEnemy = nearestEnemy;
                    nearestEnemy = Enemies.listEnemies[i];
                }
                else if (distancetoEnemy < _2ndShortestDistance && distancetoEnemy != shortestDistance)
                {
                    _2ndShortestDistance = distancetoEnemy;
                    _2ndEnemy = Enemies.listEnemies[i];
                }
                if (nearestEnemy != null && shortestDistance < Alliance.range && nearestEnemy.isLive)
                {
                    Alliance.target = nearestEnemy;
                }
                else if (_2ndEnemy != null && !nearestEnemy.isLive)
                {
                    nearestEnemy = _2ndEnemy;
                    Alliance.target = nearestEnemy;
                }
                if (!nearestEnemy.isLive && _2ndEnemy == null)
                {
                    Alliance.target = null;
                }
            }
        }
        else
        {
            characterState = CharacterState.Idle;
        }

    }

    public void SpawnBullet()
    {
        ObjectPoolManager.Instance.SpawnObject(Alliance.Bullet, Barrel.transform.position, Quaternion.identity);

    }
    public void CheckDestroyEffect(GameObject Obj, float _time)
    {
        if (!Obj.GetComponent<DestroyEffect>())
        {
            Obj.AddComponent<DestroyEffect>()._time = _time;
        }
        else
        {
            Obj.GetComponent<DestroyEffect>().Start();
        }
    }
}
