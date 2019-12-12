using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Elemental { None = 0, Wind = 1, Ice = 2, Fire = 3, Earth = 4 }
public class BulletController : MonoBehaviour
{
    public Bullet bullet;
    protected EnemyController Target;
    public bool SeekTarget = false;
    private Vector2 Direction;
    private float RotationZ;
    public Elemental elementalBullet;
    public float damagePlus;
    Vector3 dir = Vector3.zero;
    protected void Start()
    {
    }
    public void SetDataBullet(float _speed, float _damage)
    {
        bullet.Speed = _speed;
        bullet.Damage = _damage;
    }
    public void SetTarget(EnemyController _Target)
    {
        Target = _Target;
    }
    // Update is called once per frame
    protected void Update()
    {
        if (Target == null || !Target.isLive)
        {
            Target = null;
            StartCoroutine(DelayDespawn());
        }
        else
            dir = Target.transform.position - transform.position;
        if (dir == Vector3.zero)
            dir = new Vector3(0, 1, 0);

        transform.up = dir;
        transform.Translate(dir.normalized * bullet.Speed * Time.deltaTime, Space.World);
    }
    IEnumerator DelayDespawn()
    {
        yield return new WaitForSeconds(3);
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
