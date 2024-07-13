using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walker : MonoBehaviour
{
    public Transform leftFootTarget;
    public Transform rightFootTarget;
    public AnimationCurve horizontalCurve; // 수평 이동
    public AnimationCurve verticalCurve; // 다리의 수직 이동
    private Vector3 leftTargetOffset;
    private Vector3 rightTargetOffset;

    private float leftLegLast = 0;
    private float rightLegLast = 0;

    // Start is called before the first frame update
    void Start()
    {
        leftTargetOffset = leftFootTarget.localPosition;
        rightTargetOffset = rightFootTarget.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        float leftLegForwardMovement = horizontalCurve.Evaluate(Time.time);
        float rightLegForwardMovement = horizontalCurve.Evaluate(Time.time - 1);


        // 1. 절대 위치에서 애니메이션 이벤트 실행

        // 2. 상대 위치를 받아와서 애니메이션 움직이게 하는 방법

        leftFootTarget.localPosition = leftTargetOffset + 
            this.transform.InverseTransformVector(leftFootTarget.forward) * leftLegForwardMovement +
            this.transform.InverseTransformVector(leftFootTarget.up) * verticalCurve.Evaluate(Time.time + 0.5f);
        rightFootTarget.localPosition = rightTargetOffset + 
            this.transform.InverseTransformVector(rightFootTarget.forward) * rightLegForwardMovement +
            this.transform.InverseTransformVector(rightFootTarget.up) * verticalCurve.Evaluate(Time.time - 0.5f);

        float leftLegDirection = leftLegForwardMovement - leftLegLast;
        float rightLegDirection = rightLegForwardMovement - rightLegLast;

        RaycastHit hit;
        if(leftLegDirection < 0 &&
            Physics.Raycast(leftFootTarget.position + leftFootTarget.up, -leftFootTarget.up, out hit, Mathf.Infinity))
        {
            leftFootTarget.position = hit.point;
            //this.transform.position += this.transform.forward * Mathf.Abs(leftLegDirection);
        }

        if (rightLegDirection < 0 &&
            Physics.Raycast(rightFootTarget.position + rightFootTarget.up, -rightFootTarget.up, out hit, Mathf.Infinity))
        {
            rightFootTarget.position = hit.point;
            //this.transform.position += this.transform.forward * Mathf.Abs(rightLegDirection);
        }

        leftLegLast = leftLegForwardMovement;
        rightLegLast = rightLegForwardMovement;
    }
}
