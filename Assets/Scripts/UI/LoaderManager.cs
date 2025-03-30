using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoaderManager : MonoBehaviour
{
    public static LoaderManager Instance { get; private set; }
    private static string targetScene; // Ŀ�곡����
    private AsyncOperation asyncLoad; // �洢�첽���ز���
    public bool IsLoadComplete { get; private set; } = false; // ����Ƿ�������

    private void Awake()
    {
        // ȷ�� LoaderManager ֻ��һ��ʵ���������ڳ����л�ʱ��������
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
    /// ���ô˷�����ת��Loading��������׼���첽����Ŀ�곡��
    /// </summary>
    public static void LoadScene(string sceneName)
    {
        targetScene = sceneName;
        SceneManager.LoadScene("Loading"); // ����ת��Loading����
    }

    /// <summary>
    /// ��Loading�����е��ã���ʼ�첽����Ŀ�곡����������������
    /// </summary>
    public void StartLoading()
    {
        StartCoroutine(LoadTargetScene());
    }

    private IEnumerator LoadTargetScene()
    {
        asyncLoad = SceneManager.LoadSceneAsync(targetScene);
        asyncLoad.allowSceneActivation = false; // �Ȳ������

        while (!asyncLoad.isDone)
        {
            // ������Ը���UI�����������磺
            // UIManager.Instance.SetLoadingProgress(asyncLoad.progress);

            if (asyncLoad.progress >= 0.9f)
            {
                IsLoadComplete = true; // ��Ǽ������
                break; // �˳�ѭ�����ȴ���ť���
            }
            yield return null;
        }
    }

    /// <summary>
    /// ����ҵ����������Ϸ����ťʱ��������صĳ���
    /// </summary>
    public void ActivateLoadedScene()
    {
        if (asyncLoad != null && IsLoadComplete)
        {
            asyncLoad.allowSceneActivation = true;
        }
    }
}
