using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : Characters
{
    private float currentHealth = 100f;
    public float Health = 100f;
    public Image healthBar;
    
    // Start is called before the first frame update
    void Start()
    {
        Live = true;
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
        
    }
    private void FixedUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }
    public void TakeDamge(float _Damge)
    {
        currentHealth -= _Damge;
        healthBar.fillAmount = currentHealth / Health * 1.0f;
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    public void Die()
    {
        Live = false;
    }
}
