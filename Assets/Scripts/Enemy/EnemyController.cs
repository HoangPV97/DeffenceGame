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
    public bool isMove = true, isAttack, isLive = true;
    bool isHurt, isIdle;
    protected GameObject Tower;
    public float distancetoTower;
    protected float countdown;
    protected float distance;
    public GameEffect gameEffect;
    protected GameObject effectObj;
    public Rigidbody2D Rigidbody2D;
    [SerializeField] Canvas canvas;
    [SerializeField] BoxCollider2D boxCollider2D;
    private Vector2 DirectionMove;
    bool Coroutine_running;
    // Start is called before the first frame update
    protected virtual void Start()
    {
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
        enemy.health.Init(md.HP * growth, 0);
        enemy.damage = md.ATK * growth;
        enemy.armor = md.Armor * growth;
        enemy.speed = md.MoveSpeed;
        enemy.rateOfFire = md.ATKSpeed;
        Debug.Log("RateofFire :" + enemy.rateOfFire);
        enemy.bulletSpeed = md.BulletSpeed;
        enemy.range = md.Range;
        isMove = true;
        isLive = true;
        SeekingTower();
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
        if (enemy.health.CurrentHealth <= 0)
        {
            StartCoroutine(Die());
        }
        CheckAttack();
    }
    protected void SeekingTower()
    {
        Tower = GameObject.FindGameObjectWithTag("Tower");
    }
    public virtual void Move(float _speed, float percent_slow = 100f)
    {
        if (!isAttack)
        {
            if (isMove)
            {
                CurrentState = EnemyState.Run;
                Rigidbody2D.velocity = DirectionMove * (distance / enemy.speed) * (percent_slow / 100);
            }
            else
            {
                Rigidbody2D.velocity = Vector2.zero;
                CurrentState = EnemyState.Idle;
            }
        }
    }
    IEnumerator Die()
    {
        GameplayController.Instance.PlayerController.listEnemies.Remove(this);
        GameplayController.Instance.Alliance_1?.listEnemies.Remove(this);
        GameplayController.Instance.Alliance_2?.listEnemies.Remove(this);

        isLive = false;
        isAttack = false;
        isMove = false;
        CurrentState = EnemyState.Die;
        Rigidbody2D.velocity = Vector2.zero;
        var lText = GetComponentsInChildren<LoadingText>();
        for (int i = 0; i < lText.Length; i++)
        {
            ObjectPoolManager.Instance.DespawnObJect(lText[i].gameObject);
        }
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
    public void DealDamge(float _damage, float _damageplus = 0f)
    {
        canvas.gameObject.SetActive(true);
        isHurt = true;
        //CurrentState = EnemyState.Hurt;
        SpawnDamageText("damage", gameObject.transform.position, _damage);
        if (_damageplus > 0)
        {
            SpawnDamageText("elementaldamage", gameObject.transform.position + new Vector3(0, 0.2f, 0), _damageplus);
        }
        enemy.health.ReduceHealth(_damage + _damageplus);
        //Invoke("DisableCanvas", 2);
        //StartCoroutine(IEDisableCanvas());
    }
    private void SpawnDamageText(string tag, Vector2 _postion, float _damage)
    {
        GameObject damageobj = ObjectPoolManager.Instance.SpawnObject(tag, _postion, Quaternion.identity);
        damageobj.transform.SetParent(canvas.gameObject.transform);
        damageobj.GetComponent<LoadingText>().SetTextDamage(_damage.ToString());
    }
    public void IsKnockback(Vector3 _knockback)
    {
        isMove = true;
        gameEffect.KnockBack(gameObject, _knockback);
        if (effectObj != null)
        {
            gameEffect.KnockBack(effectObj, _knockback);
        }
    }
    public virtual void DealEffect(Effect _effect, Vector3 _position, float _time)
    {
         if (!Coroutine_running || _effect != gameEffect.CurrentEffect)
        {
            StartCoroutine(WaitingEffect(_time, () =>
            {

                Coroutine_running = true;
                
                if (_effect.Equals(Effect.Slow))
                {
                    Debug.Log("Slow");
                }
                else if (_effect.Equals(Effect.Stun) || _effect.Equals(Effect.Freeze))
                {
                    isMove = false;
                    Rigidbody2D.velocity = Vector2.zero;
                    //Move(enemy.speed);
                    CurrentState = EnemyState.Idle;
                    skeletonAnimation.timeScale = 0;
                    if (effectObj == null || _effect != gameEffect.CurrentEffect)
                        effectObj = gameEffect.GetEffect(_effect, _position, _time);
                }
                else if (_effect.Equals(Effect.Poiton))
                {
                    if (_effect != gameEffect.CurrentEffect)
                        effectObj = gameEffect.GetEffect(_effect, _position, _time);
                }
                gameEffect.SetEffect(_effect);
            }, () =>
            {
                skeletonAnimation.timeScale = 1;
                gameEffect.SetEffect(Effect.None);
                Despawn(effectObj);
                effectObj = null;
                isMove = true;
                if (_effect.Equals(Effect.Freeze))
                {
                    gameEffect.GetEffect(Effect.destroyFreeze, _position, _time);
                }
                if (!isAttack)
                {
                    Move(enemy.speed);
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
    private void OnTriggerenter2D(Collider2D collider2D)
    {
        if (collider2D.gameObject.tag.Equals("Tower"))
        {
            isMove = false;
            isAttack = true;
            Rigidbody2D.velocity = Vector2.zero;
            CurrentState = EnemyState.Attack;
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

        if ((distancetoTower <= 2.2f || distancetoTower <= enemy.range) && isLive)
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
            if(isMove)     
                Move(enemy.speed);
        }
    }

}
