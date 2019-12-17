using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Elemental { None = 0, Wind = 1, Ice = 2, Fire = 3, Earth = 4 }
public class BulletController : MonoBehaviour
{
    public Bullet bullet;
    protected EnemyController Target;
    public bool SeekTarget = false;
    public Elemental elementalBullet;
    public float damagePlus;
    protected Vector3 dir = Vector3.zero;
    #region Attribute bullet
    public bool explosion;
    public bool bounce;
    public bool poison;
    public bool slow;
    public bool pierce;
    public bool critical;
    public bool multishot;
    public bool quickhand;
    #endregion
    protected void Start()
    {
    }
    public virtual void SetDataBullet(float _speed, float _damage)
    {
        bullet.Speed = _speed;
        bullet.Damage = _damage;
    }
    public void SetTarget(EnemyController _Target)
    {
        Target = _Target;
    }
    public void setDirection(Vector3 _dir)
    {
        dir=_dir;
    }
    // Update is called once per frame
    protected void Update()
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
    public void Move( Vector3 _dir)
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
}
