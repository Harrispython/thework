using UnityEngine;
using UnityEngine.UI;

public class LoadingManager : MonoBehaviour
{
    public Button enterGameButton; // ��������Ϸ����ť


    void Start()
    {
        enterGameButton.gameObject.SetActive(false); // ��ʼ���ذ�ť
        LoaderManager.Instance.StartLoading();
    }

    void Update()
    {
        if (LoaderManager.Instance.IsLoadComplete)
        {
            enterGameButton.gameObject.SetActive(true); // ������ɺ���ʾ��ť
        }

        // ���½�����
        //progressBar.fillAmount = Mathf.Clamp01(LoaderManager.Instance.IsLoadComplete ? 1f : 0.9f);
    }

    public void OnEnterGameButtonClick()
    {
        LoaderManager.Instance.ActivateLoadedScene(); // ����Ŀ�곡��
    }
}
