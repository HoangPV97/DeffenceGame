using Spine;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Spine.AnimationState;

public class Enemies
{
    public static List<EnemyController> listEnemies = new List<EnemyController>();
}
public enum EnemyState
{
    Idle, Run, Attack, Hurt, Die, Skill
}
public class EnemyController : MonoBehaviour
{
    public Enemy enemy;
    protected EnemyState previousState, CurrentState;
    public PlaySkeletonAnimationState playSkeletonAnimation;
    public bool isMove = true, isAttack = true, isLive = true;
    bool isHurt, isIdle;
    protected GameObject Tower;
    PlayerController playerController;
    protected float distancetoTower;
    protected float countdown;
    public static float EnemyLive;
    SoundManager soundManager;
    float distance;
    GameEffect gameEffect;
    GameObject effectObj ;
    public Rigidbody2D Rigidbody2D;
    
    [SerializeField] GameObject HealthUI;
    [SerializeField] BoxCollider2D boxCollider2D;
    // Start is called before the first frame update
    private void Awake()
    {
        Enemies.listEnemies = new List<EnemyController>();
        soundManager = SoundManager.Instance;

    }
    protected void Start()
    {
        countdown = enemy.rateOfFire;
        playerController = FindObjectOfType<PlayerController>();
        gameEffect = GetComponent<GameEffect>();
        enemy.health.Init();
        SeekingTower();
        distance = Vector3.Distance(transform.position, Tower.transform.position);
        Move();
    }

    // Update is called once per frame
    protected void Update()
    {

        if (previousState != CurrentState)
        {
            ChangeState();
            previousState = CurrentState;
        }
        if (enemy.health.CurrentHealth <= 0)
        {
            StartCoroutine(Die());
        }
    }
    //Find player with tag="Player"
    protected void SeekingTower()
    {
        Tower = GameObject.FindGameObjectWithTag("Tower");
    }
    /// <summary>
    /// Move Enemy with velocity= distance/time
    /// </summary>
    public void Move()
    {
        if (isMove)
        {
            CurrentState = EnemyState.Run;
            Rigidbody2D.velocity = Vector2.down * (distance / enemy.speed);
        }
        else
        {
            Rigidbody2D.velocity = Vector2.zero;
            CurrentState = EnemyState.Idle;
        }

    }
    IEnumerator Die()
    {

        isLive = false;
        isAttack = false;
        CurrentState = EnemyState.Die;
        Rigidbody2D.velocity = Vector2.zero;
        HealthUI.SetActive(false);
        boxCollider2D.enabled = false;
        if(effectObj!=null && effectObj.activeSelf)
        {
            effectObj.SetActive(false);
        }
        yield return new WaitForSeconds(1);
        Enemies.listEnemies.Remove(this);
        EnemyLive--;
        gameObject.SetActive(false);
    }

    //public void SpawnGold()
    //{
    //    GameObject _gold = PoolManager.SpawnObject("gold", transform.position, Quaternion.identity);
    //    soundManager.PlayClipOneShot(soundManager.Coin);
    //    _gold.GetComponent<Gold>().Price = enemy.price;
    //}
    public void DealDamge(float _damage, float _damageplus)
    {
        isHurt = true;
        //CurrentState = EnemyState.Hurt;
        SpawnDamageText("damage", gameObject.transform.position, _damage);
        if (_damageplus > 0)
        {
            SpawnDamageText("elementaldamage", gameObject.transform.position + new Vector3(0, 0.2f, 0), _damageplus);
        }
        enemy.health.ReduceHealth(_damage+_damageplus);

    }
    private void SpawnDamageText(string tag, Vector2 _postion, float _damage)
    {
        GameObject damageobj = ObjectPoolManager.Instance.SpawnObject(tag, _postion, Quaternion.identity);
        damageobj.transform.parent = GetComponentInChildren<Canvas>().gameObject.transform;
        damageobj.GetComponent<LoadingText>().SetTextDamage(_damage.ToString());
    }
    public void DealEffect(Effect _effect, Vector3 _position, float _time)
    {
        
        StartCoroutine(WaitingEffect(_time, () =>
       {
           if (_effect.Equals(Effect.Slow))
           {
               gameEffect.KnockBack(gameObject, _position);
               if (effectObj != null)
               {
                   gameEffect.KnockBack(effectObj, _position);
                   return;
               }
           }
           else if (_effect.Equals(Effect.Stun) || _effect.Equals(Effect.Freeze))
           {
               isMove = false;
               Move();
               isAttack = false;
               effectObj = gameEffect.GetEffect(_effect, _position, _time);
           }
       }, () =>
       {

           isMove = true;
           if (_effect.Equals(Effect.Freeze))
           {
               gameEffect.GetEffect(Effect.destroyFreeze, _position, _time);
           }
           if (!isAttack)
           {
               Move();
           }
           isAttack = true;
       }));
    }
    public void KnockBack(Vector3 _position)
    {
        Rigidbody2D.AddForce(new Vector2(gameObject.transform.position.x * 1, gameObject.transform.position.x * -1));
    }
    IEnumerator WaitingEffect(float _time, Action _action1, Action _action2)
    {
        _action1.Invoke();
        yield return new WaitForSeconds(_time);
        _action2.Invoke();
    }
    protected IEnumerator WaitingDestroyEffect(GameObject _gameObject, float _time)
    {
        yield return new WaitForSeconds(_time);
        _gameObject?.SetActive(false);
    }
    public void ChangeState()
    {

        string stateName = null;
        switch (CurrentState)
        {
            case EnemyState.Attack:
                stateName = "attack";
                break;
            case EnemyState.Die:
                stateName = "die";
                break;
            case EnemyState.Hurt:
                stateName = "hurt";
                break;
            case EnemyState.Idle:
                stateName = "idle";
                break;
            case EnemyState.Run:
                stateName = "run";
                break;
            case EnemyState.Skill:
                stateName = "skill";
                break;
            default:
                break;
        }
        playSkeletonAnimation.PlayAnimationState(stateName);
    }
    private void OnTriggerEnter2D(Collider2D _tower)
    {
        if (_tower.gameObject.tag.Equals("Tower"))
        {
            isMove = false;
            Move();
            isAttack = true;
            CurrentState = EnemyState.Attack;
        }
    }
    private void OnEnable()
    {
        HealthUI.SetActive(true);
        boxCollider2D.enabled = true;
    }
    public void OnTriggerExit2D(Collider2D BlockPoint)
    {
        if (BlockPoint.gameObject.tag.Equals("BlockPoint"))
        {
            Enemies.listEnemies.Add(this);
        }
    }
}
