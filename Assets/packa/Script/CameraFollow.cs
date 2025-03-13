using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("跟随设置")]
    [Tooltip("要跟随的目标")]
    public Transform target; // 要跟随的目标（通常是玩家）
    
    [Tooltip("摄像机与目标的距离")]
    public float distance = 5.0f; // 摄像机与目标的距离
    
    [Tooltip("摄像机高度")]
    public float height = 2.0f; // 摄像机高度
    
    [Tooltip("摄像机平滑跟随速度")]
    public float smoothSpeed = 10.0f; // 摄像机平滑跟随速度
    
    [Header("旋转设置")]
    [Tooltip("是否允许鼠标旋转")]
    public bool enableRotation = true; // 是否允许鼠标旋转
    
    [Tooltip("水平旋转速度")]
    public float horizontalRotationSpeed = 2.0f; // 水平旋转速度
    
    [Tooltip("垂直旋转速度")]
    public float verticalRotationSpeed = 2.0f; // 垂直旋转速度
    
    [Tooltip("垂直旋转最小角度")]
    public float minVerticalAngle = -30.0f; // 垂直旋转最小角度
    
    [Tooltip("垂直旋转最大角度")]
    public float maxVerticalAngle = 60.0f; // 垂直旋转最大角度
    
    [Header("碰撞检测")]
    [Tooltip("是否启用碰撞检测")]
    public bool enableCollisionDetection = true; // 是否启用碰撞检测
    
    [Tooltip("碰撞检测层")]
    public LayerMask collisionLayers; // 碰撞检测层
    
    // 私有变量
    private float currentRotationX = 0f; // 当前水平旋转角度
    private float currentRotationY = 0f; // 当前垂直旋转角度
    private Vector3 finalPosition; // 最终摄像机位置
    private Vector3 smoothVelocity = Vector3.zero; // 平滑速度
    
    // 初始化
    void Start()
    {
        // 如果没有指定目标，尝试查找PlayerController组件
        if (target == null)
        {
            PlayerController playerController = FindObjectOfType<PlayerController>();
            if (playerController != null)
            {
                target = playerController.transform;
                Debug.Log("已自动设置摄像机跟随目标为PlayerController");
            }
            else
            {
                Debug.LogWarning("未设置摄像机跟随目标，请手动设置target属性");
            }
        }
        
        // 初始化旋转角度
        Vector3 angles = transform.eulerAngles;
        currentRotationX = angles.y;
        currentRotationY = angles.x;
    }
    
    // 在Update之后执行，确保在所有移动计算之后更新摄像机位置
    void LateUpdate()
    {
        // 如果没有目标，不执行跟随
        if (target == null)
            return;
        
        // 处理鼠标输入旋转
        if (enableRotation)
        {
            HandleRotationInput();
        }
        
        // 计算理想的摄像机位置
        CalculateCameraPosition();
        
        // 检测碰撞并调整位置
        if (enableCollisionDetection)
        {
            AdjustForCollision();
        }
        
        // 平滑移动摄像机到计算出的位置
        transform.position = Vector3.SmoothDamp(transform.position, finalPosition, ref smoothVelocity, 1f / smoothSpeed);
        
        // 让摄像机始终看向目标
        transform.LookAt(target.position + Vector3.up * height * 0.7f);
    }
    
    // 处理鼠标输入旋转
    void HandleRotationInput()
    {
        // 获取鼠标输入
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        
        // 更新旋转角度
        currentRotationX += mouseX * horizontalRotationSpeed;
        currentRotationY -= mouseY * verticalRotationSpeed; // 注意这里是减，因为鼠标向上是正值
        
        // 限制垂直旋转角度
        currentRotationY = Mathf.Clamp(currentRotationY, minVerticalAngle, maxVerticalAngle);
    }
    
    // 计算摄像机位置
    void CalculateCameraPosition()
    {
        // 计算旋转
        Quaternion rotation = Quaternion.Euler(currentRotationY, currentRotationX, 0);
        
        // 计算摄像机位置（目标位置 - 旋转后的后方向量 * 距离）
        Vector3 targetPosition = target.position + Vector3.up * height;
        finalPosition = targetPosition - rotation * Vector3.forward * distance;
    }
    
    // 检测碰撞并调整位置
    void AdjustForCollision()
    {
        // 从目标到理想摄像机位置发射射线
        Vector3 targetPosition = target.position + Vector3.up * height;
        Vector3 direction = finalPosition - targetPosition;
        float targetDistance = direction.magnitude;
        
        // 检测是否有碰撞
        RaycastHit hit;
        if (Physics.Raycast(targetPosition, direction.normalized, out hit, targetDistance, collisionLayers))
        {
            // 如果有碰撞，调整摄像机位置到碰撞点前一点
            finalPosition = hit.point - direction.normalized * 0.2f;
        }
    }
} 