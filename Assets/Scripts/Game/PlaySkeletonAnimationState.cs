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
    public static PlaySkeletonAnimationState Instance;
    public SkeletonAnimation skeletonAnimation;
    public CharacterState characterState, preCharacterState;
    //public SkeletonAnimation skeletonAnimation;
    public List<StateNameToAnimationReference> statesAnimation = new List<StateNameToAnimationReference>();
    Bone bone;
    public delegate void Onatk_Player();
    public static Onatk_Player PlayerShoot;


    [SpineEvent(dataField: "skeletonAnimation", fallbackToTextField: true)]
    public string eventName;
    void Start()
    {

        skeletonAnimation.AnimationState.Event += OnEvent1;
        if (Instance == null)
        {
            Instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (preCharacterState != characterState)
        {
            ChangeCharacterState();
            preCharacterState = characterState;
        }
    }
    private void ChangeCharacterState()
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
        PlayAnimationState(nameState);
    }
    public void PlayAnimationState(string _state)
    {
        PlayAnimation(GetAnimationStateInList(_state));
    }
    public void PlayAnimation(Spine.Animation _animation)
    {
        skeletonAnimation.AnimationState.SetAnimation(0, _animation, true);
    }


    private void OnEvent1(TrackEntry trackEntry, Spine.Event e)
    {
        if (e.Data.Name.Equals(eventName))
        {
            PlayerShoot.Invoke();
        }

    }

    public Spine.Animation GetAnimationStateInList(string State)
    {
        return statesAnimation.Where(obj => obj.stateName.Equals(State)).SingleOrDefault().animation;
    }
}
