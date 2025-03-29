using UnityEngine;
using UnityEngine.UI;

public class CompassSystem : MonoBehaviour
{
    [Header("指南针组件")]
    public Transform compassBase;        // 底座对象
    public Transform compassNeedle;      // 指针对象
    public Transform player;             // 玩家对象
    public RawImage compassUI;           // UI显示组件
    public Camera compassCamera;         // 渲染指南针的相机

    [Header("磁铁影响")]
    public Transform magnetTransform;    // 磁铁对象（可选）
    [Tooltip("磁力影响范围")]
    public float magneticInfluence = 5f; // 磁力影响的最大范围
    [Tooltip("磁力强度")]
    [Range(0f, 1f)]
    public float magneticStrength = 0.5f; // 磁力影响强度（0=无影响，1=完全吸引）
    public float Temp;

    [Header("设置")]
    public Vector2Int renderTextureSize = new Vector2Int(256, 256);

    [Header("平滑设置")]
    public bool enableSmoothRotation = true;
    [Range(0.01f, 1f)]
    public float rotationSmoothness = 0.1f;
    [Range(10f, 360f)]
    public float maxRotationSpeed = 180f;

    private RenderTexture compassRenderTexture;
    private float currentAngle;
 // 记录指南针当前角度


    void Start()
    {
        SetupRenderTexture();
        if (!ValidateComponents())
        {
            enabled = false;
            return;
        }
        SetupCompassCamera();
    }
        private void SetupRenderTexture()
    {
        if (compassCamera != null && compassUI != null)
        {
            compassRenderTexture = new RenderTexture(renderTextureSize.x, renderTextureSize.y, 16);
            compassRenderTexture.Create();
            compassCamera.targetTexture = compassRenderTexture;
            SetupCompassCamera();
            compassUI.texture = compassRenderTexture;
        }
    }
    private void SetupCompassCamera()
    {
        if (compassCamera == null) return;
        compassCamera.enabled = true;
        compassCamera.depth = 10;
        compassCamera.clearFlags = CameraClearFlags.Depth;
        compassCamera.cullingMask = 1 << LayerMask.NameToLayer("sinan");
        compassCamera.renderingPath = RenderingPath.Forward;
        compassCamera.nearClipPlane = 0.01f;
        compassCamera.farClipPlane = 10f;
    }

    private bool ValidateComponents()
    {
        if (compassBase == null) { Debug.LogError("未设置指南针底座对象！"); return false; }
        if (compassNeedle == null) { Debug.LogError("未设置指南针指针对象！"); return false; }
        if (player == null) { Debug.LogError("未设置玩家对象！"); return false; }
        if (compassCamera == null) { Debug.LogError("未设置指南针相机！"); return false; }
        if (compassUI == null) { Debug.LogError("未设置指南针UI显示组件！"); return false; }
        return true;
    }

    void Update()
    {
        if (compassNeedle != null && player != null)
        {
            float targetAngle = GetRelativeMagnetAngle();

            // 平滑旋转处理
            if (enableSmoothRotation)
            {
                float angleDifference = GetAngleDifference(currentAngle, targetAngle);
                float maxDeltaAngle = maxRotationSpeed * Time.deltaTime;
                float smoothDelta = Mathf.Clamp(angleDifference * rotationSmoothness, -maxDeltaAngle, maxDeltaAngle);
                currentAngle += smoothDelta;
                compassNeedle.transform.rotation = Quaternion.Euler(-90, 0, currentAngle);
            }
            else
            {
                currentAngle = targetAngle;
                compassNeedle.transform.rotation = Quaternion.Euler(-90, 0, currentAngle );
            }

            // 应用旋转到指南针指针
            
        }
    }

    // 计算磁铁相对玩家的角度，并转换为指南针角度
    private float GetRelativeMagnetAngle()
    {
        if (magnetTransform != null) // 确保磁铁存在
        {
            Vector3 toMagnet = magnetTransform.position - player.position; // 计算玩家到磁铁的方向
            float magnetAngle = Mathf.Atan2(toMagnet.x, toMagnet.z) * Mathf.Rad2Deg; // 计算磁铁的世界角度
            return (magnetAngle - player.eulerAngles.y+Temp) % 360; // 确保角度在 0 - 360 之间
        }
        else
        {
            return (0 - player.eulerAngles.y + 360) % 360; // 磁铁销毁后指南针指向世界正北（Z轴方向）
        }

    }

    // 计算最短旋转角度，避免指南针卡顿
    private float GetAngleDifference(float fromAngle, float toAngle)
    {
        float diff = toAngle - fromAngle;
        while (diff > 180) diff -= 360;
        while (diff < -180) diff += 360;
        return diff;
    }
}
