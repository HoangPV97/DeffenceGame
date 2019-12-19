using Spine;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Spine.AnimationState;

public class PlaySkeletonAnimationState : MonoBehaviour
{
    [System.Serializable]
    public class StateNameToAnimationReference
    {
        public string stateName;
        public AnimationReferenceAsset animation;
    }
    public SkeletonAnimation skeletonAnimation;
    public List<StateNameToAnimationReference> statesAnimation = new List<StateNameToAnimationReference>();
    public void PlayAnimationState(string _state,float _timeScale)
    {
        PlayAnimation(GetAnimationStateInList(_state),_timeScale);
        
    }
    public void PlayAnimation(Spine.Animation _animation, float _timeScale)
    {
        skeletonAnimation.AnimationState.SetAnimation(0, _animation, true);
        skeletonAnimation.timeScale = _timeScale;

    }
    public Spine.Animation GetAnimationStateInList(string State)
    {
        return statesAnimation.Where(obj => obj.stateName.Equals(State)).SingleOrDefault().animation;
    }
}
