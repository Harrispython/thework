using UnityEngine;

public class PlatformSelector : MonoBehaviour
{
    [Header("平台引用")]
    [Tooltip("前方平台")]
    public Transform frontPlatform;
    [Tooltip("后方平台")]
    public Transform backPlatform;

    // 当前选中的平台
    private Transform selectedPlatform;

    // 提供公共方法获取当前选中的平台
    public Transform GetSelectedPlatform()
    {
        return selectedPlatform;
    }

    private void Update()
    {
        // 按1选择前方平台
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            selectedPlatform = frontPlatform;
            Debug.Log("已选择前方平台");
        }
        // 按2选择后方平台
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            selectedPlatform = backPlatform;
            Debug.Log("已选择后方平台");
        }
    }
} 