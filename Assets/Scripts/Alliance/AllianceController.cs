using Spine;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[System.Serializable]
#region Alliance
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
#endregion
public class AllianceController : MonoBehaviour
{
    public Alliance Alliance;
    public GameObject Barrel;
    public Elemental elementalType;
    protected float shortestDistance, _2ndShortestDistance;
    protected EnemyController nearestEnemy, _2ndEnemy;
    //public PlaySkeletonAnimationState playSkeletonAnimation;
    public CharacterState characterState, preCharacterState;
    public SkeletonAnimation skeletonAnimation;
    public AnimationReferenceAsset attack, idle;
    [SpineEvent(dataField: "skeletonAnimation", fallbackToTextField: true)]
    public string eventName;
    public List<EnemyController> listEnemies;
    public int ATK;
    public float ATKspeed;
    public float ATKplus;
    public float BulletSpeed;
    public CircleCollider2D CircleCollider2D;
    public virtual void Start()
    {
        CircleCollider2D.radius = Alliance.range;
        listEnemies = new List<EnemyController>();
        skeletonAnimation.AnimationState.Event += OnEvent;
    }
    public virtual void SetDataWeapon(Elemental elemental, float Atkspeed, float atk, float BulletSpeed,float _range)
    {
        ATK =Mathf.RoundToInt(ATK*DataController.Instance.InGameBaseData.achi_AddedDmgAlliance) ;
        ATK += (int)DataController.Instance.InGameBaseData.AllianceDamage;
        this.elementalType = elemental;
        ATKspeed = Atkspeed;
        this.BulletSpeed = BulletSpeed;
        Alliance.range = _range;
    }
    protected virtual void Update()
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
        CheckShoot();
    }
    private void ChangeCharacterState()
    {
        if (characterState.Equals(CharacterState.Attack))
        {
            skeletonAnimation.AnimationState.SetAnimation(0, attack, true);
            skeletonAnimation.timeScale = ATKspeed / 100;
        }
        else if (characterState.Equals(CharacterState.Idle))
        {
            skeletonAnimation.AnimationState.SetAnimation(0, idle, true);
            skeletonAnimation.timeScale = 1;
        }

    }
    public virtual void UpdateEnemy()
    {
        if (listEnemies.Count > 0)
        {
            int index = 0;
            listEnemies = listEnemies.OrderBy(obj => (obj.transform.position - transform.position).magnitude).ToList();
            nearestEnemy = listEnemies[index];
            if (!nearestEnemy.isLive && listEnemies.Count > 1)
            {
                nearestEnemy = listEnemies[index + 1];
            }
            if (nearestEnemy != null && nearestEnemy.isLive)
            {
                Alliance.target = nearestEnemy;
            }
            else
            {
                Alliance.target = null;
                characterState = CharacterState.Idle;
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
    private void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.gameObject.tag.Equals("Enemy"))
        {
            listEnemies.Add(collider2D.gameObject.GetComponent<EnemyController>());
        }
    }

    public virtual void CheckShoot()
    {
        if (Alliance.target != null)
        {
            characterState = CharacterState.Attack;
        }
        else
        {
            characterState = CharacterState.Idle;
        }
    }

    public virtual void Shoot()
    {
        //characterState = CharacterState.Attack;
        //GameObject bullet = ObjectPoolManager.Instance.SpawnObject(Alliance.Bullet, Barrel.transform.position, Quaternion.identity);
        //var alianceBullet = bullet.GetComponent<BulletController>();
        //if (alianceBullet != null)
        //{
        //    alianceBullet.elementalBullet = elementalType;
        //    alianceBullet.SetTarget(Alliance.target);
        //    alianceBullet.SetDataBullet(BulletSpeed, ATK);
        //}
    }

    private void OnEvent(TrackEntry trackEntry, Spine.Event e)
    {
        bool eventMatch = (e.Data.Name.Equals(eventName));
        if (eventMatch)
        {
            Shoot();
        }
    }
    public float GetAttributeData(string attribute,Elemental elemental, int tier)
    {
        var Attributes = DataController.Instance.GetAllianceDataBases(elemental, tier).SkillAttributes;
        for (int i = 0; i < Attributes.Count; i++)
        {
            if (Attributes[i].Attribute.Equals( attribute))
                return Attributes[i].Value[0];
        }
        return 0;
    }
}
