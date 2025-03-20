using UnityEngine;
using UnityEngine.UI; // 添加UI命名空间
using System.Collections;
using HighlightingSystem.Demo; // 添加协程支持

public class LightColorController : MonoBehaviour
{
    // 灯光组件引用
    private Light lightComponent;
    
    [Header("【颜色控制】")]
    [Tooltip("颜色变化的速度，值越大变化越快")]
    [SerializeField]
    [Range(0.1f, 5.0f)]
    private float colorChangeSpeed = 1.0f;
    
    [Tooltip("起始颜色")]
    [SerializeField]
    private Color startColor = Color.red;
    
    [Tooltip("目标颜色")]
    [SerializeField]
    private Color endColor = Color.blue;
    
    [Header("【强度控制】")]
    [Tooltip("是否启用强度渐变效果")]
    [SerializeField]
    private bool enableIntensityPingPong = true;
    
    [Tooltip("最小发光强度")]
    [SerializeField]
    [Range(0f, 5.0f)]
    private float minIntensity = 0.5f;
    
    [Tooltip("最大发光强度")]
    [SerializeField]
    [Range(0f, 5.0f)]
    private float maxIntensity = 2.0f;
    
    [Tooltip("强度变化速度，值越大变化越快")]
    [SerializeField]
    [Range(0.1f, 5.0f)]
    private float intensityChangeSpeed = 0.5f;
    
    [Header("【范围控制】")]
    [Tooltip("是否启用光照范围渐变效果")]
    [SerializeField]
    private bool enableRangePingPong = false;
    
    [Tooltip("最小光照范围")]
    [SerializeField]
    [Range(1f, 20f)]
    private float minRange = 5f;
    
    [Tooltip("最大光照范围")]
    [SerializeField]
    [Range(1f, 20f)]
    private float maxRange = 10f;
    
    [Tooltip("范围变化速度，值越大变化越快")]
    [SerializeField]
    [Range(0.1f, 5.0f)]
    private float rangeChangeSpeed = 0.3f;
    
    // 内部使用的插值计时器
    private float colorLerpTime = 0f;
    private float intensityLerpTime = 0f;
    private float rangeLerpTime = 0f;
    
    [Header("【UI提示控制】")]
    [SerializeField]
    private GameObject promptUI; // UI提示预制体
    [SerializeField]
    private Vector3 promptOffset = new Vector3(0, 2, 0); // UI显示位置的偏移量
    
    private bool isTransitioning = false; // 是否正在过渡到结束颜色
    private bool isActive = true; // 控制颜色循环是否激活
    private Canvas worldSpaceCanvas; // 世界空间UI画布
    private GameObject currentPrompt; // 当前显示的提示UI
    
    [Header("【组件控制】")]
    [Tooltip("父物体上的旋转控制器组件")]
    private RotationController rotationController;
    [Tooltip("父物体上的动画控制器组件")]
    private Animator animator;
    
    void Start()
    {
        // 获取灯光组件
        lightComponent = GetComponent<Light>();
        
        // 获取父物体上的组件
        if (transform.parent != null)
        {
            rotationController = transform.parent.GetComponent<RotationController>();
            animator = transform.parent.GetComponent<Animator>();
            
            // 初始时禁用这些组件
            if (rotationController != null) rotationController.enabled = false;
            if (animator != null) animator.enabled = false;
        }
        
        if (lightComponent == null)
        {
            Debug.LogError("未找到Light组件！请确保此脚本挂载在带有Light组件的游戏对象上。");
            enabled = false;
            return;
        }
        
        // 设置初始值
        lightComponent.color = startColor;
        lightComponent.intensity = minIntensity;
        lightComponent.range = minRange;
        
        // 延迟显示UI提示
        StartCoroutine(ShowPromptAfterDelay());
    }
    
