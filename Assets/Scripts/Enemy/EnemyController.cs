using Spine;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Spine.AnimationState;

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
    protected float distancetoTower;
    protected float countdown;
    public static float EnemyLive;
    protected ObjectPoolManager PoolManager;
    SoundManager soundManager;
    float distance;
    GameEffect gameEffect;
    public Rigidbody2D Rigidbody2D;
    GameObject effect;
    [SerializeField] GameObject HealthUI;
    [SerializeField] BoxCollider2D boxCollider2D;
    // Start is called before the first frame update
    private void Awake()
    {
        PoolManager = ObjectPoolManager.Instance;
        soundManager = SoundManager.Instance;

    }
    protected void Start()
    {
        countdown = enemy.rateOfFire;
        gameEffect = GetComponent<GameEffect>();
        enemy.health.CurrentHealth = enemy.health.health;
        SeekingPlayer();
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
    protected void SeekingPlayer()
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
        if (effect != null && effect.activeSelf)
        {
            effect.SetActive(false);
        }
        yield return new WaitForSeconds(1);
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
        enemy.health.CurrentHealth -= _damage + _damageplus;
        enemy.health.healthBar.fillAmount = enemy.health.CurrentHealth / enemy.health.health;

    }
    private void SpawnDamageText(string tag, Vector2 _postion, float _damage)
    {
        GameObject damageobj = PoolManager.SpawnObject(tag, _postion, Quaternion.identity);
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
               if (effect != null)
               {
                   gameEffect.KnockBack(effect, _position);
               }
               return;
               }
           else if (_effect.Equals(Effect.Stun) || _effect.Equals(Effect.freeze))
           {    
               isMove = false;
               Move();
               isAttack = false;
               effect = gameEffect.GetEffect(_effect, _position, _time);
           }
       }, () =>
       {
           isMove = true;
           if (!isAttack)
           {
               Move();
           }
           isAttack = true;
       }));
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
}
