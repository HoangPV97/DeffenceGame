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
public class Enemy : MonoBehaviour
{

    public EnemyState previousState, CurrentState;
    public PlaySkeletonAnimationState playSkeletonAnimation;
    public Health Health;
    public float Speed, Damge, range, Price, Armor;
    public bool isMove = true ,isAttack, isLive = true;
    bool  isHurt, isDie, isIdle;
    protected GameObject Player;
    protected float distancetoPlayer;
    public float RateOfFire;
    protected float countdown;
    public static float EnemyLive;
    //public ParticleSystem particleSystem;
    protected ObjectPoolManager PoolManager;
    SoundManager soundManager;
    float distance;
    public Elemental elementalEnemy;
    GameEffect gameEffect;
    public Rigidbody2D Rigidbody2D;
    // Start is called before the first frame update
    private void Awake()
    {
        PoolManager = ObjectPoolManager.Instance;
        soundManager = SoundManager.Instance;

    }
    protected void Start()
    {
        countdown = RateOfFire;
        gameEffect = GetComponent<GameEffect>();
        Health.CurrentHealth = Health.health;
        SeekingPlayer();
        distance = Vector3.Distance(transform.position, Player.transform.position);
        Move();
        PlaySkeletonAnimationState.eventBacktoRun += BackToRunState;
    }

    // Update is called once per frame
    protected void Update()
    {

        if (previousState != CurrentState)
        {
            ChangeState();
            previousState = CurrentState;
        }
    }
    /// <summary>
    /// Speed up for enemy
    /// </summary>
    /// <param name="_Speed"> Speed bonus</param>
    public void SpeedUpEnemy(float _Speed)
    {
        Speed += _Speed;
    }
    //Find player with tag="Player"
    protected void SeekingPlayer()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }
    /// <summary>
    /// Move Enemy with velocity= distance/time
    /// </summary>
    public void Move()
    {
        if (isMove)
        {
            Rigidbody2D.velocity = Vector2.down * (distance / Speed);
        }
        else
        {
            Rigidbody2D.velocity = Vector2.zero;
        }
        CurrentState = (Rigidbody2D.velocity.magnitude > 0) ? CurrentState = EnemyState.Run : CurrentState = EnemyState.Idle;
    }
    IEnumerator Die()
    {
        CurrentState = EnemyState.Die;
        yield return new WaitForSeconds(1);
        EnemyLive--;
        SpawnGold();
        gameObject.SetActive(false);
    }

    public void SpawnGold()
    {
        GameObject _gold = PoolManager.SpawnObject("gold", transform.position, Quaternion.identity);
        soundManager.PlayClipOneShot(soundManager.Coin);
        _gold.GetComponent<Gold>().Price = Price;
    }
    public void TakeDamage(Elemental _elementalType, float _damage, float _damagePlus)
    {

        float DamagePlus = 0;
        switch (elementalEnemy)
        {
            case Elemental.Fire:
                if (_elementalType.Equals(Elemental.Wind))
                {
                    DamagePlus = _damagePlus;
                }
                break;
            case Elemental.Ice:
                if (_elementalType.Equals(Elemental.Fire))
                {
                    DamagePlus = _damagePlus;
                }

                break;
            case Elemental.Wind:
                if (_elementalType.Equals(Elemental.Ice))
                {
                    DamagePlus = _damagePlus;
                }
                break;
        }
        GameObject bulletImpact = gameEffect.GetElementalEffect(_elementalType, gameObject.transform.position);
        bulletImpact.transform.parent = gameObject.transform;
        DealDamge(_damage, DamagePlus);
    }
    protected void DealDamge(float _damage, float _damageplus)
    {
        isHurt = true;
        CurrentState = EnemyState.Hurt;
        SpawnDamageText("damage", gameObject.transform.position, _damage);
        if (_damageplus > 0)
        {
            SpawnDamageText("elementaldamage", gameObject.transform.position + new Vector3(0, 0.2f, 0), _damageplus);
        }
        Health.CurrentHealth -= _damage + _damageplus;
        Health.healthBar.fillAmount = Health.CurrentHealth / Health.health;
        if (Health.CurrentHealth <= 0)
        {
            StartCoroutine(Die());
        }
    }
    private void SpawnDamageText(string tag, Vector2 _postion, float _damage)
    {
        GameObject damageobj = PoolManager.SpawnObject(tag, _postion, Quaternion.identity);
        damageobj.transform.parent = GetComponentInChildren<Canvas>().gameObject.transform;
        damageobj.GetComponent<LoadingText>().SetTextDamage(_damage.ToString());
    }
    public void TakeEffect(Effect _effect, float _time)
    {
        if (gameObject.activeSelf)
        {
            StartCoroutine(WaitingEffect(_time, () =>
           {
               gameEffect.SetEffect(_effect, true, _time);
               isMove = false;
               isAttack = false;
               Move();
           }, () =>
           {
               gameEffect.SetEffect(_effect, false);
               isMove = true;
               if (!isAttack)
               {
                   Move();
               }
               
           }));
        }
    }
    public void KnockBack(float _backSpace)
    {
        transform.position += new Vector3(0, _backSpace * 0.1f, 0);
    }
    IEnumerator WaitingEffect(float _time, Action _action1, Action _action2)
    {
        _action1.Invoke();
        yield return new WaitForSeconds(_time);
        _action2.Invoke();
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
                if (isMove)
                {
                    Invoke("BackToRunState", playSkeletonAnimation.GetAnimationStateInList(stateName).Duration);
                }
                if (isAttack)
                {
                    Invoke("BackToAttackState", playSkeletonAnimation.GetAnimationStateInList(stateName).Duration);
                }

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

    public void BackToRunState()
    {
            CurrentState = EnemyState.Run;
    }
    public void BackToAttackState()
    {
        CurrentState = EnemyState.Attack;
    }
    private void OnTriggerEnter2D(Collider2D _player)
    {
        if (_player.gameObject.tag.Equals("Player"))
        {
            isAttack = true;
            CurrentState = EnemyState.Attack;
            isMove = false;
            Move();
        }
    }
}
