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
    public AnimationReferenceAsset attack, attack2, idle;
    [SpineEvent(dataField: "skeletonAnimation", fallbackToTextField: true)]
    public string eventName;
    Vector2 direct;
    float rotationZ;
    EnemyController nearestEnemy = null;
    public CircleCollider2D CircleCollider2D;
    bool idleStatus;
    [Header("Weapon Data")]
    public int ATK;
    public float ATKspeed;
    public float BulletSpeed;
    public float ATKplus;
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
    private bool quickShoot;
    public bool Poison;
    public int PoisonDamage;
    [SerializeField] private bool ChangeAttackHand = true;
    // Start is called before the first frame update
    public void Awake()
    {
        CircleCollider2D.radius = player.range;
    }
    protected virtual void Start()
    {
        skeletonAnimation.AnimationState.Event += OnEvent;
        skeletonAnimation.AnimationState.Complete += OnAttack;
        skeletonAnimation.AnimationState.Event += ChangeHand;
        currentMode = AutoMode.TurnOff;
    }

    public virtual void SetDataWeapon()
    {
        var IngameWeapon = DataController.Instance.inGameWeapons;
        var InGameBaseData = DataController.Instance.InGameBaseData;
        this.elementalType = IngameWeapon.Type;
        ATK = Mathf.RoundToInt(IngameWeapon.ATK);
        ATKspeed = IngameWeapon.ATKspeed;
        ATKplus = IngameWeapon.ATKplus;
        BulletSpeed = IngameWeapon.BulletSpeed;
        CriticalChance = InGameBaseData.Critical;
        QuickHandChance = InGameBaseData.QuickHand;
    }
    public void SetFireRateWeaPon(float _FireRate)
    {
        ATKspeed += (_FireRate * ATKspeed / 100);
    }
    public void SetDamageWeaPon(float _ATK)
    {
        ATK += Mathf.RoundToInt(_ATK * ATK / 100);
    }
    public void SetCriticalWeaPon(float _critical)
    {
        CriticalChance += (int)(_critical * CriticalChance / 100);
    }
    public void SetDataWeaPon(int _damage, float _FireRate)
    {
        ATK = ATK + (_damage * ATK / 100);
        ATKspeed = ATKspeed + (_FireRate * ATKspeed / 100);
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

        switch (currentMode)
        {
            case AutoMode.TurnOff:
                if (Input.GetMouseButton(0) /*&& !EventSystem.current.IsPointerOverGameObject()*/
                    && (Camera.main.ScreenToWorldPoint(Input.mousePosition).y > -5.5f))
                {
                    characterState = CharacterState.Attack;
                }
                else
                {
                    if (!idleStatus)
                        idleStatus = true;
                }
                break;
            case AutoMode.TurnOn:
                if (player.target != null)
                {
                    characterState = CharacterState.Attack;
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
        if (e.Data.Name.Equals(eventName) && !quickShoot)
        {
            Shoot(CriticalChance, KnockBackChance, QuickHandChance, MultiShotChance);
        }
    }
    void GetAnimAttack()
    {
        if (attack2 != null && ChangeAttackHand)
        {
            skeletonAnimation.AnimationState.SetAnimation(0, attack2, true);
        }
        else
            skeletonAnimation.AnimationState.SetAnimation(0, attack, true);
        skeletonAnimation.timeScale = ATKspeed / 100;
    }
    private void ChangeState()
    {
        if (characterState.Equals(CharacterState.Attack))
        {
            GetAnimAttack();
        }
        else if (characterState.Equals(CharacterState.Idle))
        {
            skeletonAnimation.timeScale = 1;
            skeletonAnimation.AnimationState.SetAnimation(0, idle, true);
        }
    }
    private void ChangeHand(TrackEntry trackEntry, Spine.Event e)
    {
        if (e.Data.Name.Equals(eventName))
        {
            ChangeAttackHand = !ChangeAttackHand;
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
            if (!nearestEnemy.isLive && listEnemies.Count > 1)
            {
                nearestEnemy = listEnemies[index + 1];
            }
            if (nearestEnemy != null && nearestEnemy.isLive)
            {
                player.target = nearestEnemy;
            }
            else
            {
                //player.target = null;
                characterState = CharacterState.Idle;
            }
        }
        else if (listEnemies.Count <= 0 && currentMode == AutoMode.TurnOn)
        {
            player.target = null;
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
        mBullet.SetDataBullet(BulletSpeed, ATK, ATKplus);
        if (Poison)
        {
            mBullet.poison = true;
            mBullet.poisonDamage = PoisonDamage;
        }
        return mBullet;
    }
    public void Shoot(float _critical, float _knockback, float _quickhand, bool _multiShot)
    {
        if (currentMode == AutoMode.TurnOff && Camera.main.ScreenToWorldPoint(Input.mousePosition).y > -5.5f)
        {
            direct = Camera.main.ScreenToWorldPoint(Input.mousePosition) - Barrel.transform.position;
            rotationZ = Mathf.Atan2(direct.y, direct.x) * Mathf.Rad2Deg;
        }
        else
        {
            if (player.target != null)
            {
                direct = player.target.gameObject.transform.position - Barrel.transform.position;
                rotationZ = Mathf.Atan2(direct.y, direct.x) * Mathf.Rad2Deg;
            }
        }
        BulletController mBullet = SpawnBullet(direct, rotationZ, player.Bullet);
        //random Knockback
        float RandomKnockBack = UnityEngine.Random.Range(0, 100);
        if (RandomKnockBack < _knockback)
        {
            mBullet.SetKnockBack(2.5f);
        }

        //Random Critical
        float RandomCritical = UnityEngine.Random.Range(0, 100);
        if (RandomCritical < _critical)
        {
            mBullet.SetDataBullet(BulletSpeed, 2 * ATK, ATKplus);
        }
        // Multishot
        if (_multiShot)
        {
            TwoMoreBullet();
        }
        //Random QuickShot
        float RandomQuickHand = UnityEngine.Random.Range(0, 100);
        if (RandomQuickHand < _quickhand && _multiShot)
        {
            quickShoot = true;
            StartCoroutine(IEQuickHand(0.3f, _multiShot));
        }
        else if (RandomQuickHand < _quickhand && !_multiShot)
        {
            StartCoroutine(IEQuickHand(0.3f, _multiShot));
        }
    }
    public void TwoMoreBullet()
    {
        BulletController mBullet1 = SpawnBullet(Quaternion.Euler(0, 0, 5) * direct, rotationZ, player.Bullet);
        mBullet1.SetDataBullet(BulletSpeed, Mathf.RoundToInt(ATK * MultiShotDamage / 100), ATKplus);
        BulletController mBullet2 = SpawnBullet(Quaternion.Euler(0, 0, -5) * direct, rotationZ, player.Bullet);
        mBullet2.SetDataBullet(BulletSpeed, Mathf.RoundToInt(ATK * MultiShotDamage / 100), ATKplus);
    }
    public IEnumerator IEQuickHand(float _time, bool Multi)
    {
        quickShoot = true;
        skeletonAnimation.AnimationState.SetAnimation(0, "atk", false);
        yield return new WaitForSeconds(_time);
        if (!Multi)
        {
            SpawnBullet(direct, rotationZ, player.Bullet);
        }
        else
        {
            Shoot(CriticalChance * ReduceMultiShotPercent, KnockBackChance * ReduceMultiShotPercent,
                QuickHandChance * ReduceMultiShotPercent, MultiShotChance);
            TwoMoreBullet();
        }
        characterState = CharacterState.Idle;
        skeletonAnimation.timeScale = ATKspeed / 100;
        quickShoot = false;

    }
    private float ReduceMultiShotPercent
    {
        get { return MultiShotAddedAttributePercent / 100; }
    }
    private void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.gameObject.tag.Equals("Enemy"))
        {
            var enemyController = collider2D.GetComponent<EnemyController>();
            if (enemyController != null && !listEnemies.Contains(enemyController))
            {
                listEnemies.Add(enemyController);
            }
        }
    }
}

