using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    private float currentHealth;
    public float Price ;
    public float Speed, Damge, range;
    public float Health;
    public Image healthBar;
    public GameObject Bullet;
    GameObject Player;
    string PlayerTag="Player";
    float distancetoPlayer;
    private float RateOfFire = 1f;
    public static float EnemyLive;
    public GameObject DamageText;
    // Start is called before the first frame update
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
        gameObject.transform.position += Vector3.down* Speed * Time.deltaTime * 0.01f;
    }
    public void Attack()
    {
        distancetoPlayer = Vector3.Distance(transform.position, Player.transform.position);
        if (Bullet != null&& distancetoPlayer < range)
        {
            GameObject EnemyBullet = Instantiate(Bullet, transform.position, Quaternion.identity);
            EnemyBullet m_EnemyBullet = EnemyBullet.GetComponent<EnemyBullet>();
            m_EnemyBullet?.SetTarget(Player.transform);
            m_EnemyBullet?.SetDamage(Damge);
        }
    }
    public void TakeDamge(float _Damge)
    {
        //if (DamageText != null)
        //{
            GameObject instance = Instantiate(DamageText,GetComponentInChildren<Canvas>().gameObject.transform);
            instance.GetComponent<LoadingText>().SetTextDamage(_Damge.ToString());
        //}
        
        currentHealth -= _Damge;
        healthBar.fillAmount = currentHealth / Health;
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    public void Die()
    {

        Destroy(gameObject);
        GameController.Instance.GoldUp(Price);
        EnemyLive--;
        //if (EnemyLive == 0)
        //{
        //    GameController.Instance.WinGame();
        //}
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
}
