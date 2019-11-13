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
    private float currentHealth;
    public float Speed, Damge, range, Price, Health, Armor;
    bool isMove = true,isLive=true,isAttack,isHurt,isDie,isIdle;
    public Image healthBar;
    protected GameObject Player;
    protected float distancetoPlayer;
    public float RateOfFire ;
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
        Rigidbody2D = GetComponent<Rigidbody2D>();
        gameEffect = GetComponent<GameEffect>();
        currentHealth = Health;
        SeekingPlayer();
        distance = Vector3.Distance(transform.position, Player.transform.position);
        Move();
        PlaySkeletonAnimationState.eventBacktoRun += BackToRunState;
    }

    // Update is called once per frame
    protected void Update()
    {
        bool stateChanged = previousState != CurrentState;
        //previousState = CurrentState;
        if (stateChanged)
        {
            ChangeState();
            previousState = CurrentState;
        }
        
        
        Debug.Log("Previous : " + previousState);
        Debug.Log("Current : " + CurrentState);
       
        
    }
    private void FixedUpdate()
    {
               
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

    //
    public void Attack( )
    {
        
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
        DealDamge(_damage, DamagePlus);
    }
    protected void DealDamge(float _damage, float _damageplus)
    {
        CurrentState = EnemyState.Hurt;
        SpawnDamageText("damage", gameObject.transform.position, _damage);
        if (_damageplus > 0)
        {
            SpawnDamageText("elementaldamage", gameObject.transform.position + new Vector3(0, 0.2f, 0), _damageplus);
        }
        currentHealth -= _damage + _damageplus;
        healthBar.fillAmount = currentHealth / Health;
        //if (currentHealth < (Health / 2))
        //{
        //    particleSystem.gameObject.SetActive(true);
        //}

        if (currentHealth <= 0)
        {
           StartCoroutine( Die());
        }
    }
    private void SpawnDamageText(string tag, Vector2 _postion, float _damage)
    {
        GameObject damageobj = PoolManager.SpawnObject(tag, _postion, Quaternion.identity);
        damageobj.transform.parent = GetComponentInChildren<Canvas>().gameObject.transform;
        damageobj.GetComponent<LoadingText>().SetTextDamage(_damage.ToString());
    }
    public void TakeEffect(Effect _effect,float _time)
    {
        if (gameObject.activeSelf)
        {
            StartCoroutine(WaitingEffect(_time, () =>
           {
               gameEffect.SetEffect(_effect, true, _time);
               isMove = false;
           }, () =>
           {
               gameEffect.SetEffect(_effect, false);
               isMove = true;
           }));
        }
    }
    public void KnockBack(float _backSpace)
    {
        transform.position += new Vector3(0, _backSpace *0.1f, 0);
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
                Invoke("BackToRunState", playSkeletonAnimation.GetAnimationStateInList(stateName).Duration);
                
                break;
            case EnemyState.Die:
                stateName = "die";
                break;
            case EnemyState.Hurt:
                stateName = "hurt";
                Invoke("BackToRunState", playSkeletonAnimation.GetAnimationStateInList(stateName).Duration);
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

    public void  BackToRunState()
    {
        CurrentState = EnemyState.Run;
    }
}
