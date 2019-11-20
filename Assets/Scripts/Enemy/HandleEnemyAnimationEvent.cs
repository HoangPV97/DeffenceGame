using Spine;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HandleEnemyAnimationEvent : MonoBehaviour
{
    public SkeletonAnimation skeletonAnimation;
    public UnityEvent EventAttack;
    // Start is called before the first frame update
    void Start()
    {
        skeletonAnimation.AnimationState.Event += OnEvent;
    }
    private void OnEvent(TrackEntry trackEntry, Spine.Event e)
    {
        if (e.Data.Name.Equals("onatk") && EventAttack != null)
        {
            EventAttack.Invoke();
        }
    }
}
