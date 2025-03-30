using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoaderManager : MonoBehaviour
{
    public static LoaderManager Instance { get; private set; }
    private static string targetScene; // 目标场景名
    private AsyncOperation asyncLoad; // 存储异步加载操作
    public bool IsLoadComplete { get; private set; } = false; // 标记是否加载完成

    private void Awake()
    {
        // 确保 LoaderManager 只有一个实例，并且在场景切换时不被销毁
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 调用此方法跳转到Loading场景，并准备异步加载目标场景
    /// </summary>
    public static void LoadScene(string sceneName)
    {
        targetScene = sceneName;
        SceneManager.LoadScene("Loading"); // 先跳转到Loading界面
    }

    /// <summary>
    /// 在Loading场景中调用，开始异步加载目标场景，但不立即激活
    /// </summary>
    public void StartLoading()
    {
        StartCoroutine(LoadTargetScene());
    }

    private IEnumerator LoadTargetScene()
    {
        asyncLoad = SceneManager.LoadSceneAsync(targetScene);
        asyncLoad.allowSceneActivation = false; // 先不激活场景

        while (!asyncLoad.isDone)
        {
            // 这里可以更新UI进度条，比如：
            // UIManager.Instance.SetLoadingProgress(asyncLoad.progress);

            if (asyncLoad.progress >= 0.9f)
            {
                IsLoadComplete = true; // 标记加载完成
                break; // 退出循环，等待按钮点击
            }
            yield return null;
        }
    }

    /// <summary>
    /// 当玩家点击“进入游戏”按钮时，激活加载的场景
    /// </summary>
    public void ActivateLoadedScene()
    {
        if (asyncLoad != null && IsLoadComplete)
        {
            asyncLoad.allowSceneActivation = true;
        }
    }
}
