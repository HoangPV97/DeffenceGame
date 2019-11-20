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
}
public class AllianceController : MonoBehaviour
{
    public Alliance Alliance;
    protected EnemyController m_Enemy;
    public GameObject Barrel;
    protected ObjectPoolManager poolManager;
    public Elemental elementalType;
    //public PlaySkeletonAnimationState playSkeletonAnimation;
    public CharacterState characterState, preCharacterState;
    public SkeletonAnimation skeletonAnimation;
    public AnimationReferenceAsset attack, idle;
    [SpineEvent(dataField: "skeletonAnimation", fallbackToTextField: true)]
    public string eventName;
    protected void Start()
    {
        poolManager = ObjectPoolManager.Instance;
        //InvokeRepeating("UpdateEnemy", 0, 0.5f);
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
    protected void UpdateEnemy()
    {
        EnemyController[] Enemies = GameObject.FindObjectsOfType<EnemyController>();
        float shortestDistance = Mathf.Infinity;
        EnemyController nearestEnemy = null;
        foreach (var Enemy in Enemies)
        {
            float distancetoEnemy = Vector3.Distance(transform.position, Enemy.transform.position);
            if (distancetoEnemy < shortestDistance)
            {
                shortestDistance = distancetoEnemy;
                nearestEnemy = Enemy;

            }

        }
        if (nearestEnemy != null && shortestDistance < Alliance.range && nearestEnemy.isLive)
        {
            Alliance.target = nearestEnemy;

            m_Enemy = nearestEnemy.GetComponent<EnemyController>();
        }
        else
        {
            characterState = CharacterState.Idle;
            Alliance.target = null;
        }
    }

    public void SpawnBullet()
    {
         poolManager.SpawnObject(Alliance.Bullet, Barrel.transform.position, Quaternion.identity);

    }
    //public GameObject SpawnBullet(string tag, Vector3 _position)
    //{
    //    return poolManager.SpawnObject(tag, _position, Quaternion.identity);
    //}
}
