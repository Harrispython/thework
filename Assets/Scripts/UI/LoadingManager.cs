using UnityEngine;
using UnityEngine.UI;

public class LoadingManager : MonoBehaviour
{
    public Button enterGameButton; // “进入游戏”按钮


    void Start()
    {
        enterGameButton.gameObject.SetActive(false); // 初始隐藏按钮
        LoaderManager.Instance.StartLoading();
    }

    void Update()
    {
        if (LoaderManager.Instance.IsLoadComplete)
        {
            enterGameButton.gameObject.SetActive(true); // 加载完成后显示按钮
        }

        // 更新进度条
        //progressBar.fillAmount = Mathf.Clamp01(LoaderManager.Instance.IsLoadComplete ? 1f : 0.9f);
    }

    public void OnEnterGameButtonClick()
    {
        LoaderManager.Instance.ActivateLoadedScene(); // 激活目标场景
    }
}
