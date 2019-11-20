using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePlayerBullet : BulletController
{
    // Start is called before the first frame update
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
        GetComponent<Rigidbody2D>().velocity = _direction.normalized * 50 * bullet.Speed * Time.deltaTime;
    }
    protected void OnTriggerEnter2D(Collider2D Target)
    {
        if (Target.gameObject.tag.Equals("BlockPoint"))
        {
            gameObject.SetActive(false);
        }
        if (Target.gameObject.tag.Equals(bullet.TargetTag))
        {
            EnemyController enemy = Target.GetComponent<EnemyController>();
            //enemy.CurrentState = EnemyState.Hurt;

            IWindEffectable elemental = enemy.GetComponent<IWindEffectable>();
            if (elemental != null)
            {
                elemental.WindImpactEffect(enemy.transform.position);
                enemy.DealDamge( bullet.Damage, Mathf.Round(damagePlus * bullet.Damage / 100));
            }
            else
            {
                enemy.DealDamge( bullet.Damage, 0);
            }
            if (SeekTarget)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
