using UnityEngine;

public class PlatformRotator : MonoBehaviour
{
    [Header("旋转设置")]
    [Tooltip("旋转速度")]
    public float rotationSpeed = 100f;

    private PlatformSelector platformSelector;
    private CameraViewSwitch viewSwitch;

    private void Start()
    {
        platformSelector = GetComponent<PlatformSelector>();
        viewSwitch = GetComponent<CameraViewSwitch>();

        if (platformSelector == null)
        {
            Debug.LogError("未找到PlatformSelector组件！");
        }
        if (viewSwitch == null)
        {
            Debug.LogError("未找到CameraViewSwitch组件！");
        }
    }

    private void Update()
    {
        // 检查是否在第一人称模式且有选中的平台
        if (viewSwitch != null && viewSwitch.IsInFirstPerson() && platformSelector != null)
        {
            Transform selectedPlatform = platformSelector.GetSelectedPlatform();
            if (selectedPlatform != null)
            {
                // 获取水平输入（A/D键）
                float horizontalInput = Input.GetAxis("Horizontal");
                
                if (horizontalInput != 0)
                {
                    // 绕世界Y轴旋转选中的平台
                    selectedPlatform.Rotate(Vector3.up, horizontalInput * rotationSpeed * Time.deltaTime, Space.World);
                }
            }
        }
    }
} 