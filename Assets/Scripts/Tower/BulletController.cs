using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Elemental { Ice, Fire, Wind }
public class BulletController : MonoBehaviour
{
    public Bullet bullet;
    protected Transform Target;
    public bool SeekTarget = false;
    private Vector2 Direction;
    private float RotationZ;
    public Elemental elementalBullet;
    public float damagePlus;
    protected void Start()
    {
    }
    public void SetTarget(Transform _Target)
    {
        Target = _Target;
    }
    // Update is called once per frame
    protected void Update()
    {
        if (Target == null || !Target.gameObject.activeSelf)
        {
            gameObject.SetActive(false);
            return;
        }
        Vector3 dir = Target.position - transform.position;
        transform.up = dir;
        transform.Translate(dir.normalized * bullet.Speed * Time.deltaTime, Space.World);

    }
    public void DirectShooting(Vector2 _direction)
    {
        Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
        GetComponent<Rigidbody2D>().velocity = _direction.normalized * 50 * bullet.Speed * Time.deltaTime;
    }
    protected void OnTriggerEnter2D(Collider2D Target)
    {
        if (Target.gameObject.tag.Equals("BlockPoint"))
        {
            gameObject.SetActive(false);
        }
    }
}
