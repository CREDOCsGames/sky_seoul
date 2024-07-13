using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playerable : MonoBehaviour
{
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    private float applySpeed;

    [SerializeField] private float jumpForce;

    // 상태 변수
    private bool isRun = false;
    private bool isGround = true;
    private bool isWall = false;
    public bool freeze;
    public bool activeGrapple;

    // 착지여부 확인 콜라이더
    private CapsuleCollider capsuleCollider;

    [SerializeField] private float lookSensitivity; // 카메라의 민감도

    [SerializeField] private float cameraRotationLimit; // 카메라의 최대 각도
    private float currentCameraRotationX = 0f; // 0은 정면을 바라보는 값, 45(각도 만큼 위로 본다)

    [SerializeField] private Camera theCamera;

    Rigidbody rigid;
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
        rigid = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        applySpeed = walkSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (freeze)
        {
            rigid.velocity = Vector3.zero;
        }
        else
        {
            IsGround();
            TryJump();
            TryRun();
            CameraRotation();
            CharacterRotation();
        }
    }

    private void FixedUpdate()
    {
        if (freeze)
        {

        }
        else
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
            {
                Move();
            }
            else
            {
                anim.SetBool("isWalk", false);
            }
        }
    }



    private void IsGround()
    {
        isGround = Physics.Raycast(transform.position, Vector3.down, capsuleCollider.bounds.extents.y+ 0.1f);  // Vector3 transform의 차이.. 월드 값과 로컬 값의 차이가 있다.!
            // 캡슐콜라이더의 bounds(영역)의 y값의 extents(half)값
            // 정확히 반으로 하면 계단 같은 장소에서 문제가 발생할 수 있다 => 오차수정을 위해 값을 더해준다.
    }

    private void TryJump()
    {
        if (Input.GetKeyDown(KeyCode.Space)&& isGround)
        {
            Jump();
        }
    }

    private void Jump()
    {
        anim.SetTrigger("doJump");
        rigid.velocity = transform.up * jumpForce;
    }

    private bool enableMovementOnNextTouch;

    public void JumpToPoSition(Vector3 targetPosition, float trajectoryHeight)
    {
        activeGrapple = true;

        velocityToSet = CalculateJumpVelocity(transform.position, targetPosition, trajectoryHeight);
        Invoke(nameof(SetVelocity), 0.1f);

        Invoke(nameof(ResetRestrictions), 3f);
    }

    private Vector3 velocityToSet;

    private void SetVelocity()
    {
        enableMovementOnNextTouch = true;
        rigid.velocity = velocityToSet;
    }

    public void ResetRestrictions()
    {
        activeGrapple = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (enableMovementOnNextTouch)
        {
            enableMovementOnNextTouch = false;
            ResetRestrictions();

            GetComponentInChildren<Hooking>().StopGrapple();  
        }
    }

    private void TryRun()
    {
        if (activeGrapple) return;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            anim.SetBool("isRun", true);
            Running();
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            anim.SetBool("isRun", false);
            RunningCancel();
        }
    }

    private void Running()
    {
        isRun = true;
        applySpeed = runSpeed;
    }

    private void RunningCancel()
    {
        isRun = false;
        applySpeed = walkSpeed;
    }

    private void Move()
    {
        if (activeGrapple) return;

        anim.SetBool("isWalk", true);
        float _moveDirX = Input.GetAxisRaw("Horizontal"); //  왼쪽 -1, 기본 0 , 오른쪽 1값을 출력한다.
        float _moveDirZ = Input.GetAxisRaw("Vertical"); // 위 1, 기본 0 , 아래 -1 값을 출력한다.

        Vector3 _moveHorizontal = transform.right * _moveDirX;    //(1,0,0) / (0,0,0) / (-1,0,0) 백터값이 출력
        Vector3 _moveVertical = transform.forward * _moveDirZ;    // (0,0,1) / (0,0,0) / (0,0,-1) 백터값이 출력

        Vector3 _velocity = (_moveHorizontal + _moveVertical).normalized * applySpeed;    // (1,0,0) (0,0,1) = (1,0,1) 2 정규화를 통해 1초에 얼마만큼 움직일 수 있는지 값으로 초기화 해준다.

        rigid.MovePosition(transform.position + _velocity * Time.deltaTime); // rigid를 활용한 캐릭터 이동 현재 위치에서 매 frame마다 이동시킨다.  Tiem.deltaTime의 값은 대략 0.016이다.      
    }

    private void CharacterRotation() // 캐릭터 좌우 회전
    {
        float _yRotation = Input.GetAxisRaw("Mouse X"); // y축로 회전 시켜야 좌우 회전을 한다. yRotation값을 변경시키는 이유.
        Vector3 _chracterRotationY = new Vector3(0f, _yRotation, 0f) * lookSensitivity;
        rigid.MoveRotation(rigid.rotation * Quaternion.Euler(_chracterRotationY)); // 마우스에 의해 정해진 값을 쿼터니언값으로 변경시켜주는 함수.
        //theCamera.transform.localEulerAngles = _chracterRotationY;
    }

    private void CameraRotation() // 마우스 상하 움직임
    {
        float _xRotation = Input.GetAxisRaw("Mouse Y"); // 마우스는 2차원 X,Y공간 유니티는 3차원 공간의 X값.(X,Y,Z)중 X
        float _cameraRotationX = _xRotation * lookSensitivity;
        currentCameraRotationX -= _cameraRotationX;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit); // 각도가 반전되는 이유? 카메라 각의 반전을 고려안해서. 올라갈때 빼주고 내려갈때 더해준다.

        theCamera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
    }

    public Vector3 CalculateJumpVelocity(Vector3 startPoint, Vector3 endPoint, float trajectoryHeight)
    {
        float graviry = Physics.gravity.y;
        float displacementY = endPoint.y - startPoint.y;
        Vector3 displacementXZ = new Vector3(endPoint.x - startPoint.x, 0f, endPoint.z - startPoint.z);

        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * graviry * trajectoryHeight);
        Vector3 velocityXZ = displacementXZ / (Mathf.Sqrt(-2 * trajectoryHeight / graviry)
            + Mathf.Sqrt(2 * (displacementY - trajectoryHeight) / graviry));

        return velocityXZ + velocityY;
    }

}
