using Spine;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public enum CharacterState { Idle, Attack };
public enum AutoMode { TurnOn, TurnOff };
public class PlayerController : MonoBehaviour
{
    public List<EnemyController> listEnemies;
    public Player player;
    public AutoMode currentMode;
    public ViewPlayerController ViewPlayer;
    private float coundown;
    public GameObject Barrel;
    public Elemental elementalType;
    public CharacterState characterState, preCharacterState;
    public SkeletonAnimation skeletonAnimation;
    public AnimationReferenceAsset attack, idle;
    [SpineEvent(dataField: "skeletonAnimation", fallbackToTextField: true)]
    public string eventName;
    Vector2 direct;
    float rotationZ;
    EnemyController nearestEnemy = null;
    public CircleCollider2D CircleCollider2D;
    bool idleStatus;
    [Header("Weapon Data")]
    public float ATK;
    public float ATKspeed;
    public float BulletSpeed;
    [Header("Archery Data")]
    public int ArcheryDamage;
    public int CriticalChance;
    public int KnockBackChance;
    public int AllianceDamage;
    public int QuickHandChance;
    public float QuickHandDamagePercent;
    public bool MultiShotChance;
    public float MultiShotDamage;
    public float MultiShotAddedAttributePercent;

    // Start is called before the first frame update
    public void Awake()
    {
        CircleCollider2D.radius = player.range;
    }
    void Start()
    {
        skeletonAnimation.AnimationState.Event += OnEvent;
        skeletonAnimation.AnimationState.Complete += OnAttack;
        currentMode = AutoMode.TurnOff;
    }
    public void SetDataWeapon()
    {
        this.elementalType = DataController.Instance.inGameWeapons.Type;
        ATK = DataController.Instance.inGameWeapons.ATK + DataController.Instance.InGameBaseData.Damage;
        ATKspeed = DataController.Instance.inGameWeapons.ATKspeed;
        BulletSpeed = DataController.Instance.inGameWeapons.BulletSpeed;
        CriticalChance = DataController.Instance.InGameBaseData.Critical;
        QuickHandChance = DataController.Instance.InGameBaseData.QuickHand;
    }
    public void SetDataWeaPon(float _FireRate)
    {
        ATKspeed +=  (_FireRate* ATKspeed/100);
    }
    public void SetDataWeaPon(float _damage,float _FireRate, int _critical)
    {
        ATK = ATK + (_damage * ATK/100);
        ATKspeed = ATKspeed + (_FireRate * ATKspeed / 100);
        CriticalChance = _critical;
    }
    // Update is called once per frame
    private void Update()
    {
        UpdateEnemy();
        if (preCharacterState != characterState)
        {
            ChangeState();
            preCharacterState = characterState;
        }
        // if (coundown < 0)
        //  {
        switch (currentMode)
        {
            case AutoMode.TurnOff:
                //characterState = CharacterState.Idle;
                

                if (Input.GetMouseButton(0) /*&& !EventSystem.current.IsPointerOverGameObject()*/
                    && (Camera.main.ScreenToWorldPoint(Input.mousePosition).y > -5.5f))
                {
                    direct = Camera.main.ScreenToWorldPoint(Input.mousePosition) - Barrel.transform.position;
                    rotationZ = Mathf.Atan2(direct.y, direct.x) * Mathf.Rad2Deg;
                    characterState = CharacterState.Attack;
                }
                else
                {
                    if(!idleStatus)
                        idleStatus = true;
                }
                break;
            case AutoMode.TurnOn:
                if (player.target != null)
                {
                    characterState = CharacterState.Attack;
                    direct = player.target.gameObject.transform.position - Barrel.transform.position;
                    rotationZ = Mathf.Atan2(direct.y, direct.x) * Mathf.Rad2Deg;
                }
                else
                {
                    characterState = CharacterState.Idle;
                }
                break;
        }

    }
    //public void Shoot()
    //{
    //    //SetDatabullet(direct, rotationZ, player.Bullet);
    //}

    private void OnEvent(TrackEntry trackEntry, Spine.Event e)
    {
        if (e.Data.Name.Equals(eventName))
        {
            Shoot(CriticalChance,KnockBackChance,QuickHandChance,MultiShotChance);
        }
    }
    private void ChangeState()
    {
        if (characterState.Equals(CharacterState.Attack))
        {
            skeletonAnimation.AnimationState.SetAnimation(0, attack, true);
            skeletonAnimation.timeScale = ATKspeed / 100;
        }
        else if (characterState.Equals(CharacterState.Idle))
        {
            skeletonAnimation.timeScale = 1;
            skeletonAnimation.AnimationState.SetAnimation(0, idle, true);
        }
    }

