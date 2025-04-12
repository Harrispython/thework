using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class FadeCanvasGroup : MonoBehaviour  
{
    public float fadeDuration = 1f;   //渐变时间
    
    private CanvasGroup canvasGroup;
    [Header("渐入或者渐出")]
    public bool IsValue;
    
    [Header("淡入淡出完成时事件")]
    public UnityEvent onFadeComplete;  // 添加事件，在淡入或淡出完成时触发
    
    // 是否自动开始淡入淡出
    [Header("是否自动开始淡入淡出")]
    public bool autoStart = true;
    
    // 目标画布组（淡入淡出完成后要触发的画布组）
    [Header("淡入淡出完成后触发的画布组")]
    public FadeCanvasGroup nextCanvasGroup;

    void Start()
    {
        canvasGroup=GetComponent<CanvasGroup>();
        if (autoStart)
        {
            StartFade();
        }
    }
    
    // 公共方法，用于从外部启动淡入淡出
    public void StartFade()
    {
        if (IsValue)
        {
            StartCoroutine(FadeIn());
        }
        else
        {
            StartCoroutine(FadeOut());
        }
    }

    IEnumerator FadeIn()
    {
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Clamp01(elapsed / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = 1f;
        
        // 淡入完成后触发事件
        onFadeComplete.Invoke();
        
        // 如果设置了下一个画布组，则触发它的淡入淡出
        if (nextCanvasGroup != null)
        {
            nextCanvasGroup.StartFade();
        }
    }

    IEnumerator FadeOut()
    {
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Clamp01(1f - elapsed / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = 0f;
        
        // 淡出完成后触发事件
        onFadeComplete.Invoke();
        
        // 如果设置了下一个画布组，则触发它的淡入淡出
        if (nextCanvasGroup != null)
        {
            nextCanvasGroup.StartFade();
        }
    }
}
