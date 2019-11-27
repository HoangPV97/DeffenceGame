using Spine;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public enum CharacterState { Idle, Attack };
public class PlayerController : MonoBehaviour
{
    public Player player;
    public enum AutoMode { TurnOn, TurnOff };
    public AutoMode currentMode;
    //public Elemental elementalPlayer;
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
    float shortestDistance = Mathf.Infinity;
    float _2ndShortestDistance = Mathf.Infinity;
    EnemyController nearestEnemy = null;
    EnemyController _2ndEnemy = null;
    public int ATK;
    public int ATKspeed;
    // Start is called before the first frame update
    void Start()
    {
        
        skeletonAnimation.AnimationState.Event += OnEvent;
        currentMode = AutoMode.TurnOff;  
    }
    public void SetDataWeapon()
    {
        ATK = WeaponsData.Instance.GetDataAtackWeapon(1, 1, elementalType);
        ATKspeed = WeaponsData.Instance.GetDataAtackWeapon(1, 1, elementalType);
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
        if (coundown < 0)
        {
            switch (currentMode)
            {
                case AutoMode.TurnOff:
                    characterState = CharacterState.Idle;
                    direct = Camera.main.ScreenToWorldPoint(Input.mousePosition) - Barrel.transform.position;
                    rotationZ = Mathf.Atan2(direct.y, direct.x) * Mathf.Rad2Deg;
                    if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject()
                        && (Camera.main.ScreenToWorldPoint(Input.mousePosition).y > -5.5f))
                    {
                        //ClicktoShoot();
                        characterState = CharacterState.Attack;
                        coundown = player.rateOfFire;
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
                    break;
            }
        }

        coundown -= Time.deltaTime;
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
                if (nearestEnemy != null && shortestDistance < player.range && nearestEnemy.isLive)
                {
                    player.target = nearestEnemy;
                }
                else if (_2ndEnemy != null && !nearestEnemy.isLive)
                {
                    nearestEnemy = _2ndEnemy;
                    player.target = nearestEnemy;
                }
                if (!nearestEnemy.isLive && _2ndEnemy == null)
                {
                    player.target = null;
                }
            }
        }
        else
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
        mBullet.SetDataBullet(ATK, ATKspeed);
        mBullet.elementalBullet = elementalType;
        mBullet.DirectShooting(_direction);
    }
    public void SlowSkill(Vector2 _direction, float _rotatioZ)
    {
        GameObject skill_1_player = ObjectPoolManager.Instance.SpawnObject(player.Bullet_Skill_1, gameObject.transform.position, Quaternion.identity);
        GameObject effectStart = ObjectPoolManager.Instance.SpawnObject(player.effectStart, this.transform.position, Quaternion.identity);
        if (!effectStart.GetComponent<DestroyEffect>())
        {
            effectStart.AddComponent<DestroyEffect>()._time = 0.3f;
        }
        skill_1_player.transform.rotation = Quaternion.Euler(0, 0, _rotatioZ);
        Rigidbody2D rigidbody = skill_1_player.GetComponent<Rigidbody2D>();
        float speed = skill_1_player.GetComponent<BulletController>().bullet.Speed;
        rigidbody.velocity = _direction.normalized * 40 * speed * Time.deltaTime;
    }
    
}

