using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingRope : MonoBehaviour
{
    private Spring spring;
    private LineRenderer lr;
    public Grappling grapplingGun;
    private Vector3 currentGrapplePosition;
    public int quality; // rope의 디테일 수
    public float damper;
    public float strength;
    public float velocity;
    public float waveCount;
    public float waveHeight;
    public AnimationCurve affectCurve;

    void Awake()
    {
        lr = GetComponent<LineRenderer>();
        spring = new Spring();
        spring.SetTarget(0);
    }

    void LateUpdate()
    {
        if(!grapplingGun.IsGrappling())
            DrawRope();

        if(!grapplingGun.IsHooking())
            DrawHook();
    }


    void DrawRope()
    {
        //If not grappling, don't draw rope
        if (!grapplingGun.IsHooking())
        {
            currentGrapplePosition = grapplingGun.gunTip.position;
            spring.Reset();
            if (lr.positionCount > 0)
                lr.positionCount = 0;
            return;
        } 

        if(lr.positionCount == 0)
        {
            spring.SetVelocity(velocity);
            lr.positionCount = quality + 1;
        }
         
        spring.SetDamper(damper);        // 
        spring.SetStrength(strength);    // 얼마나 강력하게 타겟 포인트로 이동하는가 
        spring.Update(Time.deltaTime);

        var grapplePoint = grapplingGun.GetHookPoint();
        var gunTipPosition = grapplingGun.gunTip.position;
        var up = Quaternion.LookRotation(grapplePoint - gunTipPosition.normalized) * Vector3.up;

        currentGrapplePosition = Vector3.Lerp(currentGrapplePosition, grapplingGun.GetHookPoint(), Time.deltaTime * 8f);

        for(var i =0; i<  quality +1; i++)
        {
            var delta = i / (float)quality;
            var offset = up * waveHeight * Mathf.Sin(delta * waveCount * Mathf.PI) * spring.Value * affectCurve.Evaluate(delta);

            lr.SetPosition(i, Vector3.Lerp(gunTipPosition, currentGrapplePosition, delta) + offset);
        }
    }

    void DrawHook()
    {
        //If not grappling, don't draw rope
        if (!grapplingGun.IsGrappling())
        {
            currentGrapplePosition = grapplingGun.gunTip.position;
            spring.Reset();
            if (lr.positionCount > 0)
                lr.positionCount = 0;
            return;
        }

        if (lr.positionCount == 0)
        {
            spring.SetVelocity(velocity);
            lr.positionCount = quality + 1;
        }

        spring.SetDamper(damper);        // 
        spring.SetStrength(strength);    // 얼마나 강력하게 타겟 포인트로 이동하는가 
        spring.Update(Time.deltaTime);

        var grapplePoint = grapplingGun.GetGrapplePoint();
        var gunTipPosition = grapplingGun.gunTip.position;
        var up = Quaternion.LookRotation(grapplePoint - gunTipPosition.normalized) * Vector3.up;

        currentGrapplePosition = Vector3.Lerp(currentGrapplePosition, grapplingGun.GetGrapplePoint(), Time.deltaTime * 8f);

        for (var i = 0; i < quality + 1; i++)
        {
            var delta = i / (float)quality;
            var offset = up * waveHeight * Mathf.Sin(delta * waveCount * Mathf.PI) * spring.Value * affectCurve.Evaluate(delta);

            lr.SetPosition(i, Vector3.Lerp(gunTipPosition, currentGrapplePosition, delta) + offset);
        }
    }
}
