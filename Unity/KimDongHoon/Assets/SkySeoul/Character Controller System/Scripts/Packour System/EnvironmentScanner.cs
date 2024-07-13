using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentScanner : MonoBehaviour
{
    [SerializeField] Vector3 forwardRayOffset = new Vector3(0, 2.5f, 0);
    [SerializeField] float forwardRayLength = 0.8f;
    [SerializeField] float heightRayLength = 5f;
    [SerializeField] float ledgeRayLength = 5f;
    [SerializeField] LayerMask obstacleLayer;
    [SerializeField] LayerMask climbLedgelayer;
    [SerializeField] float climbLedgeRayLength = 1.5f; 
    [SerializeField] float ledgeHeightThreshold = 0.75f;

    public ObstalceHitData ObstacleCheck()
    {
        var hitData = new ObstalceHitData();

        var forwardOrigin = transform.position + forwardRayOffset;

        hitData.forwardHitFound = Physics.Raycast(forwardOrigin, transform.forward, out hitData.forwardHit
            , forwardRayLength, obstacleLayer);

        Debug.DrawRay(forwardOrigin, transform.forward * forwardRayLength, (hitData.forwardHitFound) ? Color.red : Color.white);

        if (hitData.forwardHitFound)
        {
            var heightOrigin = hitData.forwardHit.point + Vector3.up * heightRayLength;
          hitData.heightHitFound =  Physics.Raycast(heightOrigin, Vector3.down,
                out hitData.heightHit, heightRayLength, obstacleLayer);

            Debug.DrawRay(heightOrigin, Vector3.down * heightRayLength, (hitData.heightHitFound) ? Color.red : Color.white);
        }

        return hitData;
    }

    public bool ClimbLedgeCheck(Vector3 dir, out RaycastHit ledgeHit)
    {
        ledgeHit = new RaycastHit();

        if (dir == Vector3.zero)
            return false;

        var origin = transform.position + Vector3.up * 1.5f;
        var offset = new Vector3(0, 0.18f, 0);

        for (int i = 0; i < 15; i++)
        {
            Debug.DrawRay(origin + offset * i, dir);

            if(Physics.Raycast(origin + offset * i, dir, out RaycastHit hit, climbLedgeRayLength, climbLedgelayer))
            {
                ledgeHit = hit;
                return true;
            }
        }

        return false;
    }

    public bool DropLedgeCheck(out RaycastHit ledgeHit)
    {
        ledgeHit = new RaycastHit();

        var origin = transform.position + Vector3.down * 0.1f + transform.forward * 2f;
        Debug.DrawRay(origin, -transform.forward);

        if (Physics.Raycast(origin, -transform.forward, out RaycastHit hit, 3, climbLedgelayer))
        {
            ledgeHit = hit;
            return true;
        }
        return false;
    }
 

    public bool ObstacleLedgeCheck(Vector3 moveDir, out LedgeData ledgeData)
    {
        ledgeData = new LedgeData();

        if(moveDir == Vector3.zero)
            return false;

        float originOffset = 0.5f;
        var origin = transform.position + moveDir * originOffset + Vector3.up;

        if(Physics.Raycast(origin, Vector3.down, out RaycastHit hit, ledgeRayLength, obstacleLayer))
        {
            Debug.DrawRay(origin, Vector3.down * ledgeRayLength, Color.green);

            var surfaceRayOrigin = transform.position + moveDir - new Vector3(0, 0.1f, 0);
            if(Physics.Raycast(surfaceRayOrigin, -moveDir, out RaycastHit surfaceHit, 2, obstacleLayer))
            {
                float height = transform.position.y - hit.point.y;

                if (height > ledgeHeightThreshold)
                {
                    ledgeData.angle = Vector3.Angle(transform.forward, surfaceHit.normal);
                    ledgeData.height = height;
                    ledgeData.surfaceHit = surfaceHit;

                    return true;
                }
            }

           
        }

        return false;
    }
}

public struct ObstalceHitData
{
    public bool forwardHitFound;
    public bool heightHitFound;
    public RaycastHit forwardHit;
    public RaycastHit heightHit;
}

public struct LedgeData
{
    public float height;
    public float angle;
    public RaycastHit surfaceHit;
}
