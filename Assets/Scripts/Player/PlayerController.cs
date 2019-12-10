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
    public List<EnemyController> listEnemies;

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
    public float ATK;
    public float ATKspeed;
    public CircleCollider2D CircleCollider2D;
    // Start is called before the first frame update
    public void Awake()
    {
        CircleCollider2D.radius = player.range;
    }
    void Start()
    {
        skeletonAnimation.AnimationState.Event += OnEvent;
        currentMode = AutoMode.TurnOff;
    }

    public void SetDataWeapon()
    {
        this.elementalType = DataController.Instance.inGameWeapons.Type;
        ATK = DataController.Instance.inGameWeapons.ATK;
        ATKspeed = DataController.Instance.inGameWeapons.ATKspeed;
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
                    if (Input.GetMouseButton(0) /*&& !EventSystem.current.IsPointerOverGameObject()*/
                        && (Camera.main.ScreenToWorldPoint(Input.mousePosition).y > -5.5f))
                    {
                        //ClicktoShoot();
                        direct = Camera.main.ScreenToWorldPoint(Input.mousePosition) - Barrel.transform.position;
                        rotationZ = Mathf.Atan2(direct.y, direct.x) * Mathf.Rad2Deg;
                        characterState = CharacterState.Attack;
                    }
                    else
                    {
                        characterState = CharacterState.Idle;
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
        if (listEnemies.Count > 0)
        {
            for (int i = 0; i < listEnemies.Count; i++)
            {
                float distancetoEnemy = Vector3.Distance(transform.position, listEnemies[i].transform.position);
                if (distancetoEnemy < shortestDistance)
                {
                    _2ndShortestDistance = shortestDistance;
                    shortestDistance = distancetoEnemy;
                    _2ndEnemy = nearestEnemy;
                    nearestEnemy = listEnemies[i];
                }
                else if (distancetoEnemy < _2ndShortestDistance && distancetoEnemy != shortestDistance)
                {
                    _2ndShortestDistance = distancetoEnemy;
                    _2ndEnemy = listEnemies[i];
                }
                if (nearestEnemy != null  && nearestEnemy.isLive)
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
                    characterState = CharacterState.Idle;
                }
            }
        }
        else if(listEnemies.Count <=0 && currentMode== AutoMode.TurnOn)
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
        mBullet.SetDataBullet(ATKspeed, ATK);
        mBullet.SetTarget(player.target);
        mBullet.elementalBullet = elementalType;
        mBullet.GetComponent<Rigidbody2D>().velocity = _direction.normalized *10* mBullet.bullet.Speed * Time.deltaTime;
    }
    private void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.gameObject.tag.Equals("Enemy"))
        {
            listEnemies.Add(collider2D.gameObject.GetComponent<EnemyController>());
        }
    }
}

