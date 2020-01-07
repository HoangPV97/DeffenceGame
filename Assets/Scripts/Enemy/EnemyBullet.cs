﻿using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public Bullet bullet;
    [SerializeField]
    string exploseEffect;
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
    private void OnTriggerEnter2D(Collider2D _Target)
    {
        if (_Target.gameObject.tag.Equals("BlockPoint"))
        {
            ObjectPoolManager.Instance.DespawnObJect(gameObject);
        }
        else if (_Target.gameObject.tag.Equals(bullet.TargetTag))
        {
            Tower tower = _Target.GetComponent<Tower>();
            tower?.TakeDamage(bullet.Damage);
            GameObject Exposion = ObjectPoolManager.Instance.SpawnObject(exploseEffect, gameObject.transform.position, Quaternion.identity);
            Exposion.AddComponent<DestroyEffect>()._time = 0.5f;
            //  gameObject.SetActive(false);
            ObjectPoolManager.Instance.DespawnObJect(gameObject);
        }
    }
    public void SetTarget(Transform _Target)
    {
        Target = _Target;
    }
    private void OnEnable()
    {
        var trail = GetComponentInChildren<TrailRenderer>();
        if (trail != null)
            trail.Clear();
    }
}
