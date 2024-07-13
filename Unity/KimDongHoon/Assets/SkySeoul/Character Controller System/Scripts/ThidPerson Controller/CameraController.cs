using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.UI.Image;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform followTarget;

    [SerializeField] float rotSpeed = 2f;

    [SerializeField] float distance = 5;

    [SerializeField] float limitAngle = 45;

    [SerializeField] Vector2 framingOffset;

    [SerializeField] bool invertX;
    [SerializeField] bool invertY;

    float rotationX;
    float rotationY;

    float invertXVal;
    float invertYVal;

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        invertXVal = (invertX) ? -1 : 1;
        invertYVal = (invertY) ? -1 : 1;

        rotationX -= Input.GetAxis("Mouse Y") * invertYVal * rotSpeed;
        rotationX = Mathf.Clamp(rotationX, -limitAngle, limitAngle);

        rotationY += Input.GetAxis("Mouse X")* invertXVal * rotSpeed;

        var targetRotation = Quaternion.Euler(rotationX, rotationY, 0);

        var focusPosition = followTarget.position + new Vector3(framingOffset.x, framingOffset.y);

        transform.position = focusPosition - targetRotation * new Vector3(0, 0, distance);
        transform.rotation = targetRotation;
    }

    private void LateUpdate()
    {
        Vector3 direction = (followTarget.position - transform.position).normalized;
        RaycastHit[] hits = Physics.RaycastAll(transform.position, direction, Mathf.Infinity,
        1 << LayerMask.NameToLayer("Ground"));

        Debug.DrawRay(transform.position, direction * 1000f, Color.red);

        for (int i = 0; i < hits.Length; i++)
        {
            TransparentObject[] obj = hits[i].transform.GetComponentsInChildren<TransparentObject>();

            if (obj.Any())
            {
                for (int k = 0; k < obj.Length; k++)
                {
                    obj[k]?.BecomeTransparent();
                }
            }     
        }


    }
    public Quaternion PlanarRotation => Quaternion.Euler(0, rotationY, 0);
}
