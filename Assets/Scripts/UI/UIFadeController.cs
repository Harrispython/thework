using UnityEngine;
using UnityEngine.UI; // 引入UI命名空间

public class UIFadeController : MonoBehaviour
{
    [Header("渐变设置")]
    [Tooltip("完成渐变效果所需的时间（秒）")]
    public float fadeDuration = 1.0f;

    [Tooltip("初始是否显示")]
    public bool startVisible = false;

    [Header("按键设置")]
    [Tooltip("触发渐隐的按键")]
    public KeyCode fadeKey = KeyCode.E;

    private CanvasGroup canvasGroup; // 使用CanvasGroup来控制整体透明度
    private bool isVisible = false;
    private bool isAnimating = false;
    private float currentAlpha = 1f;
    private float targetAlpha = 1f;
    private float currentTime = 0f;
    
    [Header("是否重复")]
    public bool repeat=false;//判断是否需要重复
    private bool isrepeat=false;//判断是否需要重复

    void Start()
    {
        // 获取或添加CanvasGroup组件
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        // 设置初始状态
        isVisible = startVisible;
        currentAlpha = startVisible ? 1f : 0f;
        targetAlpha = currentAlpha;
        canvasGroup.alpha = currentAlpha;
    }

    void Update()
    {
        // 检测按键输入
        if (Input.GetKeyDown(fadeKey))
        {
            if (!isrepeat)
            {
                ToggleVisibility();
            }
        }

        // 处理渐变动画
        if (isAnimating)
        {
            currentTime += Time.deltaTime;
            float progress = currentTime / fadeDuration;

            if (progress >= 1f)
            {
                // 动画完成
                progress = 1f;
                isAnimating = false;
                currentTime = 0f;
                canvasGroup.alpha = targetAlpha;
            }
            else
            {
                // 计算当前透明度
                canvasGroup.alpha = Mathf.Lerp(currentAlpha, targetAlpha, progress);
            }
        }
    }

    // 切换可见性
    public void ToggleVisibility()
    {
        if (!repeat)
        {
            FadeIn();
            isrepeat = true;
            return;
        } 
        if (isVisible)
        {
            FadeOut();
        }
        else
        {
            FadeIn();
        }
    }

    // 渐显
    public void FadeIn()
    {
        if (!isVisible)
        {
            isVisible = true;
            targetAlpha = 1f;
            currentAlpha = canvasGroup.alpha;
            currentTime = 0f;
            isAnimating = true;
        }
    }

    // 渐隐
    public void FadeOut()
    {
        if (isVisible)
        {
            isVisible = false;
            targetAlpha = 0f;
            currentAlpha = canvasGroup.alpha;
            currentTime = 0f;
            isAnimating = true;
        }
    }

    // 立即设置可见性（无动画）
    public void SetVisibilityImmediate(bool visible)
    {
        isVisible = visible;
        currentAlpha = visible ? 1f : 0f;
        targetAlpha = currentAlpha;
        canvasGroup.alpha = currentAlpha;
        isAnimating = false;
        currentTime = 0f;
    }
} 