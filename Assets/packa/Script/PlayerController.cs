using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("移动设置")]
    [Tooltip("角色移动速度")]
    public float moveSpeed = 5.0f; // 移动速度
    
    [Tooltip("角色旋转速度")]
    public float rotationSpeed = 10.0f; // 旋转速度
    
    [Tooltip("角色跳跃力度")]
    public float jumpForce = 5.0f; // 跳跃力度
    
    [Header("地面检测")]
    [Tooltip("地面检测距离")]
    public float groundCheckDistance = 0.2f; // 地面检测距离
    
    [Tooltip("地面检测层")]
    public LayerMask groundLayer; // 地面层
    
    // 私有变量
    private CharacterController characterController; // 角色控制器组件
    private Vector3 moveDirection; // 移动方向
    private float verticalVelocity; // 垂直速度
    private bool isGrounded; // 是否在地面上
    private Transform cameraTransform; // 摄像机变换组件
    
    // 初始化
    void Start()
    {
        // 获取角色控制器组件
        characterController = GetComponent<CharacterController>();
        if (characterController == null)
        {
            // 如果没有CharacterController组件，添加一个
            characterController = gameObject.AddComponent<CharacterController>();
            Debug.Log("已自动添加CharacterController组件");
        }
        
        // 获取主摄像机的变换组件
        cameraTransform = Camera.main.transform;
        
        // 锁定并隐藏鼠标光标（可选）
        // Cursor.lockState = CursorLockMode.Locked;
        // Cursor.visible = false;
    }
    
    // 每帧更新
    void Update()
    {
        // 检查是否在地面上
        CheckGrounded();
        
        // 处理移动输入
        HandleMovement();
        
        // 处理跳跃输入
        HandleJump();
        
        // 应用重力
        ApplyGravity();
        
        // 移动角色
        MoveCharacter();
    }
    
    // 检查是否在地面上
    void CheckGrounded()
    {
        isGrounded = Physics.CheckSphere(transform.position - new Vector3(0, characterController.height / 2, 0), 
                                         groundCheckDistance, groundLayer);
        
        // 如果在地面上且垂直速度为负，重置垂直速度
        if (isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = -1f;
        }
    }
    
    // 处理移动输入
    void HandleMovement()
    {
        // 获取水平和垂直输入
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        
        // 基于输入创建移动向量
        Vector3 movementInput = new Vector3(horizontalInput, 0, verticalInput);
        
        // 如果有摄像机，根据摄像机方向调整移动方向
        if (cameraTransform != null)
        {
            // 获取摄像机前方和右方向（忽略Y轴）
            Vector3 forward = cameraTransform.forward;
            Vector3 right = cameraTransform.right;
            
            forward.y = 0;
            right.y = 0;
            forward.Normalize();
            right.Normalize();
            
            // 根据摄像机方向计算移动方向
            moveDirection = forward * verticalInput + right * horizontalInput;
        }
        else
        {
            // 如果没有摄像机，直接使用世界坐标系
            moveDirection = movementInput;
        }
        
        // 标准化移动方向（如果有输入）
        if (moveDirection.magnitude > 0.1f)
        {
            moveDirection.Normalize();
            
            // 平滑旋转角色面向移动方向
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
    
    // 处理跳跃输入
    void HandleJump()
    {
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            // 应用跳跃力
            verticalVelocity = Mathf.Sqrt(jumpForce * -2f * Physics.gravity.y);
        }
    }
    
    // 应用重力
    void ApplyGravity()
    {
        // 应用重力
        verticalVelocity += Physics.gravity.y * Time.deltaTime;
    }
    
    // 移动角色
    void MoveCharacter()
    {
        // 计算最终移动向量（水平移动 + 垂直速度）
        Vector3 movement = moveDirection * moveSpeed;
        movement.y = verticalVelocity;
        
        // 移动角色
        characterController.Move(movement * Time.deltaTime);
    }
} 