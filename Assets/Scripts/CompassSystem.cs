using UnityEngine;
using UnityEngine.UI;

public class CompassSystem : MonoBehaviour
{
    [Header("指南针组件")]
    [Tooltip("指南针底座")]
    public Transform compassBase;        // 底座对象
    [Tooltip("指南针指针")]
    public Transform compassNeedle;      // 指针对象
    [Tooltip("玩家Transform")]
    public Transform player;             // 玩家对象
    [Tooltip("指南针UI显示")]
    public RawImage compassUI;          // UI显示组件
    [Tooltip("渲染指南针的相机")]
    public Camera compassCamera;         // 用于渲染指南针的相机

    [Header("设置")] 
    [Tooltip("底座旋转角度")]
    public Vector3 baseRotation = new Vector3(0, 0, 0);  // 底座的旋转角度
    [Tooltip("指南针朝向，默认为北方")]
    public Vector3 compassDirection = Vector3.forward;  // 指南针朝向
    [Tooltip("渲染纹理的分辨率")]
    public Vector2Int renderTextureSize = new Vector2Int(256, 256);  // 渲染纹理的分辨率
    
    [Header("平滑设置")]
    [Tooltip("启用平滑旋转")]
    public bool enableSmoothRotation = true;  // 是否启用平滑旋转
    [Tooltip("旋转平滑度（值越小越平滑）")]
    [Range(0.01f, 1f)]
    public float rotationSmoothness = 0.1f;  // 旋转平滑程度
    [Tooltip("每秒最大旋转角度")]
    [Range(10f, 360f)]
    public float maxRotationSpeed = 180f;  // 每秒最大旋转角度

    private RenderTexture compassRenderTexture;  // 渲染纹理
    private Quaternion targetRotation;  // 目标旋转
    private Quaternion currentRotation;  // 当前旋转
    private float currentAngle;  // 当前角度

    void Start()
    {
        // 初始化渲染纹理
        SetupRenderTexture();
        
        // 确保组件都已赋值
        if (!ValidateComponents())
        {
            enabled = false;
            return;
        }
        
        // 确保相机已启用，并且在正确的层次上
        if (compassCamera != null)
        {
            SetupCompassCamera();
            
            // 设置层和初始化指南针
            // InitializeCompass();
        }
    }

    void Update()
    {
        if (compassNeedle != null && player != null)
        {
            // 指针根据玩家旋转进行反方向旋转
            float targetAngle = -player.eulerAngles.y;
            
            if (enableSmoothRotation)
            {
                // 计算当前角度和目标角度之间的最短差值
                float angleDifference = GetAngleDifference(currentAngle, targetAngle);
                
                // 计算本帧允许的最大旋转量
                float maxDeltaAngle = maxRotationSpeed * Time.deltaTime;
                
                // 限制单帧旋转角度
                float smoothDelta = Mathf.Clamp(angleDifference * rotationSmoothness, -maxDeltaAngle, maxDeltaAngle);
                
                // 平滑过渡到目标角度
                currentAngle += smoothDelta;
                
                // 应用旋转到指针 - 沿Z轴旋转
                compassNeedle.transform.rotation = Quaternion.Euler(-90,0 , currentAngle);
            }
            else
            {
                // 如果不启用平滑，则直接设置旋转
                currentAngle = targetAngle; // 更新当前角度，以便在重新启用平滑时有正确的初始值
                compassNeedle.transform.rotation = Quaternion.Euler(-90, 0, currentAngle);
            }
        }
    }

    // 初始化渲染纹理
    private void SetupRenderTexture()
    {
        if (compassCamera != null && compassUI != null)
        {
            // 创建渲染纹理
            compassRenderTexture = new RenderTexture(renderTextureSize.x, renderTextureSize.y, 16);
            compassRenderTexture.Create();

            // 设置相机的渲染目标
            compassCamera.targetTexture = compassRenderTexture;
            
            // 设置相机的其他参数
            SetupCompassCamera();

            // 设置UI显示
            compassUI.texture = compassRenderTexture;
        }
    }

    // 设置指南针相机的专用方法
    private void SetupCompassCamera()
    {
        if (compassCamera == null) return;
        
        // 确保相机启用
        compassCamera.enabled = true;
        
        // 设置相机深度为较高值，确保它总是能渲染
        compassCamera.depth = 10;
        
        // 设置清除标志，只清除深度而不清除颜色
        compassCamera.clearFlags = CameraClearFlags.Depth;
        
        // 设置相机只渲染指南针所在的层
        compassCamera.cullingMask = 1 << LayerMask.NameToLayer("sinan");
        
        // 设置相机为正交模式，适合UI渲染
        //compassCamera.orthographic = true;
        
        // 设置相机的渲染顺序为叠加，这样不会覆盖主相机的渲染结果
        compassCamera.renderingPath = RenderingPath.Forward;
        
        // 设置近剪裁面和远剪裁面，避免与主相机冲突
        compassCamera.nearClipPlane = 0.01f;
        compassCamera.farClipPlane = 10f;
    }

    // 验证必要组件是否已赋值
    private bool ValidateComponents()
    {
        if (compassBase == null)
        {
            Debug.LogError("未设置指南针底座对象！");
            return false;
        }

        if (compassNeedle == null)
        {
            Debug.LogError("未设置指南针指针对象！");
            return false;
        }

        if (player == null)
        {
            Debug.LogError("未设置玩家对象！");
            return false;
        }

        if (compassCamera == null)
        {
            Debug.LogError("未设置指南针相机！");
            return false;
        }

        if (compassUI == null)
        {
            Debug.LogError("未设置指南针UI显示组件！");
            return false;
        }

        return true;
    }

    // 清理渲染纹理
    private void OnDestroy()
    {
        if (compassRenderTexture != null)
        {
            compassRenderTexture.Release();
        }
    }

    // 计算两个角度之间的最短差值（处理0度和360度的过渡）
    private float GetAngleDifference(float fromAngle, float toAngle)
    {
        float diff = toAngle - fromAngle;
        
        // 确保差值在-180到180之间，这样可以走最短路径
        while (diff > 180) diff -= 360;
        while (diff < -180) diff += 360;
        
        return diff;
    }
    
    
    // // 初始化指南针状态
    // private void InitializeCompass()
    // {
    //     if (compassBase != null)
    //     {
    //         // 设置底座的初始旋转
    //         compassBase.transform.rotation = Quaternion.Euler(baseRotation);
    //         print(compassNeedle.rotation);
    //
    //     }
    //     
    //     if (compassNeedle != null && player != null)
    //     {
    //         // 设置指针的初始旋转
    //         compassNeedle.transform.rotation = Quaternion.Euler(baseRotation);
    //         print(compassNeedle.rotation);
    //     }
    // }
} 