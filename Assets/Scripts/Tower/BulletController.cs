using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public Bullet bullet;
    protected EnemyController Target;
    public bool SeekTarget = false;
    public Elemental elementalBullet;
    public float damagePlus;
    protected Vector3 dir = Vector3.zero;
    protected bool checkCollision;
    #region Attribute bullet
    public bool explosion;
    public bool bounce;
    public bool poison;
    public bool slow;
    public bool pierce;
    public bool critical;
    public bool multishot;
    public bool quickhand;
    public bool knockback;
    public bool stun;
    public EnemyController nearEnemy;
    private float timeStun = 2f;
    protected float bounceRange = 5f;
    protected int numberBounce = 3;
    private float percent_Slow = 20;
    #endregion
    protected virtual void Start()
    {
        knockback = false;
    }
    public void SetKnockBack(Vector3 _Distance)
    {
        bullet.KnockbackDistance = _Distance;
    }
    public virtual void SetDataBullet(float _speed, float _damage, float _critical_ratio = 0, float _critical_damage = 0)
    {
        bullet.Speed = _speed;
        bullet.Damage = _damage;
        bullet.CriticalDamage = _critical_damage;
        percent_Slow = 20f;
        checkCollision = false;
    }
    public void SetTarget(EnemyController _Target)
    {
        Target = _Target;
    }
    public void setDirection(Vector3 _dir)
    {
        dir = _dir;
    }
    protected virtual void FixedUpdate()
    {
        if (Target == null || !Target.isLive)
        {
            Target = null;
            StartCoroutine(DelayDespawn(3));
        }
        else
            dir = Target.transform.position - transform.position;
        if (dir == Vector3.zero)
            dir = new Vector3(0, 1, 0);
        Move(dir);
    }
    // Update is called once per frame
    protected virtual void Update()
    {
        
    }
    public void Move(Vector3 _dir)
    {
        transform.up = _dir;
        transform.Translate(_dir.normalized * bullet.Speed * Time.deltaTime, Space.World);
    }
    protected IEnumerator DelayDespawn(float _time)
    {
        yield return new WaitForSeconds(_time);
        Despawn();
    }
    public void Despawn()
    {
        ObjectPoolManager.Instance.DespawnObJect(gameObject);
    }

    private void OnEnable()
    {
        var trail = GetComponentInChildren<TrailRenderer>();
        if (trail != null)
            trail.Clear();
    }
    protected virtual void OnTriggerEnter2D(Collider2D _Target)
    {
        if (_Target.gameObject.tag.Equals("BlockPoint"))
        {
            Despawn();
        }
        if (_Target.gameObject.tag.Equals(bullet.TargetTag))
        {
            EnemyController enemyController = _Target.GetComponent<EnemyController>();
            if (SeekTarget)
            {
                Despawn();
            }
            #region Explosion Bullet
            if (explosion)
            {
                ObjectPoolManager.Instance.SpawnObject("explosionBullet", this.transform.position, Quaternion.identity);
                Despawn();
            }
            #endregion
            #region SlowBullet
            if (slow)
            {
                enemyController.Deal_Slow_Effect(2f, percent_Slow);
                Debug.Log("SlowEnemy");
            }
            #endregion
            #region StunBullet
            if (stun)
            {
                enemyController.DealEffect(Effect.StunBullet, enemyController.transform.position, timeStun);
            }
            #endregion
            if(bullet.KnockbackDistance != Vector3.zero)
            {
                enemyController.KnockBack(bullet.KnockbackDistance);
            }
            if (bullet.CriticalDamage > 0)
            {

            }
        }
    }
}
