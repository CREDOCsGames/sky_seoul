using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackourController : MonoBehaviour
{
    [SerializeField] List<ParkourAction> parkourActions;
    [SerializeField] ParkourAction jumpDownAction;
    [SerializeField] float autoDropHeight = 1f;

    EnvironmentScanner environmentScanner;
    Animator animator;
    PlayerController playerController;

    private void Awake()
    {
        environmentScanner = GetComponent<EnvironmentScanner>();
        animator = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();
    }

    private void Update()
    {
        var hitData = environmentScanner.ObstacleCheck();

        if (Input.GetButton("Jump") && !playerController.InAction /*&& playerController.IsGrounded*/)
        {
            if (hitData.forwardHitFound)
            {
                foreach(var action in parkourActions)
                {
                    if(action.CheckIfPossible(hitData, transform))
                    {
                        StartCoroutine(DoParkourAction(action));
                        break;
                    }
                }
            }
        }

        if (playerController.IsOnLedge && !playerController.InAction && !hitData.forwardHitFound)
        {
            bool shouldJump = true;
            if (playerController.LedgeData.height > autoDropHeight && !Input.GetButton("Jump"))
                shouldJump = false;

            if(shouldJump && playerController.LedgeData.angle <= 50)
            {
                playerController.IsOnLedge = false;
                StartCoroutine(DoParkourAction(jumpDownAction));
            }
        }
    }

    IEnumerator DoParkourAction(ParkourAction action)
    {
        playerController.SetControl(false);

        MatchTargetParams matchParam = null;

        if (action.EnableTargetMatching)
        {
            matchParam = new MatchTargetParams()
            {
                pos = action.MatchPos,
                bodyPart = action.MatchBodyPart,
                posWeight = action.MatchPosWeight,
                startTime = action.MatchStartTime,
                targetTime = action.MatchTargetTime
            };
        }

        yield return playerController.DoAction(action.AnimName, matchParam, action.TargetRotation, action.RotateToObstacle, action.PostActionDelay, action.Mirror);

        playerController.SetControl(true);
    }

    void MatchTarget(ParkourAction action)
    {
        if (animator.isMatchingTarget) return;

        animator.MatchTarget(action.MatchPos, transform.rotation, action.MatchBodyPart, new MatchTargetWeightMask(action.MatchPosWeight, 0),
            action.MatchStartTime, action.MatchTargetTime);
    }
}
