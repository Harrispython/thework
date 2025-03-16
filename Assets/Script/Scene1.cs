using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene1 : MonoBehaviour
{
    // 保存异步加载操作的引用
    private AsyncOperation asyncLoad;
    
    // 是否场景已经加载完成
    private bool isSceneLoaded = false;

    // 渐变用的UI面板
    [SerializeField]
    private CanvasGroup fadePanel;

    // 渐变时间
    [SerializeField]
    private float fadeDuration = 0.5f;

    private void Awake()
    {
        // 如果没有指定fadePanel，尝试在场景中查找
        if (fadePanel == null)
        {
            fadePanel = FindObjectOfType<CanvasGroup>();
        }

        // 如果仍然没有找到，创建一个
        if (fadePanel == null)
        {
            CreateFadePanel();
        }

        // 确保渐变面板初始时是透明的
        fadePanel.alpha = 0;
        fadePanel.blocksRaycasts = false;
    }

    // 创建渐变面板
    private void CreateFadePanel()
    {
        GameObject panelObj = new GameObject("FadePanel");
        Canvas canvas = panelObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 9999; // 确保在最上层

        fadePanel = panelObj.AddComponent<CanvasGroup>();
        
        // 添加黑色背景图片
        UnityEngine.UI.Image image = panelObj.AddComponent<UnityEngine.UI.Image>();
        image.color = Color.black;
        
        // 设置面板大小铺满屏幕
        RectTransform rect = panelObj.GetComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.sizeDelta = Vector2.zero;
        
        // 确保不会被销毁
        DontDestroyOnLoad(panelObj);
    }

    // 预加载场景的方法
    public void PreloadScene(int sceneIndex)
    {
        StartCoroutine(PreloadSceneAsync(sceneIndex));
    }

    // 预加载场景的协程
    private IEnumerator PreloadSceneAsync(int sceneIndex)
    {
        // 开始异步加载场景
        asyncLoad = SceneManager.LoadSceneAsync(sceneIndex);
        asyncLoad.allowSceneActivation = false;
        
        // 等待加载到90%
        while (asyncLoad.progress < 0.9f)
        {
            yield return new WaitForEndOfFrame();
        }

        isSceneLoaded = true;
        Debug.Log("场景预加载完成，等待切换信号");
    }

    // Timeline信号调用的切换方法
    public void ActivateScene()
    {
        if (isSceneLoaded && asyncLoad != null)
        {
            StartCoroutine(SmoothSceneTransition());
        }
        else
        {
            Debug.LogWarning("场景尚未加载完成，无法切换！");
        }
    }

    // 平滑的场景切换协程
    private IEnumerator SmoothSceneTransition()
    {
        // 渐变到黑色
        yield return StartCoroutine(FadeToBlack());

        // 激活新场景
        asyncLoad.allowSceneActivation = true;

        // 等待场景加载完成
        while (!asyncLoad.isDone)
        {
            yield return new WaitForEndOfFrame();
        }

        // 给新场景一帧时间初始化
        yield return new WaitForEndOfFrame();

        // 渐变回透明
        yield return StartCoroutine(FadeFromBlack());

        // 重置状态
        isSceneLoaded = false;
        asyncLoad = null;
    }

    // 渐变到黑色
    private IEnumerator FadeToBlack()
    {
        fadePanel.blocksRaycasts = true;
        float elapsedTime = 0;
        
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            fadePanel.alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
            yield return null;
        }
    }

    // 渐变到透明
    private IEnumerator FadeFromBlack()
    {
        float elapsedTime = 0;
        
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            fadePanel.alpha = 1 - Mathf.Clamp01(elapsedTime / fadeDuration);
            yield return null;
        }
        
        fadePanel.blocksRaycasts = false;
    }

    void Start()
    {
        // 示例：在开始时预加载场景1
        PreloadScene(1);
    }

    void Update()
    {
        
    }
}
