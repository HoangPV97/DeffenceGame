using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    private float currentHealth;
    public float Speed, Damge, range, Price, Health, Armor;
    public Image healthBar;
    GameObject Player;
    string PlayerTag = "Player";
    float distancetoPlayer;
    private float RateOfFire = 1f;
    public static float EnemyLive;
    public ParticleSystem particleSystem;
    ObjectPoolManager PoolManager;
    SoundManager soundManager;
    float distance;
    public Elemental elementalEnemy;
    // Start is called before the first frame update
    private void Awake()
    {
        PoolManager = ObjectPoolManager.Instance;
        soundManager = SoundManager.Instance;
    }
    void Start()
    {
        currentHealth = Health;
        SeekingPlayer();
        distance = Vector3.Distance(transform.position, Player.transform.position);
    }

    // Update is called once per frame
    void Update()
    {

        Move();
        AutoAttack();
    }
    /// <summary>
    /// Speed up for enemy
    /// </summary>
    /// <param name="_Speed"> Speed bonus</param>
    public void SpeedUpEnemy(float _Speed)
    {
        Speed += _Speed;
    }
    //Find player with tag="Player"
    void SeekingPlayer()
    {
        Player = GameObject.FindGameObjectWithTag(PlayerTag);
    }
    /// <summary>
    /// Move Enemy with velocity= distance/time
    /// </summary>
    public void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, Player.transform.position, Time.deltaTime * (distance / Speed));
    }

    //
    public void Attack()
    {
        distancetoPlayer = Vector3.Distance(transform.position, Player.transform.position);
        if (distancetoPlayer < range)
        {
            GameObject EnemyBullet = PoolManager.SpawnObject("enemybullet", transform.position, Quaternion.identity);
            EnemyBullet m_EnemyBullet = EnemyBullet.GetComponent<EnemyBullet>();
            m_EnemyBullet?.SetTarget(Player.transform);
            m_EnemyBullet?.SetDamage(Damge);
        }
    }
    public void Die()
    {

        gameObject.SetActive(false);
        EnemyLive--;
        SpawnGold();
    }
    private void AutoAttack()
    {
        if (RateOfFire <= 0f)
        {
            Attack();
            RateOfFire = 1f;
        }
        RateOfFire -= Time.deltaTime;
    }
    public void SpawnGold()
    {
        GameObject _gold = PoolManager.SpawnObject("gold", transform.position, Quaternion.identity);
        soundManager.PlayClipOneShot(soundManager.Coin);
        _gold.GetComponent<Gold>().Price = Price;
    }
    public void TakeDamage(Elemental _elementalType, float _damage, float _damagePlus)
    {
        switch (elementalEnemy)
        {
            case Elemental.Fire:
                if (_elementalType.Equals(Elemental.Ice))
                {
                    DealDamge(_damage, _damagePlus);
                }
                else
                {
                    DealDamge(_damage, 0);
                }
                break;
            case Elemental.Ice:
                if (_elementalType.Equals(Elemental.Wind))
                {
                    DealDamge(_damage, _damagePlus);
                }
                else
                {
                    DealDamge(_damage, 0);
                }
                break;
            case Elemental.Wind:
                if (_elementalType.Equals(Elemental.Fire))
                {
                    DealDamge(_damage, _damagePlus);
                }
                else
                {
                    DealDamge(_damage, 0);
                }
                break;
        }
    }
    public void DealDamge(float _damage, float _damageplus)
    {
        SpawnDamageText("damage",gameObject.transform.position, _damage);
        if (_damageplus > 0)
        {
            SpawnDamageText("elementaldamage", gameObject.transform.position+new Vector3(0,0.2f,0), _damageplus);
        }       
        currentHealth -= _damage + _damageplus;
        healthBar.fillAmount = currentHealth / Health;
        if (currentHealth < (Health / 2))
        {
            particleSystem.gameObject.SetActive(true);
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }
    public void SpawnDamageText(string tag,Vector2 _postion,float _damage)
    {
        GameObject damageobj = PoolManager.SpawnObject(tag, _postion, Quaternion.identity);
        damageobj.transform.parent = GetComponentInChildren<Canvas>().gameObject.transform;
        damageobj.GetComponent<LoadingText>().SetTextDamage(_damage.ToString());
    }
}
