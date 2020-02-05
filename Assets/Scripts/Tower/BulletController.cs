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
    //#region Attribute bullet
    //public bool explosion;
    public bool poison;
    public int poisonDamage;
    //#endregion
    protected virtual void Start()
    {
    }
    public void SetKnockBack(float _Distance)
    {
        bullet.KnockbackDistance = _Distance;
    }
    public virtual void SetDataBullet(float _speed, int _damage, float _atkplus)
    {
        bullet.Speed = _speed;
        bullet.Damage = _damage;
        bullet.ATKplus = _atkplus;
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
            //#region Explosion Bullet
            //if (explosion)
            //{
            //    ObjectPoolManager.Instance.SpawnObject("explosionBullet", this.transform.position, Quaternion.identity);
            //    Despawn();
            //}
            //#endregion
            //#region StunBullet
            //if (stun)
            //{
            //    enemyController.DealEffect(Effect.StunBullet, enemyController.transform.position, timeStun);
            //}
            //#endregion
            //if (bullet.KnockbackDistance != 0)
            //{
            //    enemyController.KnockBack(bullet.KnockbackDistance);
            //    bullet.KnockbackDistance = 0;
            //}
        }
    }
}
