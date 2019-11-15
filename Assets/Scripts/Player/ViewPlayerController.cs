using Spine;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewPlayerController : MonoBehaviour
{
    public SkeletonAnimation skeletonAnimation;
    [SpineBone(dataField: "skeletonAnimation")]
    public string boneName;
    Bone bone;
    // Start is called before the first frame update
    void Start()
    {
        bone = skeletonAnimation.skeleton.FindBone(boneName);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetPositionBone(Vector3 _position)
    {
        bone.SetLocalPosition(_position);
    }
}
