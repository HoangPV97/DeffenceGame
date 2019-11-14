using Spine.Unity;
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
        public float Duration { get => animation.Animation.Duration; }
    }
    public SkeletonAnimation skeletonAnimation;
    public delegate void BacktoRunState();
    public static event BacktoRunState eventBacktoRun;
    //public SkeletonAnimation skeletonAnimation;
    public List<StateNameToAnimationReference> statesAnimation = new List<StateNameToAnimationReference>();
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PlayAnimationState(string _state)
    {
        PlayAnimation(GetAnimationStateInList(_state));
    }
    public void PlayAnimation(Spine.Animation _animation)
    {
        //_animation.Duration = 0.3f;
        skeletonAnimation.AnimationState.SetAnimation(0, _animation, true);
        
    }
    public Spine.Animation GetAnimationStateInList(string State)
    {
        return statesAnimation.Where(obj => obj.stateName.Equals(State)).SingleOrDefault().animation;
    }
}
