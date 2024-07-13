using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : MonoBehaviour
{
    [Header("Refer")]
    public Transform orientation;
    public Transform playerCam;
    public Rigidbody rb;

    // 이동 스크립트

    [Header("Dashing")]
    public float dashForce;
    public float dashUpwardForce;
    public float dashDuration;

    [Header("Cooldown")]
    public float dashCd;
    public float dashCdTimer;

    [Header("Input")]
    public KeyCode dashKey = KeyCode.E;

    private void Start()
    {
        rb = GetComponent<Rigidbody> ();

    }

    private void Update()
    {
        if(Input.GetKey (dashKey))
        {
            DashAction();
        }
    }

    void DashAction() 
    {
        Vector3 forceToApply = orientation.forward * dashForce + orientation.up * dashUpwardForce;

        rb.AddForce(forceToApply, ForceMode.Impulse);

        Invoke(nameof(ResetDash), dashDuration);
    }

    void ResetDash()
    {

    }
}
