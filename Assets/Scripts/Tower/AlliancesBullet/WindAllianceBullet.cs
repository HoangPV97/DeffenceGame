using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindAllianceBullet : BulletController
{
    public float bounceRange;
    public int numberBounce;
    public bool bounce;
    protected override void Start()
    {
        elementalBullet = Elemental.Wind;
    }
    public void SetDataBullet(float _speed, int _damage,float _bounceRange,int _numberBounce,int _Increasedamage)
    {
        bounceRange = _bounceRange;
        numberBounce = _numberBounce;
        damagePlus = _Increasedamage;
        base.SetDataBullet(_speed, _damage);
    }
    protected override void FixedUpdate()
    {
        if (Target == null || !Target.isLive)
        {
            Target = null;
            Move(dir);
        }
        else
        {
            dir = Target.transform.position - transform.position;
            Move(dir);
        }
        if (dir.magnitude < 0.1f)
        {
            Despawn();
        }

    }
    protected override void OnTriggerEnter2D(Collider2D Target)
    {
        if (Target.gameObject.tag.Equals("BlockPoint"))
        {
            Despawn();
        }
        if (Target.gameObject.tag.Equals(bullet.TargetTag))
        {
            EnemyController enemy = Target.GetComponent<EnemyController>();
            SetTarget(enemy);
            enemy.gameEffect.SpawnEffect("windimpact", enemy.transform.position, 0.5f);
            IIceEffectable elemental = enemy?.GetComponent<IIceEffectable>();
            if (elemental != null)
            {
                enemy.DealDamge(bullet.Damage, Mathf.Round(damagePlus * bullet.Damage / 100));
            }
            else
            {
                enemy.DealDamge(bullet.Damage, 0);
            }
            if (bounce)
            {
                Bounce();
            }
        }
    }
    public void Bounce()
    {
        if (GameplayController.Instance.PlayerController.listEnemies.Count > 0 && Target != null)
        {
            nearEnemy = CheckNearEnemy(GameplayController.Instance.PlayerController.listEnemies, Target.GetComponent<EnemyController>());
            if (numberBounce > 0 && nearEnemy != null && nearEnemy.isLive)
            {
                SetTarget(nearEnemy);
                Vector2 _dir = nearEnemy.transform.position - transform.position;
                if (_dir.magnitude < 0.3f)
                {
                    Despawn();
                }
                bullet.Damage = (int)Mathf.Round(bullet.Damage * 50 / 100);
                numberBounce--;
                if (numberBounce <= 0 || !nearEnemy.isLive)
                {
                    Despawn();
                }
            }
            else
            {
                Despawn();
            }
            //nearEnemy = null;
        }

    }
    public EnemyController CheckNearEnemy(List<EnemyController> listEnemies, EnemyController curentEnemy)
    {
        nearEnemy = null;
        int index = 0;
        float shortdistance = Mathf.Infinity;
        for (int i = 0; i < GameplayController.Instance.PlayerController.listEnemies.Count; i++)
        {
            float distance = Vector3.Distance(Target.gameObject.transform.position, GameplayController.Instance.PlayerController.listEnemies[i].transform.position);
            if (distance < shortdistance && GameplayController.Instance.PlayerController.listEnemies[i].gameObject != Target.gameObject
                && GameplayController.Instance.PlayerController.listEnemies[i].isLive)
            {
                shortdistance = distance;
                index = i;
            }
        }
        if (shortdistance <= bounceRange)
        {
            return GameplayController.Instance.PlayerController.listEnemies[index];
        }
        else
        {
            return null;
        }
    }
}
