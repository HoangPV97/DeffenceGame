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
    public float ATK;
    public float ATKspeed;
    public float BulletSpeed;
    public float CriticalRatio;
    public float CriticalDamage;
    public CircleCollider2D CircleCollider2D;
    bool idleStatus;
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
        ATK = DataController.Instance.inGameWeapons.ATK;
        ATKspeed = DataController.Instance.inGameWeapons.ATKspeed;
        BulletSpeed = DataController.Instance.inGameWeapons.BulletSpeed;
    }
    public void SetDataWeaPon(float _FireRate)
    {
        ATKspeed +=  (_FireRate* ATKspeed/100);
    }
    public void SetDataWeaPon(float _damage,float _FireRate, float _critical)
    {
        ATK = ATK + (_damage * ATK/100);
        ATKspeed = ATKspeed + (_FireRate * ATKspeed / 100);
        CriticalRatio = _critical;
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
                

                if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject()
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
                    //AutoShootTarget();
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
    public void Shoot()
    {
        ShootToDirection(direct, rotationZ, "tankbullet");
    }

    private void OnEvent(TrackEntry trackEntry, Spine.Event e)
    {
        if (e.Data.Name.Equals(eventName))
        {
            Shoot();
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
            if (!nearestEnemy.isLive)
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

    public void ShootToDirection(Vector2 _direction, float _rotatioZ, string _bullet)
    {
        ViewPlayer.SetPositionBone(direct);
        GameObject bullet = ObjectPoolManager.Instance.SpawnObject(_bullet, Barrel.transform.position, Quaternion.identity);
        bullet.transform.rotation = Quaternion.Euler(0, 0, _rotatioZ - 90);
        BulletController mBullet = bullet.GetComponent<BulletController>();
        mBullet.SetTarget(player.target);
        mBullet.setDirection(_direction);
        mBullet.SetDataBullet(BulletSpeed, ATK, CriticalRatio,CriticalDamage);
        mBullet.elementalBullet = elementalType;
    }
    private void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.gameObject.tag.Equals("Enemy"))
        {
            listEnemies.Add(collider2D.gameObject.GetComponent<EnemyController>());
        }
    }
}

