using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopteAction : MonoBehaviour
{
    public Transform player;
    Camera cam;
    RaycastHit hit;
    public LayerMask GrapplingObj;
    LineRenderer lr;

    public Transform tip;

    private bool OnGraplling;

    Vector3 spot;
    SpringJoint sj;

    [Range(0,20)]
    [SerializeField] float spring;
    [Range(0, 20)]
    [SerializeField] float damper;
    [Range(0, 20)]
    [SerializeField] float massScale;


    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        lr = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RopeShot();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            EndShoot();
        }
    }

    private void LateUpdate()
    {
        DrawRope();
    }

    void RopeShot()
    {
        if(Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 100f, GrapplingObj))
        {
            OnGraplling = true;

            spot = hit.point;
            lr.positionCount = 2;
            lr.SetPosition(0, this.transform.position);
            lr.SetPosition(1, hit.point);

            sj = player.gameObject.AddComponent<SpringJoint>();
            sj.autoConfigureConnectedAnchor = false;
            sj.connectedAnchor = spot;

            float dis = Vector3.Distance(this.transform.position, spot);

            sj.maxDistance = dis * 0.8f;
            sj.minDistance = dis * 0.25f;
            sj.spring = spring;
            sj.damper = damper;
            sj.massScale = massScale;
        }
    }

    void EndShoot()
    {
        OnGraplling = false;
        lr.positionCount = 0;
        Destroy(sj);
    }

    void DrawRope()
    {
        if (OnGraplling)
        {
            lr.SetPosition(0, tip.position);
        }
    }
}
