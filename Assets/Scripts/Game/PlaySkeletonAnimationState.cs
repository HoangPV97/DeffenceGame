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
    public void PlayAnimationState(string _state)
    {
        PlayAnimation(GetAnimationStateInList(_state));
    }
    public void PlayAnimation(Spine.Animation _animation)
    {
        skeletonAnimation.AnimationState.SetAnimation(0, _animation, true);
    }
    public Spine.Animation GetAnimationStateInList(string State)
    {
        return statesAnimation.Where(obj => obj.stateName.Equals(State)).SingleOrDefault().animation;
    }
}
