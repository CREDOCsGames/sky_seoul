using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdCam : MonoBehaviour
{
    public static ThirdCam instance;
    public PlayerController player;
    public Camera cameraObject;
    [SerializeField] Transform cameraPivotTransform;

    // 카메라 퍼포몬스 제어 변수
    [Header("Camera Settings")]
    public float cameraSmoothSpeed = 1;   // 수가 커질 수록 도착하는 시간이 길어진다.
    [SerializeField] private float leftAndRightSpeed = 220;
    [SerializeField] private float upAndDownSpeed = 200;
    [SerializeField] private float minimumPivot = -30;
    [SerializeField] private float maximumPivot = 60;
    [SerializeField] float cameraCollisionRadius = 0.2f;
    [SerializeField] LayerMask collideWithLayer;

    // 카메라 제어 변수
    [Header("Camera Value")]
    private Vector3 cameraVelocity;
    private Vector3 cameraObjectPosition;   // user for camera Collision
    [SerializeField] float rotationX; // Up and Down angle
    [SerializeField] float rotationY; // left and right angle
    // Values used for cmaera collsion
    private float camerzZPosition;
    private float targetCameraZPosition;

    public Quaternion PlanarRotation => Quaternion.Euler(0, rotationY, 0);

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        camerzZPosition = cameraObject.transform.localPosition.z;
    }

    private void FixedUpdate()
    {
        HandleAllCameraAction();
    }

    public void HandleAllCameraAction()
    {
        if (player != null)
        {
            HandleFollowTarget();
            HandleRotations();
            HandleCollision();
        }

    }

    private void HandleFollowTarget()
    {
        Vector3 targetCameraPosition = Vector3.SmoothDamp(transform.position, player.transform.position, ref cameraVelocity, cameraSmoothSpeed * Time.deltaTime);
        transform.position = targetCameraPosition;
    }

    private void HandleRotations()
    {
        //rotationY += (PlayerInputManager.instsance.cameraHorizontalInput * leftAndRightSpeed) * Time.deltaTime;
        rotationY += Input.GetAxis("Mouse X") * leftAndRightSpeed * Time.deltaTime;
        //rotationX -= (PlayerInputManager.instsance.cameraVerticalInput * upAndDownSpeed) * Time.deltaTime;
        rotationX -= Input.GetAxis("Mouse Y") * upAndDownSpeed * Time.deltaTime;
        rotationX = Mathf.Clamp(rotationX, minimumPivot, maximumPivot);


        Vector3 cameraRotation = Vector3.zero;
        Quaternion targetRotation;

        // 좌우 회전
        cameraRotation.y = rotationY;
        targetRotation = Quaternion.Euler(cameraRotation);
        transform.rotation = targetRotation;

        // 위아래 회전
        cameraRotation = Vector3.zero;
        cameraRotation.x = rotationX;
        targetRotation = Quaternion.Euler(cameraRotation);
        cameraPivotTransform.localRotation = targetRotation;
    }

    private void HandleCollision()
    {
        targetCameraZPosition = camerzZPosition;
        RaycastHit hit;
        // 방향 계산
        Vector3 direction = cameraObject.transform.position - cameraPivotTransform.position;
        direction.Normalize();

        // 카메라 앞에 오브젝트가 있다면..
        if (Physics.SphereCast(cameraPivotTransform.position, cameraCollisionRadius, direction, out hit, Mathf.Abs(targetCameraZPosition), collideWithLayer))
        {
            // 충돌한 오브젝트로 부터 거리를 구한다
            float distanceFromHitObject = Vector3.Distance(cameraPivotTransform.position, hit.point);
            // We THEN EQUATE OUR TARGET 2 POSITION TO THE FOLLOWING
            targetCameraZPosition = -(distanceFromHitObject - cameraCollisionRadius);  // 카메라의 위치에서 벽이 가로막힌 Z거리
        }

        // IF OUR TARGET POSITION IS LESS THAN OUR COLLISION RADIUS, WE SUBTRACT OUR COLLISION RADIUS ( SNAP BACK)
        if (Mathf.Abs(targetCameraZPosition) < cameraCollisionRadius)
        {
            targetCameraZPosition = -cameraCollisionRadius;
        }

        // WE THEN APPLY OUR FINAL POSITION USING LERP
        cameraObjectPosition.z = Mathf.Lerp(cameraObject.transform.localPosition.z, targetCameraZPosition, 0.2f);  // 기존 -3(멀리 찍는 위치)에서 오브젝트가 충돌한 위치보다 앞(가까이) 이동
        cameraObject.transform.localPosition = cameraObjectPosition;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        Gizmos.DrawLine(cameraObject.transform.position, cameraPivotTransform.position);
    }

}
