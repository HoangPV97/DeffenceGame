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
    EnemyController nearEnemy;
    private float bounceRange = 3f;
    private int numberBounce = 3;
    private float percent_Slow = 20;
    #endregion
    protected virtual void Start()
    {
    }
    public virtual void SetDataBullet(float _speed, float _damage)
    {
        bullet.Speed = _speed;
        bullet.Damage = _damage;
        bounceRange = 5f;
        numberBounce = 3;
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
    // Update is called once per frame
    protected virtual void Update()
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
            #region BounceBullet
            if (bounce)
            {
                if (GameplayController.Instance.PlayerController.listEnemies.Count > 0)
                {
                    //listEnemies = listEnemies.OrderBy(obj => (obj.transform.position - transform.position).magnitude).ToList();
                    nearEnemy = null;
                    int index = 0;
                    float shortdistance = Mathf.Infinity;
                    for (int i = 0; i < GameplayController.Instance.PlayerController.listEnemies.Count; i++)
                    {
                        float distance = Vector3.Distance(_Target.gameObject.transform.position, GameplayController.Instance.PlayerController.listEnemies[i].transform.position);
                        if (distance < shortdistance && GameplayController.Instance.PlayerController.listEnemies[i].gameObject != _Target.gameObject)
                        {
                            shortdistance = distance;
                            index = i;
                        }
                    }
                    if (shortdistance <= bounceRange)
                    {
                        nearEnemy = GameplayController.Instance.PlayerController.listEnemies[index];
                    }
                    if (numberBounce > 0 && nearEnemy != null && nearEnemy.isLive)
                    {
                        SetTarget(nearEnemy);
                        dir = nearEnemy.transform.position - transform.position;
                        Move(dir);
                        //nearEnemy.DealDamge(bullet.Damage, damagePlus);
                        bullet.Damage = Mathf.Round(bullet.Damage * 90 / 100);
                        numberBounce--;
                        if (numberBounce < 0 || !nearEnemy.isLive)
                        {
                            Despawn();
                        }
                    }
                    else
                    {
                        Despawn();
                    }
                }
            }
            #endregion
            if (slow)
            {
                enemyController.DealEffect(Effect.Slow, enemyController.transform.position, 2f);
                enemyController.Move(enemyController.enemy.speed, percent_Slow);
                Debug.Log("SlowEnemy");
            }
        }
    }
}
