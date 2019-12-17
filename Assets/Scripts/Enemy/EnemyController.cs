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
    public PlaySkeletonAnimationState playSkeletonAnimation;
    public bool isMove = true, isAttack = true, isLive = true;
    bool isHurt, isIdle;
    protected GameObject Tower;
    protected float distancetoTower;
    protected float countdown;
    public static float EnemyLive;
    SoundManager soundManager;
    float distance;
    GameEffect gameEffect;
    GameObject effectObj;
    public Rigidbody2D Rigidbody2D;

    [SerializeField] GameObject HealthUI;
    [SerializeField] BoxCollider2D boxCollider2D;
    // Start is called before the first frame update
    private void Awake()
    {
        soundManager = SoundManager.Instance;
    }
    protected void Start()
    {
        gameEffect = GetComponent<GameEffect>();
    }

    public void SetUpdata(string type, int Level)
    {
        var md = DataController.Instance.GetMonsterData(type);
        float growth = 1 + md.Growth * Level;
        enemy.health.Init(md.HP * growth, 0);
        enemy.damage = md.ATK * growth;
        enemy.armor = md.Armor * growth;
        enemy.speed = md.MoveSpeed;
        enemy.rateOfFire = md.ATKSpeed;
        enemy.bulletSpeed = md.BulletSpeed;
        enemy.range = md.Range;
        isMove = true;
        isAttack = true;
        isLive = true;
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
        CheckAttack();
    }

    protected void SeekingTower()
    {
        Tower = GameObject.FindGameObjectWithTag("Tower");
    }
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
        GameplayController.Instance.PlayerController.listEnemies.Remove(this);
        // GameplayController.Instance.Alliance_1.listEnemies.Remove(this);
        //  GameplayController.Instance.Alliance_2.listEnemies.Remove(this);
        isLive = false;
        isAttack = false;
        CurrentState = EnemyState.Die;
        Rigidbody2D.velocity = Vector2.zero;
        var lText = GetComponentsInChildren<LoadingText>();
        for (int i = 0; i < lText.Length; i++)
        {
            ObjectPoolManager.Instance.DespawnObJect(lText[i].gameObject);
        }
        HealthUI.SetActive(false);
        boxCollider2D.enabled = false;
        if (effectObj != null && effectObj.activeSelf)
        {
            effectObj.SetActive(false);
        }
        yield return new WaitForSeconds(1);

        EnemyLive--;
        Despawn();
    }
    public void DealDamge(float _damage, float _damageplus=0f)
    {
        isHurt = true;
        //CurrentState = EnemyState.Hurt;
        SpawnDamageText("damage", gameObject.transform.position, _damage);
        if (_damageplus > 0)
        {
            SpawnDamageText("elementaldamage", gameObject.transform.position + new Vector3(0, 0.2f, 0), _damageplus);
        }
        enemy.health.ReduceHealth(_damage + _damageplus);

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
    private void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.gameObject.tag.Equals("Tower"))
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
    public void Despawn()
    {
        ObjectPoolManager.Instance.DespawnObJect(gameObject);
    }

    public virtual void CheckAttack()
    {
        distancetoTower = Mathf.Abs(transform.position.y - Tower.transform.position.y);
        if (distancetoTower < enemy.range && isLive)
        {
            if (countdown <= 0f && isAttack)
            {
                isAttack = true;
                Rigidbody2D.velocity = Vector2.zero;
                CurrentState = EnemyState.Idle;
                CurrentState = EnemyState.Attack;
                countdown = enemy.rateOfFire;
            }
            countdown -= Time.deltaTime;
        }
    }

}
