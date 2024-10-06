using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;  // InputSystem�ر�

public class PlayerController : MonoBehaviour
{
    [Header("�ƶ�")]
    public float maxSpeed;
    [HideInInspector] public Vector3 currentSpeedVec;
    public float currentSpeed;
    public Vector3 inputDir;
    public Transform orientation;
    public float airArgs; // �ڿ��е��ٶ�ϵ��
    public float staminaArgs;   // ����ֵӰ������ٶȵ�ϵ��

    [Header("��Ծ")]
    public float jumpForce;
    public float jumpCooldown;
    public bool isReadyToJump = true;

    [Header("���ؼ��")]
    public float playerHeight;  // ����ģ�͵ĸ߶ȣ�ȡ��ײ��ĸ߶Ⱦ���
    public LayerMask groundLayer; //��������ͼ��
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
        // ����Ƿ񴥵أ����ߣ���㣬���򣬳��ȣ�ͼ�㣩�����������������ͷ���true
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 1.0f, groundLayer);
        
        Jump();
        Fire();
        SpeedLimit();

        // ����Ϳ��е�Ħ����
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
        // ����x z�����������ٶ�
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

            // ����y������ٶ�
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            
            rb.AddForce(jumpForce * transform.up, ForceMode.Impulse);
            Invoke(nameof(ResetJump), jumpCooldown);    // ��ȴ
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
