using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    private float currentHealth;
    public float Speed, Damge, range, Price, Health,Armor;
    public Image healthBar;
    public GameObject Bullet, Gold;
    GameObject Player;
    string PlayerTag = "Player";
    float distancetoPlayer;
    private float RateOfFire = 1f;
    public static float EnemyLive;
    public GameObject DamageText;
    public ParticleSystem particleSystem;
    ObjectPoolManager PoolManager;
    // Start is called before the first frame update
    private void Awake()
    {
        PoolManager = ObjectPoolManager.Instance;
    }
    void Start()
    {
        currentHealth = Health;
        SeekingPlayer();
    }

    // Update is called once per frame
    void Update()
    {

        Move();
        AutoAttack();
    }
    public void UpSpeedEnemy(float _Speed)
    {
        Speed += _Speed;
    }
    void SeekingPlayer()
    {
        Player = GameObject.FindGameObjectWithTag(PlayerTag);
    }
    public void Move()
    {
        gameObject.transform.Translate(Vector3.up * 0.1f * Speed * Time.deltaTime);
    }
    public void Attack()
    {
        distancetoPlayer = Vector3.Distance(transform.position, Player.transform.position);
        if (Bullet != null && distancetoPlayer < range)
        {
            GameObject EnemyBullet = PoolManager.SpawnObject("enemybullet", transform.position, Quaternion.identity);
            EnemyBullet m_EnemyBullet = EnemyBullet.GetComponent<EnemyBullet>();
            m_EnemyBullet?.SetTarget(Player.transform);
            m_EnemyBullet?.SetDamage(Damge);
        }
    }
    public void TakeDamge(float _Damge)
    {
        if (_Damge <= Armor)
        {
            return;
        }
        else
        {
            _Damge -= Armor;
        }
        if (DamageText != null)
        {
            GameObject damageobj = PoolManager.SpawnObject("damagetext", gameObject.transform.position, Quaternion.identity);
            damageobj.transform.parent = GetComponentInChildren<Canvas>().gameObject.transform;
            damageobj.GetComponent<LoadingText>().SetTextDamage(_Damge.ToString());
        }

        currentHealth -= _Damge;
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
        if (Gold != null)
        {
            GameObject _gold = PoolManager.SpawnObject("gold", transform.position, Quaternion.identity);
            _gold.GetComponent<Gold>().Price = Price;
        }   
    }
}
