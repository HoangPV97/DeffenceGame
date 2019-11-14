using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankBullet : Bullet
{
    Vector2 bulletDirection;
    // Start is called before the first frame update
    public void SetDirection(Vector2 _direction)
    {
        bulletDirection = _direction;
    }
    public void Start()
    {
        base.Start();
    }
    private void Update()
    {

        //base.Update();
    }
    // Update is called once per frame
    public void DirectShooting(Vector2 _direction)
    {
        Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
        GetComponent<Rigidbody2D>().velocity = _direction.normalized * 50 * Speed * Time.deltaTime;
    }
    protected void OnTriggerEnter2D(Collider2D Target)
    {
        base.OnTriggerEnter2D(Target);
        if (Target.gameObject.tag.Equals(TargetTag))
        {
            Enemy enemy = Target.GetComponent<Enemy>();
            enemy.CurrentState = EnemyState.Hurt;
            enemy?.TakeDamage(elementalBullet,Damge,damagePlus);
            if (SeekTarget)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