    private void OnAttack(TrackEntry trackEntry)
    {
        if (idleStatus)
        {
            idleStatus = false;
            characterState = CharacterState.Idle;
        }
    }

    public void UpdateEnemy()
    {
        if (listEnemies.Count > 0)
        {
            int index = 0;
            listEnemies = listEnemies.OrderBy(obj => (obj.transform.position - transform.position).magnitude).ToList();
            nearestEnemy = listEnemies[index];
            if (!nearestEnemy.isLive && listEnemies.Count>1)
            {
                nearestEnemy = listEnemies[index + 1];
            }
            if (nearestEnemy != null && nearestEnemy.isLive)
            {
                player.target = nearestEnemy;
            }
            else
            {
                player.target = null;
                characterState = CharacterState.Idle;
            }
        }
        else if (listEnemies.Count <= 0 && currentMode == AutoMode.TurnOn)
        {
            characterState = CharacterState.Idle;
        }
    }
    public BulletController SpawnBullet(Vector2 _direction, float _rotatioZ, string _bullet)
    {
        ViewPlayer.SetPositionBone(_direction);
        BulletController mBullet = ObjectPoolManager.Instance.SpawnObject(_bullet, Barrel.transform.position, Quaternion.Euler(0, 0, _rotatioZ - 90)).GetComponent<BulletController>();
        mBullet.SetTarget(player.target);
        mBullet.setDirection(_direction);
        mBullet.elementalBullet = elementalType;
        mBullet.SetDataBullet(BulletSpeed, ATK);
        return mBullet;
    }
    public void Shoot(float _critical,float _knockback,float _quickhand,bool _multiShot)
    {
        BulletController mBullet= SpawnBullet(direct, rotationZ, player.Bullet);    
        //random Knockback
        float RandomKnockBack = UnityEngine.Random.Range(0, 100);
        if(RandomKnockBack < _knockback)
        {
            mBullet.SetKnockBack(2.5f);
        }

        //Random Critical
        float RandomCritical = UnityEngine.Random.Range(0, 100);
        if (RandomCritical < _critical)
        {
            mBullet.SetDataBullet(BulletSpeed, 2 * ATK);
        }
        // Multishot
        if (_multiShot)
        {
            TwoMoreBullet();
        }
        //Random QuickShot
        float RandomQuickHand = UnityEngine.Random.Range(0, 100);
        if (RandomQuickHand < _quickhand &&  _multiShot)
        {
            StartCoroutine(IEQuickHand(0.3f, _multiShot));
        }
        else if (RandomQuickHand < _quickhand && !_multiShot)
        {
            StartCoroutine( IEQuickHand(0.3f, _multiShot));
        }
    }
    public void TwoMoreBullet()
    {
        //float Ax = direct.x * Mathf.Cos(5) - direct.y* Mathf.Sin(5);
        //float Ay = direct.x * Mathf.Sin(5) + direct.y * Mathf.Cos(5);
        //Vector2 newDirection = new Vector3(Ax, Ay)- Barrel.transform.position;
        Vector2 newDirection=Quaternion.AngleAxis(5,direct).eulerAngles;
        BulletController mBullet1= SpawnBullet(newDirection, rotationZ , player.Bullet);
        mBullet1.SetDataBullet(BulletSpeed, ATK * MultiShotDamage / 100);
        //BulletController mBullet2 = SpawnBullet(newDirection1, rotationZ , player.Bullet);
        //mBullet2.SetDataBullet(BulletSpeed, ATK * MultiShotDamage / 100);
    }
    public IEnumerator IEQuickHand(float _time,bool Multi)
    {
        yield return new WaitForSeconds(_time);
        if (!Multi)
        {
            SpawnBullet(direct, rotationZ, player.Bullet);
        }
        else
        {
            Shoot(CriticalChance * ReduceMultiShotPercent, KnockBackChance* ReduceMultiShotPercent,
                QuickHandChance* ReduceMultiShotPercent, MultiShotChance);
            TwoMoreBullet();
        }
    }
    private float ReduceMultiShotPercent
    {
        get{return MultiShotAddedAttributePercent / 100;}
    }
    private void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.gameObject.tag.Equals("Enemy"))
        {
            listEnemies.Add(collider2D.gameObject.GetComponent<EnemyController>());
        }
    }
}

