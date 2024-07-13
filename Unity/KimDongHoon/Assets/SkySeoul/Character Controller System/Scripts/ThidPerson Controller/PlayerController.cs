using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f; // 이동 속도
    [SerializeField] float runSpeed = 15f; // 달리기 속도
    [SerializeField] float rotSpeed = 500f; // 회전 속도
    [SerializeField] float jumpPower = 10f; // 점프 파워

    [Header("Ground Check Values")]
    [SerializeField] float groundCheckRadius = 0.2f; // 땅 체크 길이
    [SerializeField] Vector3 groundCheckOffset;
    [SerializeField] LayerMask groundLayer;


    [Header("Ropling")]
    public bool freeze;
    public bool activeGrapple;
    private Vector3 velocityToSet;
    private bool enableMovementOnNextTouch;

    bool isGrounded;
    bool hasCotnrol = true;

    public bool InAction { get; private set; }
    public bool IsHaning { get; set; }

    Vector3 desiredMoveDir;
    Vector3 moveDir;
    Vector3 velocity;
    float applyMoveSpeed;

    public bool IsOnLedge { get; set; }
    public LedgeData LedgeData { get; set; }

    float ySpeed;

    public CameraController cameraController;
    Animator animator;
    CharacterController characterController;
    EnvironmentScanner environmentScanner;
    Rigidbody rigid;
    Quaternion targetRotation;

    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        characterController = GetComponent<CharacterController>();
        environmentScanner = GetComponent<EnvironmentScanner>();
    }

    private void Update()
    {
        MovePlayer();
        SpeedLimt();
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotSpeed * Time.deltaTime);

        
    }

    void MovePlayer()
    {
        if (freeze)
        {
            rigid.velocity = Vector3.zero;
        }

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        float moveAmount = Mathf.Clamp01(Mathf.Abs(h) + Mathf.Abs(v));

        var moveInput = (new Vector3(h, 0, v)).normalized;

        desiredMoveDir = cameraController.PlanarRotation * moveInput;
        moveDir = desiredMoveDir;

        if (!hasCotnrol)
            return;

        if (IsHaning) return;

        GroundCheck();
        animator.SetBool("isGrounded", isGrounded);

        if (Input.GetKey(KeyCode.LeftShift))
        {
            applyMoveSpeed = runSpeed;
            moveAmount += 1;
        }
        else
        {
            applyMoveSpeed = moveSpeed;
        }

        if (isGrounded)
        {
            velocity = desiredMoveDir * applyMoveSpeed;

            IsOnLedge = environmentScanner.ObstacleLedgeCheck(desiredMoveDir, out LedgeData ledgeData);
            if (IsOnLedge)
            {
                LedgeData = ledgeData;

                LedgeMovement();
            }

            if (Input.GetKeyDown(KeyCode.F))
            {
                Jump();
            }

            animator.SetFloat("moveAmount", moveAmount, 0.2f, Time.deltaTime);
            rigid.AddForce(velocity, ForceMode.Force);
        }

        if (moveAmount > 0 && moveDir.magnitude > 0.2f)
        {
            targetRotation = Quaternion.LookRotation(moveDir);
        }
    }

    void SpeedLimt()
    {
        if (activeGrapple) return;

        Vector3 flatVel = new Vector3(rigid.velocity.x, 0f, rigid.velocity.z);

        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rigid.velocity = new Vector3(limitedVel.x, rigid.velocity.y, limitedVel.z);
        }
    }

    void Jump()
    {
        animator.SetTrigger("Jump");

        rigid.velocity = new Vector3(rigid.velocity.x, 0f, rigid.velocity.z);

        rigid.AddForce(transform.up * jumpPower, ForceMode.Impulse);
    }

    void GroundCheck()
    {
       //isGrounded = Physics.CheckSphere(transform.TransformPoint(groundCheckOffset), groundCheckRadius, groundLayer);
       isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckRadius * 0.5f + 0.2f, groundLayer);
    }

    void LedgeMovement()
    {
        float angle = Vector3.Angle(LedgeData.surfaceHit.normal, desiredMoveDir);

        if (angle < 90)
        {
            velocity = Vector3.zero;
            moveDir = Vector3.zero;
        }
    }

    public void SetControl(bool _hasControl)
    {
        this.hasCotnrol = _hasControl;
        characterController.enabled = hasCotnrol;

        if (!hasCotnrol)
        {
            animator.SetFloat("moveAmount", 0f);
            targetRotation = transform.rotation;
        }

    }

    public void EnableCharacterController(bool enabled)
    {
        characterController.enabled = enabled;
    }

    public void ResetTargetRotation()
    {
        targetRotation = transform.rotation;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 1, 0, 0.5f);
        Gizmos.DrawSphere(transform.TransformPoint(groundCheckOffset), groundCheckRadius);
    }

    public float RotSpeed => rotSpeed;

    public bool IsGrounded => isGrounded;


    #region RoplingMethod
    public void JumpToPoSition(Vector3 targetPosition, float trajectoryHeight, float delayTime)
    {
        activeGrapple = true;

        velocityToSet = CalculateJumpVelocity(transform.position, targetPosition, trajectoryHeight);
        Invoke(nameof(SetVelocity), delayTime);

        Invoke(nameof(ResetRestrictions), 3f);
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

    public void ResetRestrictions()
    {
        activeGrapple = false;
    }

    private void SetVelocity()
    {
        enableMovementOnNextTouch = true;
        rigid.velocity = velocityToSet;
    }
    #endregion

    private void OnCollisionEnter(Collision collision)
    {
        if (enableMovementOnNextTouch)
        {
            enableMovementOnNextTouch = false;
            ResetRestrictions();

            GetComponent<Grappling>().StopGrapple();
        }
    }

    public IEnumerator DoAction(string animName, MatchTargetParams matchParams=null, Quaternion targetRotation= new Quaternion(), bool rotate = false,
        float postDelay =0f, bool mirror=false)
    {
        InAction = true;

        animator.SetBool("mirrorAction", mirror);
        animator.CrossFadeInFixedTime(animName, 0.2f); // 애니 메이션 완료를 알리는 방법? => 직접 애니메이션 실행 시간을 확인한다
        yield return null;

        var animState = animator.GetNextAnimatorStateInfo(0);
        if (!animState.IsName(animName))
            Debug.LogError("파쿠르 애니메이션의 이름이 올바르지 않습니다.");

        float rotateStartTime = (matchParams != null) ? matchParams.startTime : 0f;

        //yield return new WaitForSeconds(animState.length); // 애니메이션별 대기 시간을 저장하는 변수를 지정한다.
        // while 반복문과 같은 내용이므로 제거

        float timer = 0f;
        while (timer <= animState.length)
        {
            timer += Time.deltaTime;
            float normalizedTime = timer / animState.length;

            // player의 rotate를 변환해준다.
            if (rotate && normalizedTime > rotateStartTime)
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotSpeed * Time.deltaTime);

            if (matchParams != null)
                MatchTarget(matchParams);

            if (animator.IsInTransition(0) && timer > 0.5f)
                break;

            yield return null;
        }

        yield return new WaitForSeconds(postDelay);

        InAction = false;
    }

    void MatchTarget(MatchTargetParams mp)
    {
        if (animator.isMatchingTarget) return;

        animator.MatchTarget(mp.pos, transform.rotation, mp.bodyPart, new MatchTargetWeightMask(mp.posWeight, 0),
            mp.startTime, mp.targetTime);
    }
}

public class MatchTargetParams
{
    public Vector3 pos;
    public AvatarTarget bodyPart;
    public Vector3 posWeight;
    public float startTime;
    public float targetTime;
}
