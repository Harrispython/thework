using UnityEngine;

public class UIManager : MonoBehaviour
{
    // 单例实例
    private static UIManager instance;
    GameObject canvasPrefab;

    // 全局访问点
    public static UIManager Instance
    {
        get
        {
            if (instance == null)
            {
                // 在场景中查找UIManager实例
                instance = FindObjectOfType<UIManager>();

                // 如果场景中没有，创建一个新的
                if (instance == null)
                {
                    GameObject go = new GameObject("UIManager");
                    instance = go.AddComponent<UIManager>();
                }
            }
            return instance;
        }
    }

    // UI显示状态
    private bool isUIVisible = false;

    // 公共属性，用于获取和设置UI显示状态
    public bool IsUIVisible
    {
        get { return isUIVisible; }
        set
        {
            isUIVisible = value;
            Cursor.lockState = isUIVisible ? CursorLockMode.None : CursorLockMode.Locked;
            // 这里可以添加UI显示状态改变时的其他逻辑
            Debug.Log($"UI显示状态已更改为: {value}");
        }
    }

    private void Awake()
    {
        // 确保只有一个实例
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // 负责根据名字搜索并实例化Canvas预制体
    public void SpawnCanvas(string prefabName)
    {
        // 检查场景中是否已存在同名的 Canvas
        Canvas[] existingCanvases = FindObjectsOfType<Canvas>();
        foreach (Canvas canvas in existingCanvases)
        {
            if (canvas.gameObject.name == prefabName)
            {
                Debug.Log($"场景中已存在名为 '{prefabName}' 的 Canvas，不再创建新实例。");
                return;
            }
        }
        existingCanvases = null;
        canvasPrefab = Resources.Load<GameObject>("Prefab/" + prefabName);
        if (canvasPrefab == null)
        {
            Debug.LogError($"Canvas 预制体 '{prefabName}' 未找到！");
            return;
        }

        Instantiate(canvasPrefab).name = prefabName;
        Debug.Log($"Canvas 预制体 '{prefabName}' 已成功生成。");
    }
    public void ActivateCavansObject(string ObjecetName)
    {
        UIControler TempUI= canvasPrefab.GetComponent<UIControler>();
        if (TempUI)
        {

            TempUI.SetChildActive(ObjecetName,true);
        }
    }
    public void DisactiveCavansObject(string ObjecetName)
    {

        UIControler TempUI = canvasPrefab.GetComponent<UIControler>();
        if (TempUI)
        {
            TempUI.SetChildActive(ObjecetName, false);
        }
    }



}
