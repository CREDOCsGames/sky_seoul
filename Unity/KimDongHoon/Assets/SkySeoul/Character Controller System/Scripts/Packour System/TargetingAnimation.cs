using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetingAnimation : MonoBehaviour
{
    public Animator animator;
    public MatchAnimation someAnimation;

    public void MatchTarget()
    {
        animator.MatchTarget(someAnimation.MatchPos, transform.rotation, someAnimation.MatchBodyPart, new MatchTargetWeightMask(someAnimation.MatchPosWeight, 0),
            someAnimation.MatchStartTime, someAnimation.MatchTargetTime);
    }

    [System.Serializable]
    public class MatchAnimation
    {
        public Vector3 MatchPos { get; set; }
        public AvatarTarget MatchBodyPart;
        public Vector3 MatchPosWeight;
        public float MatchStartTime;
        public float MatchTargetTime; 
    }
}
