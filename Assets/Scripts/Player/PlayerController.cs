using Spine;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    protected ObjectPoolManager poolManager;
    public Elemental elementalType;
    public CharacterState characterState, preCharacterState;
    public SkeletonAnimation skeletonAnimation;
    public AnimationReferenceAsset attack, idle;
    [SpineEvent(dataField: "skeletonAnimation", fallbackToTextField: true)]
    public string eventName;
    Vector2 direct;
    // Start is called before the first frame update
    private void Awake()
    {

    }
    void Start()
    {
        skeletonAnimation.AnimationState.Event += OnEvent;
        poolManager = ObjectPoolManager.Instance;
        currentMode = AutoMode.TurnOff;

    }
    // Update is called once per frame
    private void Update()
    {

        UpdateEnemy();
        CheckonShoot();
        if (player.target == null)
        {
            return;
        }
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
                    if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
                    {
                        // ViewPlayer.SetPositionBone(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                        ClicktoShoot();
                    }
                    break;
                case AutoMode.TurnOn:
                    if (player.target != null)
                    {
                        // ViewPlayer.SetPositionBone(player.target.position);
                        AutoShootTarget();
                    }
                    break;
            }
        }
        coundown -= Time.deltaTime;
    }
    public void AutoShoot()
    {

    }
    public void ClicktoShoot()
    {
        characterState = CharacterState.Attack;
        Vector2 direct = Camera.main.ScreenToWorldPoint(Input.mousePosition) - Barrel.transform.position;
        float rotationZ = Mathf.Atan2(direct.y, direct.x) * Mathf.Rad2Deg;

        ShootToDirection(direct, rotationZ, "tankbullet");
        coundown = player.rateOfFire;
    }
    public void AutoShootTarget()
    {
        characterState = CharacterState.Attack;
        Vector2 direct = player.target.position - Barrel.transform.position;
        float rotationZ = Mathf.Atan2(direct.y, direct.x) * Mathf.Rad2Deg;
        //transform.rotation = Quaternion.Euler(0, 0, rotationZ - 90);
        ShootToDirection(direct, rotationZ, "tankbullet");
        coundown = player.rateOfFire;
    }
    public void Shoot()
    {
        switch (currentMode)
        {
            case AutoMode.TurnOff:
                if (Input.GetMouseButtonDown(0))
                {
                    ClicktoShoot();
                }
                break;
            case AutoMode.TurnOn:
                if (player.target != null)
                {
                    AutoShootTarget();
                }
                break;
        }

    }
    void CheckonShoot()
    {
        if (Input.GetMouseButtonDown(0) && currentMode == AutoMode.TurnOff)
        {
            characterState = CharacterState.Attack;
        }

    }
    private void OnEvent(TrackEntry trackEntry, Spine.Event e)
    {
        Debug.Log(trackEntry);
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
    protected void UpdateEnemy()
    {
        GameObject[] Enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;
        foreach (GameObject Enemy in Enemies)
        {
            float distancetoEnemy = Vector3.Distance(transform.position, Enemy.transform.position);
            if (distancetoEnemy < shortestDistance)
            {
                shortestDistance = distancetoEnemy;
                nearestEnemy = Enemy;

            }
        }
        if (nearestEnemy != null && shortestDistance < player.range)
        {
            player.target = nearestEnemy.transform;
        }
        else
        {
            characterState = CharacterState.Idle;
            player.target = null;
        }
    }

    public void ShootToDirection(Vector2 _direction, float _rotatioZ, string _bullet)
    {
        characterState = CharacterState.Attack;
        GameObject bullet = poolManager.SpawnObject(_bullet, Barrel.transform.position, Quaternion.identity);
        bullet.transform.rotation = Quaternion.Euler(0, 0, _rotatioZ - 90);
        WindPlayerBullet mBullet = bullet.GetComponent<WindPlayerBullet>();
        mBullet.elementalBullet = elementalType;
        mBullet.DirectShooting(_direction);
    }

}
