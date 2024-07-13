using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grappling : MonoBehaviour
{
    [Header("References")]
    private PlayerController pm;
    public Transform cam;
    public Transform gunTip;
    public LayerMask whatIsGrappleable;
    public LineRenderer lr;

    [Header("Grappling")]
    public float maxGrappleDistance;
    public float grappleDelayTime;
    public float overshootYAxis;

    private Vector3 grapplePoint;
    private Vector3 hookPoint;

    [Header("Cooldown")]
    public float grapplingCd;
    private float grapplingCdTimer;

    [Header("Input")]
    public KeyCode grappleKey = KeyCode.Mouse1;
    public KeyCode hookKey = KeyCode.Mouse0;

    private bool grappling;
    private bool hooking;
    private bool isHookLink = false;

    private SpringJoint joint;
    private float maxHookDistance = 100f;

    private Animator anim;

    private void Start()
    {
        pm = GetComponent<PlayerController>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        anim.SetBool("IsHooking", hooking);

        if (Input.GetKeyDown(hookKey) && !grappling)
        {
            StartHook();
        }
        else if (Input.GetKeyUp(hookKey) && hooking)
        {
            anim.CrossFade("HookEnd", 0.5f);
            StopHook();
        }

        if (Input.GetKeyDown(grappleKey)) StartGrapple();

        if (grapplingCdTimer > 0)
            grapplingCdTimer -= Time.deltaTime;
    }

    private void LateUpdate()
    {
        //if (grappling)
        //    lr.SetPosition(0, gunTip.position);
    }

    void StartHook()
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.position, cam.forward, out hit, maxHookDistance, whatIsGrappleable))
        {
            if (!isHookLink)
                anim.CrossFade("Hook Start", 0f);

            hooking = true;
            isHookLink = true;
            hookPoint = hit.point;
            joint = gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = hookPoint;

            float distanceFromPoint = Vector3.Distance(transform.position, hookPoint);

            //The distance grapple will try to keep from grapple point. 
            joint.maxDistance = distanceFromPoint * 0.8f;
            joint.minDistance = distanceFromPoint * 0.25f;

            //Adjust these values to fit your game.
            joint.spring = 4.5f;
            joint.damper = 7f;
            joint.massScale = 4.5f;
        }
    }

    void StopHook()
    {
        hooking = false;
        isHookLink = false;
        Destroy(joint);
    }

    private void StartGrapple()
    {
        if (grapplingCdTimer > 0) return;

        if (!hooking) return;

        grapplePoint = hookPoint;

        Invoke(nameof(StopHook), grappleDelayTime);

        grappling = true;

        pm.freeze = true;

        RaycastHit hit;


        Invoke(nameof(ExecuteGrapple), grappleDelayTime);
        //if (Physics.Raycast(cam.position, cam.forward, out hit, maxGrappleDistance, whatIsGrappleable))
        //{
        //    grapplePoint = hit.point;

        //    Invoke(nameof(ExecuteGrapple), grappleDelayTime);
        //}
        //else
        //{
        //    grapplePoint = cam.position + cam.forward * maxGrappleDistance;

        //    Invoke(nameof(StopGrapple), grappleDelayTime);
        //}

    }

    private void ExecuteGrapple()
    {
        pm.freeze = false;

        Vector3 lowestPoint = new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z);

        float grapplePointRelativeYPos = hookPoint.y - lowestPoint.y;
        float highestPointOnArc = grapplePointRelativeYPos + overshootYAxis;

        if (grapplePointRelativeYPos < 0) highestPointOnArc = overshootYAxis;

        pm.JumpToPoSition(hookPoint, highestPointOnArc, grappleDelayTime);

        Invoke(nameof(StopGrapple), 3f);
    }

    public void StopGrapple()
    {

        pm.freeze = false;

        grappling = false;

        grapplingCdTimer = grapplingCd;
    }

    public bool IsGrappling()
    {
        return grappling;
    }

    public bool IsHooking()
    {
        return joint != null;
    }

    public Vector3 GetGrapplePoint()
    {
        return grapplePoint;
    }

    public Vector3 GetHookPoint()
    {
        return hookPoint;
    }
}
