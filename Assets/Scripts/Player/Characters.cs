using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Characters : MonoBehaviour
{
    // Start is called before the first frame update
    public float range = 7f;
    protected Transform Target;
    private Enemy m_Enemy;
    private float Countdown = 1f;
    public GameObject Barrel;
    public GameObject Bullet;
    protected void Start()
    {
        InvokeRepeating("UpdateEnemy", 0f, 0.5f);
    }


    // Update is called once per frame
    protected void Update()
    {
        if (Target == null)
        {
            return;
        }
        AutoShoot();
    }
    private void UpdateEnemy()
    {
        GameObject[] Enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;
        foreach (GameObject Enemy in Enemies)
        {
            float distancetoEnemy = Vector3.Distance(transform.position, Enemy.transform.position);
            if (distancetoEnemy < shortestDistance)
            {
                shortestDistance = distancetoEnemy;
                nearestEnemy = Enemy;

            }
        }
        if (nearestEnemy != null && shortestDistance < range)
        {
            Target = nearestEnemy.transform;
            m_Enemy = nearestEnemy.GetComponent<Enemy>();
        }
        else
        {
            Target = null;
        }
    }
    protected void Shoot()
    {
        if (Bullet != null)
        {
            GameObject bullet = Instantiate(Bullet, Barrel.transform.position, Quaternion.identity);
            Bullet towerBullet = bullet.GetComponent<Bullet>();
            towerBullet?.Seek(Target);
        }
        
    }
    private void AutoShoot()
    {
        if (Countdown <= 0f)
        {
            Shoot();
            Countdown = 1f;
        }
        Countdown -= Time.deltaTime;
    }
}
