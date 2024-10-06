using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;  // InputSystem必备

public class PlayerController : MonoBehaviour
{
    [Header("移动")]
    public float maxSpeed;
    [HideInInspector] public Vector3 currentSpeedVec;
    public float currentSpeed;
    public Vector3 inputDir;
    public Transform orientation;
    public float airArgs; // 在空中的速度系数
    public float staminaArgs;   // 体力值影响最大速度的系数

    [Header("跳跃")]
    public float jumpForce;
    public float jumpCooldown;
    public bool isReadyToJump = true;

    [Header("触地检测")]
    public float playerHeight;  // 人物模型的高度，取碰撞体的高度就行
    public LayerMask groundLayer; //地面所在图层
    public bool isGrounded = true;
    public float groundDrag = 1f;

    [HideInInspector] public Animator anim;
    [HideInInspector] public Rigidbody rb;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
    }
    void Start()
    {
        
    }
    void Update()
    {
        // 检测是否触地，射线（起点，方向，长度，图层），如果射线碰到物体就返回true
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 1.0f, groundLayer);
        
        Jump();
        Fire();
        SpeedLimit();

        // 地面和空中的摩擦力
        if (isGrounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;
    }
    private void FixedUpdate()
    {
        Move();
    }
    private void Move()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        inputDir = orientation.forward * verticalInput + orientation.right * horizontalInput;
        Vector3 speed = maxSpeed * inputDir.normalized;
        
        if (isGrounded)
        {
            rb.AddForce(speed, ForceMode.Force);
        }
        else
        {
            rb.AddForce(speed * airArgs, ForceMode.Force);
        }

        anim.SetFloat("speed", speed.magnitude);
    }
    private void SpeedLimit()
    {
        // 限制x z方向的最大线速度
        currentSpeedVec = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        currentSpeed = currentSpeedVec.magnitude;

        if (currentSpeed > maxSpeed * staminaArgs)
        {
            Vector3 limitedSpeed = currentSpeedVec.normalized * maxSpeed;
            rb.velocity = new Vector3(limitedSpeed.x, rb.velocity.y, limitedSpeed.z);
        }
    }
    private void Jump()
    {
        if(Input.GetAxis("Jump") != 0 && isGrounded && isReadyToJump)
        {
            isReadyToJump = false;

            // 清零y方向的速度
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            
            rb.AddForce(jumpForce * transform.up, ForceMode.Impulse);
            Invoke(nameof(ResetJump), jumpCooldown);    // 冷却
        }
    }
    private void ResetJump()
    {
        isReadyToJump = true;
    }
    private void Fire()
    {
        if (Input.GetAxis("Fire1") != 0)
        {
            anim.SetBool("isAttack", true);
        }
        else
        {
            anim.SetBool("isAttack", false);
        }
    }

    public void Hurt()
    {
        anim.SetTrigger("hurting");
    }
    public void Die()
    {
        anim.SetBool("die", true);
    }
}
