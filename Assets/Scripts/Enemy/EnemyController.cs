using Spine;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Spine.AnimationState;
#region EnemyState
public enum EnemyState
{
    Idle, Run, Attack, Hurt, Die, Skill
}
#endregion
public class EnemyController : MonoBehaviour
{
    public Enemy enemy;
    protected EnemyState previousState, CurrentState;
    public SkeletonAnimation skeletonAnimation;
    public AnimationReferenceAsset attack, idle, run, skill, die;
    public bool isMove = true, isAttack, isLive = true, disableAttack;
    protected bool isKnockBack, isIdle;
    protected Tower Tower { get { return GameplayController.Instance.Tower; } }
    public float distancetoTower;
    protected float countdown;
    protected float distance;
    public GameEffect gameEffect;
    protected GameObject effectObj;
    public Rigidbody2D Rigidbody2D;
    public Canvas canvas;
    [SerializeField] BoxCollider2D boxCollider2D;
    protected Vector2 DirectionMove;
    protected bool Coroutine_running;
    public Vector3 KnockBackDistance;
    bool CheckLayerEnemy;
    MeshRenderer renderer;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        renderer = skeletonAnimation.GetComponent<MeshRenderer>();
        gameEffect = GetComponent<GameEffect>();
    }
    public void SetDirection(Vector2 _direction)
    {
        DirectionMove = _direction;
    }
    public virtual void SetUpdata(string type, int Level)
    {
        var md = DataController.Instance.GetMonsterData(type);
        float growth = 1 + md.Growth * (Level - 1);
        enemy.health.Init((int)(md.HP * growth), 0);
        enemy.damage = (int)(md.ATK * growth);
        enemy.armor = md.Armor * growth;
        enemy.speed = md.MoveSpeed;
        enemy.rateOfFire = md.ATKSpeed;
        enemy.bulletSpeed = md.BulletSpeed;
        enemy.range = md.Range;
        isMove = true;
        isLive = true;
        distance = Vector3.Distance(transform.position, Tower.transform.position);
        DirectionMove = Vector2.down;
        Move(enemy.speed);
    }
    // Update is called once per frame
    protected virtual void Update()
    {
        if (previousState != CurrentState)
        {
            ChangeState();
            previousState = CurrentState;
        }

        CheckAttack();
        if (isKnockBack)
        {  
            gameObject.transform.Translate(KnockBackDistance * Time.deltaTime);
            if (effectObj != null)
            {
                effectObj.transform.Translate(KnockBackDistance * Time.deltaTime);
            }
            if (isLive)
            {
                isMove = true;
            }
        }
    }
    public virtual void Move(float _speed, float percent_slow = 0)
    {
        if (!isAttack)
        {
            if (isMove)
            {
                CurrentState = EnemyState.Run;
                Rigidbody2D.velocity = DirectionMove * (distance / enemy.speed) * (1 - percent_slow / 100);
            }
            else
            {
                Rigidbody2D.velocity = Vector2.zero;
                CurrentState = EnemyState.Idle;
            }
        }
    }
    public virtual IEnumerator Die()
    {
        GameplayController.Instance.PlayerController.listEnemies.Remove(this);
        for(int i = 0; i < GameplayController.Instance.AllianceController.Count; i++)
        {
            GameplayController.Instance.AllianceController[i].listEnemies.Remove(this);
        }
        //GameplayController.Instance.Alliance_1?.listEnemies.Remove(this);
        //GameplayController.Instance.Alliance_2?.listEnemies.Remove(this);
        isLive = false;
        isAttack = false;
        isMove = false;
        CurrentState = EnemyState.Die;
        Rigidbody2D.velocity = Vector2.zero;
        canvas.gameObject.SetActive(false);
        boxCollider2D.enabled = false;
        if (effectObj != null)
            effectObj.GetComponent<ParticleSystem>().Stop();
        yield return new WaitForSeconds(1);
        if (effectObj != null)
        {
            Despawn(effectObj);
        }
       
        GameController.Instance.OnEnemyDie(1);
        Despawn(gameObject);
    }
    public virtual void DealDamge(int _damage, float _damageplus = 0f)
    {
        canvas.gameObject.SetActive(true);
        int TotalDamage = _damage + Mathf.RoundToInt(_damageplus);
        if (TotalDamage > enemy.health.CurrentHealth)
        {
            TotalDamage = (int)enemy.health.CurrentHealth ;
        }
        SpawnDamageText("DAMAGE", gameObject.transform.position, TotalDamage);
        //if (_damageplus > 0)
        //{
        //    SpawnDamageText("ELEMENT_DAMAGE", gameObject.transform.position + new Vector3(0.6f, 1f, 0), Mathf.RoundToInt(_damageplus));
        //}
        enemy.health.ReduceHealth(TotalDamage);
        if (enemy.health.CurrentHealth <= 0)
        {
            isMove = true;
            CurrentState = EnemyState.Idle;
            StartCoroutine(Die());
            gameEffect.SpawnEffect("DROP_COIN", this.transform.position, 1f);
            //return;
        }
        //Invoke("DisableCanvas", 2);
    }
    protected void SpawnDamageText(string tag, Vector2 _postion, float _damage)
    {
        GameObject damageobj = ObjectPoolManager.Instance.SpawnObject(tag, _postion, Quaternion.identity);
        damageobj.transform.SetParent(gameObject.transform);
        damageobj.GetComponent<LoadingText>().SetTextDamage(_damage.ToString());
    }
    public IEnumerator IsKnockback()
    {
        isKnockBack = true;
        
        yield return new WaitForSeconds(0.3f);
        isKnockBack = false;
        if (gameEffect.CurrentEffect == Effect.None)
        {
            Move(enemy.speed);
        }
        if (!isLive)
        {
            isMove = false;
            CurrentState = EnemyState.Die;
        }
    }
    public void KnockBack(float _backSpace)
    {
        KnockBackDistance = new Vector3(0, _backSpace, 0);
        isKnockBack = true;
        StartCoroutine(IsKnockback());
    }
    public virtual void Deal_Slow_Effect(float _time, float _slowdown_percent)
    {
        StartCoroutine(WaitingEffect(_time, () =>
        {
            Coroutine_running = true;

            Move(enemy.speed, _slowdown_percent);
            gameEffect.SetEffect(Effect.Slow);
        }, () =>
        {
            gameEffect.SetEffect(Effect.None);
            if (effectObj != null)
            {
                Despawn(effectObj);
                effectObj = null;
            }
            if (!isAttack && isMove)
            {
                Move(enemy.speed);
            }
            Coroutine_running = false;
        }));
    }
    public virtual void Restrict(Effect _effect, Vector3 _position, float _time)
    {
        if (isLive)
        {
            isMove = false;
            isAttack = false;
            Rigidbody2D.velocity = Vector2.zero;
            //Move(enemy.speed);
            CurrentState = EnemyState.Idle;
            skeletonAnimation.timeScale = 0;
            if (effectObj == null || _effect != gameEffect.CurrentEffect)
                effectObj = gameEffect.GetEffect(_effect, _position, _time);
        }
        else
        {
            CurrentState = EnemyState.Die;
        }
    }
    public virtual void DealEffect(Effect _effect, Vector3 _position, float _time)
    {
        if (!Coroutine_running || _effect != gameEffect.CurrentEffect)
        {
            StartCoroutine(WaitingEffect(_time, () =>
            {
                Coroutine_running = true;
                switch (_effect)
                {
                    case Effect.Stun:
                        Restrict(Effect.Stun, _position, _time);
                        break;
                    case Effect.StunBullet:
                        Restrict(Effect.StunBullet, _position, _time);
                        break;
                    case Effect.Freeze:
                        Restrict(Effect.Freeze, _position, _time);
                        break;
                    case Effect.Poiton:
                        if (Effect.Poiton != gameEffect.CurrentEffect)
                            effectObj = gameEffect.GetEffect(_effect, _position, _time);
                        break;
                }
                gameEffect.SetEffect(_effect);
            }, () =>
            {
                skeletonAnimation.timeScale = 1;
                gameEffect.SetEffect(Effect.None);
                if (effectObj != null)
                {
                    Despawn(effectObj);
                    effectObj = null;
                }
                if (isLive)
                {
                    isMove = true;
                    if (_effect.Equals(Effect.Freeze))
                    {
                        gameEffect.GetEffect(Effect.destroyFreeze, _position, _time);
                    }
                    if (!isAttack)
                    {
                        Move(enemy.speed);
                    }
                }
                else
                {
                    CurrentState = EnemyState.Die;
                }
                
                Coroutine_running = false;
            }));
        }
    }
    protected IEnumerator WaitingEffect(float _time, Action _action1, Action _action2)
    {
        _action1.Invoke();
        yield return new WaitForSeconds(_time);
        _action2.Invoke();
    }
    public void ChangeState()
    {
        switch (CurrentState)
        {
            case EnemyState.Attack:
                skeletonAnimation.AnimationState.SetAnimation(0, attack, true);
                skeletonAnimation.timeScale = enemy.rateOfFire / 100;
                break;
            case EnemyState.Die:
                skeletonAnimation.timeScale = 1;
                skeletonAnimation.AnimationState.SetAnimation(0, die, true);
                break;
            case EnemyState.Skill:
                skeletonAnimation.timeScale = 1;
                skeletonAnimation.AnimationState.SetAnimation(0, skill, true);
                break;
            case EnemyState.Idle:
                skeletonAnimation.timeScale = 1;
                skeletonAnimation.AnimationState.SetAnimation(0, idle, true);
                break;
            case EnemyState.Run:
                skeletonAnimation.timeScale = 1;
                skeletonAnimation.AnimationState.SetAnimation(0, run, true);
                break;
            default:
                break;
        }

    }
    private void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.gameObject.tag.Equals("Tower"))
        {
            isMove = false;
            isAttack = true;
            Rigidbody2D.velocity = Vector2.zero;
            CurrentState = EnemyState.Attack;
        }
    }
    public void OnTriggerStay2D(Collider2D collider2D)
    {
        if (collider2D.gameObject.tag.Equals("Enemy"))
        {
            if (gameObject.transform.position.y < collider2D.gameObject.transform.position.y && !CheckLayerEnemy)
            {
                renderer.sortingOrder = 1;
                collider2D.GetComponent<MeshRenderer>().sortingOrder = 0;
                CheckLayerEnemy = true;
            }
        }
    }
    public void OnTriggerExit2D(Collider2D collider2D)
    {
        if (collider2D.gameObject.tag.Equals("Enemy") && CheckLayerEnemy)
        {
            renderer.sortingOrder = 0;
            CheckLayerEnemy = false;
        }
    }
    private void OnEnable()
    {
        canvas.gameObject.SetActive(false);
        boxCollider2D.enabled = true;
    }
    public bool Check_Stun_Freeze()
    {
        if (gameEffect.CurrentEffect.Equals(Effect.Stun) || gameEffect.CurrentEffect.Equals(Effect.Freeze))
        {
            return true;
        }
        return false;
    }
    public void Despawn(GameObject _Obj)
    {
        ObjectPoolManager.Instance.DespawnObJect(_Obj);
    }

    public virtual void CheckAttack()
    {
        distancetoTower = Mathf.Abs(transform.position.y - Tower.transform.position.y);

        if ((distancetoTower <= 1.7f || distancetoTower <= enemy.range) && isLive)
        {
            isAttack = true;
            isMove = false;
            Rigidbody2D.velocity = Vector2.zero;
            CurrentState = EnemyState.Idle;
            if (!Check_Stun_Freeze())
            {
                CurrentState = EnemyState.Attack;
            }
        }
        else
        {
            isAttack = false;
            //if (isMove)
            //Move(enemy.speed);
        }
    }
    public void DisableAttackIntimeInterval(float _time)
    {
        StartCoroutine(IEdisableAttack(_time));
    }
    IEnumerator IEdisableAttack(float _time)
    {
        disableAttack = true;
        yield return new WaitForSeconds(_time);
        disableAttack = false;
    }

}
