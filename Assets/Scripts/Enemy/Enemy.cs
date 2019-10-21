using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public float currentHealth=100f;
    public float Speed, Damge;
    public float Health = 100f;
    public Image healthBar;
    public GameObject Bullet;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }
    public void Move()
    {
        gameObject.transform.position += Vector3.down* Speed * Time.deltaTime * 0.01f;
    }
    public void Attack()
    {

    }
    public void TakeDamge(float _Damge)
    {
        currentHealth -= _Damge;
        healthBar.fillAmount = currentHealth / Health*1.0f;
        Debug.Log("Health : " + currentHealth / Health * 1.0f);
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    public void Die()
    {
        Destroy(gameObject);
    }
    //public void OnTriggerEnter2D(Collider2D Target)
    //{
    //    if (Target.gameObject.tag.Equals("Bullet"))
    //    {
    //        TakeDamge(10f);
    //    }
    //}
}
