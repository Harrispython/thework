using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class LensFlareController : MonoBehaviour
{
    // Lens Flare组件引用
    private LensFlareComponentSRP lensFlare;
    
    [Header("【时间轴启动】")]
    [SerializeField]
    private GameObject target;
    
    [Header("【光晕缩放控制】")]
    [Tooltip("最小缩放比例")]
    [SerializeField]
    [Range(0f, 1f)]
    private float minScale = 0.2f;
    
    [Tooltip("最大缩放比例")]
    [SerializeField]
    [Range(1f, 30f)]
    private float maxScale = 10.0f;
    
    [Tooltip("缩放变化速度，值越大变化越快")]
    [SerializeField]
    [Range(0.1f, 5.0f)]
    private float scaleChangeSpeed = 1.0f;
    
    [Header("【光晕强度控制】")]
    [Tooltip("最小强度")]
    [SerializeField]
    [Range(0f, 1f)]
    private float minIntensity = 0.2f;
    
    [Tooltip("最大强度")]
    [SerializeField]
    [Range(1f, 30f)]
    private float maxIntensity = 15.0f;
    
    [Tooltip("强度变化速度，值越大变化越快")]
    [SerializeField]
    [Range(0.1f, 5.0f)]
    private float intensityChangeSpeed = 0.5f;
    
    // 内部使用的插值计时器
    private float currentTime = 0f;
    private bool isTransitioning = false;
    
    void Start()
    {
        // 获取Lens Flare组件
        lensFlare = GetComponent<LensFlareComponentSRP>();
        
        if (lensFlare == null)
        {
            Debug.LogError("未找到Lens Flare (SRP)组件！请确保此脚本挂载在带有Lens Flare组件的游戏对象上。");
            enabled = false;
            return;
        }
        
        // 设置初始值
        lensFlare.scale = minScale;
        lensFlare.intensity = minIntensity;
        
        // 开始渐变
        StartTransition();
    }
    
    void Update()
    {
        if (!isTransitioning) return;
        
        // 更新时间
        currentTime += Time.deltaTime;
        
        // 计算渐变进度（0到1之间）
        float scaleProgress = Mathf.Clamp01(currentTime * scaleChangeSpeed);
        float intensityProgress = Mathf.Clamp01(currentTime * intensityChangeSpeed);
        
        // 应用缩放和强度值
        lensFlare.scale = Mathf.Lerp(minScale, maxScale, scaleProgress);
        lensFlare.intensity = Mathf.Lerp(minIntensity, maxIntensity, intensityProgress);
        
        // 检查是否完成渐变
        if (scaleProgress >= 1.0f && intensityProgress >= 1.0f)
        {
            isTransitioning = false;
        }if (scaleProgress >= 0.3f && intensityProgress >= 0.3f)
        {
            target.SetActive(true);
        }
    }
    // 开始渐变效果
    public void StartTransition()
    {
        currentTime = 0f;
        isTransitioning = true;
        lensFlare.scale = minScale;
        lensFlare.intensity = minIntensity;
    }
    
    // 重置到初始状态
    public void Reset()
    {
        currentTime = 0f;
        isTransitioning = false;
        lensFlare.scale = minScale;
        lensFlare.intensity = minIntensity;
    }
    
    // 立即设置到最大值
    public void SetToMax()
    {
        currentTime = 0f;
        isTransitioning = false;
        lensFlare.scale = maxScale;
        lensFlare.intensity = maxIntensity;
    }
    
    // 设置变化速度
    public void SetChangeSpeed(float scaleSpeed, float intensitySpeed)
    {
        scaleChangeSpeed = Mathf.Clamp(scaleSpeed, 0.1f, 5.0f);
        intensityChangeSpeed = Mathf.Clamp(intensitySpeed, 0.1f, 5.0f);
    }
    
    // 设置最大值
    public void SetMaxValues(float scale, float intensity)
    {
        maxScale = Mathf.Clamp(scale, 1f, 30f);
        maxIntensity = Mathf.Clamp(intensity, 1f, 30f);
    }
}