    void Update()
    {
        if (!isActive) return; // 如果不活跃，直接返回
        
        if (isTransitioning)
        {
            // 缓慢过渡到结束颜色
            colorLerpTime = Mathf.MoveTowards(colorLerpTime, 1.0f, Time.deltaTime * colorChangeSpeed * 0.5f);
            lightComponent.color = Color.Lerp(startColor, endColor, colorLerpTime);
            
            // 当达到结束颜色时停止过渡
            if (Mathf.Approximately(colorLerpTime, 1.0f))
            {
                isActive = false;
                isTransitioning = false;
            }
        }
        else
        {
            // 正常的颜色PingPong效果
            colorLerpTime = Mathf.PingPong(Time.time * colorChangeSpeed, 1.0f);
            lightComponent.color = Color.Lerp(startColor, endColor, colorLerpTime);
        }
        
        // 检测E键输入
        if (Input.GetKeyDown(KeyCode.E) && currentPrompt != null)
        {
            isTransitioning = true;
            EnableParentComponents(); // 启用父物体组件
        }
        
        // 强度PingPong效果
        if (enableIntensityPingPong)
        {
            intensityLerpTime = Mathf.PingPong(Time.time * intensityChangeSpeed, 1.0f);
            float currentIntensity = Mathf.Lerp(minIntensity, maxIntensity, intensityLerpTime);
            lightComponent.intensity = currentIntensity;
        }
        
        // 范围PingPong效果
        if (enableRangePingPong)
        {
            rangeLerpTime = Mathf.PingPong(Time.time * rangeChangeSpeed, 1.0f);
            float currentRange = Mathf.Lerp(minRange, maxRange, rangeLerpTime);
            lightComponent.range = currentRange;
        }
    }
    
    private IEnumerator ShowPromptAfterDelay()
    {
        yield return new WaitForSeconds(1f); 
        // 创建世界空间UI
        if (promptUI != null)
        {
            // 实例化提示UI
            currentPrompt = promptUI;
            currentPrompt.SetActive(true);
        }
        else
        {
            Debug.LogWarning("未设置提示UI预制体！");
        }
    }
    
    private IEnumerator UpdatePromptRotation()
    {
        while (currentPrompt != null)
        {
            if (Camera.main != null)
            {
                currentPrompt.transform.rotation = Camera.main.transform.rotation;
            }
            yield return null;
        }
    }
    
    // 公共方法用于动态改变颜色
    public void SetColors(Color newStartColor, Color newEndColor)
    {
        startColor = newStartColor;
        endColor = newEndColor;
    }
    
    // 公共方法用于改变强度范围
    public void SetIntensityRange(float min, float max)
    {
        minIntensity = min;
        maxIntensity = max;
    }
    
    // 公共方法用于改变光照范围
    public void SetRange(float min, float max)
    {
        minRange = min;
        maxRange = max;
    }
    
    // 公共方法用于改变各种变化速度
    public void SetChangeSpeed(float colorSpeed, float intensitySpeed, float rangeSpeed)
    {
        colorChangeSpeed = colorSpeed;
        intensityChangeSpeed = intensitySpeed;
        rangeChangeSpeed = rangeSpeed;
    }
    
    // 启用/禁用强度循环
    public void ToggleIntensityPingPong(bool enable)
    {
        enableIntensityPingPong = enable;
    }
    
    // 启用/禁用范围循环
    public void ToggleRangePingPong(bool enable)
    {
        enableRangePingPong = enable;
    }
    
    // 启用父物体组件并监听动画完成
    private void EnableParentComponents()
    {
        if (rotationController != null)
        {
            rotationController.enabled = true;
        }
        
        if (animator != null)
        {
            animator.enabled = true;
            StartCoroutine(WaitForAnimationComplete());
        }
    }
    
    // 监听动画完成的协程
    private IEnumerator WaitForAnimationComplete()
    {
        if (animator != null)
        {
            // 等待动画播放完成
            yield return new WaitForSeconds(0.1f); // 短暂延迟确保动画开始
            while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
            {
                yield return null;
            }
            
            // 动画播放完成后调用下一个方法
            OnAnimationComplete();
        }
    }
    // 动画完成后的回调方法
    private void OnAnimationComplete()
    {
        Debug.Log("动画播放完成，准备执行下一步操作");
        // 获取并调用AnimationEventHandler
        AnimationEventHandler eventHandler = transform.parent.GetComponent<AnimationEventHandler>();
        if (eventHandler != null)
        {
            eventHandler.OnAnimationSequenceComplete();
        }
        else
        {
            Debug.LogWarning("未找到AnimationEventHandler组件！");
        }
    }
} 