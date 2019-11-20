using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet :MonoBehaviour
{
    public Bullet bullet;
    protected Transform Target;
    public bool SeekTarget = false;
    //public SkeletonAnimation skeletonAnimation;
    //public AnimationReferenceAsset idle;
    
    public void SetDamage(float _damage)
    {
        bullet.Damage = _damage;
    }
    public void SetSpeed(float _speed)
    {
        bullet.Speed = _speed;
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.down * bullet.Speed * Time.deltaTime);
    }
    private void OnTriggerEnter2D(Collider2D _Tower)
    {
        if (_Tower.gameObject.tag.Equals(bullet.TargetTag))
        {
            Tower tower = _Tower.GetComponent<Tower>();
            tower?.TakeDamage(bullet.Damage);
            gameObject.SetActive(false);
        }
    }

    internal void SetTarget(Transform _Target)
    {
        Target = _Target;
    }
}
