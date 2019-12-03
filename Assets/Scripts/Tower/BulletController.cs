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
            // gameObject.SetActive(false);
            return;
        }
        Vector3 dir = Target.transform.position - transform.position;
        transform.up = dir;
        transform.Translate(dir.normalized * bullet.Speed * Time.deltaTime, Space.World);

    }
    public void DirectShooting(Vector2 _direction)
    {
        Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
        GetComponent<Rigidbody2D>().velocity = _direction.normalized * 50 * bullet.Speed * Time.deltaTime;
    }

    public void Despawn()
    {
        ObjectPoolManager.Instance.DespawnObJect(gameObject);
    }
}
