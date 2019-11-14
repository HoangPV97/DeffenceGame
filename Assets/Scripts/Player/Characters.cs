using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum CharacterState { Idle,Attack};
public class Characters : MonoBehaviour
{
    // Start is called before the first frame update
    public float range = 7f;
    public Transform Target;
    private Enemy m_Enemy;
    public string EnemyTag = "Enemy";
    public float RateOfFire = 1f;
    public GameObject Barrel;
    public GameObject Bullet;
    public static bool Live = true;
    ObjectPoolManager poolManager;
    public Elemental elementalType;
    protected CharacterState characterState,preCharacterState;
    public PlaySkeletonAnimationState playSkeletonAnimation;
    protected void Start()
    {
        Live = true;
        poolManager = ObjectPoolManager.Instance;
        InvokeRepeating("UpdateEnemy", 0f, 0.5f);
    }


    // Update is called once per frame
    protected void Update()
    {
        if (Target == null)
        {
            return;
        }
        if (preCharacterState != characterState)
        {
            ChangeState();
            preCharacterState = characterState;
        }
    }

    private void ChangeState()
    {
        string nameState = null;
        if (characterState.Equals(CharacterState.Attack))
        {
            nameState = "attack";
            
        }
        else if (characterState.Equals(CharacterState.Idle))
        {
            nameState = "idle";
            
        }
        Debug.Log("State Name :" + nameState);
        playSkeletonAnimation.PlayAnimationState(nameState);
    }

    private void UpdateEnemy()
    {
        GameObject[] Enemies = GameObject.FindGameObjectsWithTag(EnemyTag);
        if (Enemies.Length == 0)
        {
            characterState = CharacterState.Idle;
        }
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
    
    public GameObject Spawn(string tag, Vector3 _position)
    {
        return poolManager.SpawnObject(tag, _position, Quaternion.identity);
    }
    
}