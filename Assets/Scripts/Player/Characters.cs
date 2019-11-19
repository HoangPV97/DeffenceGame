using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Characters : MonoBehaviour
{

    //protected void Start()
    //{

    //}


    //// Update is called once per frame
    //protected void Update()
    //{

    //    if (preCharacterState != characterState)
    //    {
    //        ChangeState();
    //        preCharacterState = characterState;
    //    }
    //}

    //private void ChangeState()
    //{
    //    string nameState = null;
    //    if (characterState.Equals(CharacterState.Attack))
    //    {
    //        nameState = "attack";

    //    }
    //    else if (characterState.Equals(CharacterState.Idle))
    //    {
    //        nameState = "idle";

    //    }
    //    playSkeletonAnimation.PlayAnimationState(nameState);
    //}

    protected void UpdateEnemy(Transform Target, float range)
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
        }
        else
        {
            Target = null;
        }
    }



}