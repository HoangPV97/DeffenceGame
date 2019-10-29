using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Characters : MonoBehaviour
{
    // Start is called before the first frame update
    public float range = 7f;
    protected Transform Target;
    private Enemy m_Enemy;
    public string EnemyTag="Enemy";
    private float RateOfFire = 1f;
    public GameObject Barrel;
    public GameObject Bullet;
    public static bool Live = true;
    protected void Start()
    {
        Live = true;
        InvokeRepeating("UpdateEnemy", 0f, 0.5f);
    }


    // Update is called once per frame
    protected void Update()
    {
        if (Target == null)
        {
            return;
        }
        if (Live && Target!=null)
        {
            LookAtEnemy(Target);
            AutoShoot();
        }
        
    }
    private void UpdateEnemy()
    {
        GameObject[] Enemies = GameObject.FindGameObjectsWithTag(EnemyTag);
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
            towerBullet?.SetTarget(Target);
        }
        
    }
    public void LookAtEnemy( Transform _Taget)
    {
        Vector3 dir = _Taget.position - transform.position;
        transform.up = dir;
    }
    private void AutoShoot()
    {
        if (RateOfFire <= 0f)
        {
            Shoot();
            RateOfFire = 1f;
        }
        RateOfFire -= Time.deltaTime;
    }
}
