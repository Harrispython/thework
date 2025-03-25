using UnityEngine;
using Cinemachine;

public class CameraViewSwitch : MonoBehaviour
{
    [Header("相机设置")]
    [Tooltip("第三人称虚拟相机")]
    public CinemachineVirtualCamera thirdPersonCamera;
    
    [Tooltip("第一人称虚拟相机")]
    public CinemachineVirtualCamera firstPersonCamera;
    
    [Header("角色设置")]
    [Tooltip("角色模型")]
    public GameObject characterModel;
    
    // 当前是否在第一人称视角
    private bool isFirstPerson = false;
    
    // 相机优先级
    private const int HIGH_PRIORITY = 20;
    private const int LOW_PRIORITY = 10;
    
    private void Start()
    {
        // 初始化相机优先级
        thirdPersonCamera.Priority = HIGH_PRIORITY;
        firstPersonCamera.Priority = LOW_PRIORITY;
    }
    
    private void Update()
    {
        // 检测E键按下
        if (Input.GetKeyDown(KeyCode.E)&&GetComponent<Npc>().canChart)
        {
            SwitchView();
        }
    }
    
    private System.Collections.IEnumerator waitSecond()
    {
        yield return new WaitForSeconds(1f);
        isFirstPerson = !isFirstPerson;
        GetComponent<gameobjectenable>().enableStart();

    }
    private void SwitchView()
    {
        
        if (!isFirstPerson)
        {
            // 切换到第一人称
            thirdPersonCamera.Priority = LOW_PRIORITY;
            firstPersonCamera.Priority = HIGH_PRIORITY;
            
            
            
            // 禁用角色模型
            if (characterModel != null)
            {
                characterModel.SetActive(false);
            }
            StartCoroutine(waitSecond());

        }
        else
        {
            // 切换到第三人称
            firstPersonCamera.Priority = LOW_PRIORITY;
            thirdPersonCamera.Priority = HIGH_PRIORITY;
            
            // 启用角色模型
            if (characterModel != null)
            {
                characterModel.SetActive(true);
            }
            StartCoroutine(waitSecond());

        }
    }

    // 添加公共方法来检查是否在第一人称模式
    public bool IsInFirstPerson()
    {
        return isFirstPerson;
    }
} 