using UnityEngine;
using Cinemachine;

public class FirstPersonCamera : MonoBehaviour
{
    [Header("相机设置")]
    [Tooltip("第一人称虚拟相机")]
    public CinemachineVirtualCamera firstPersonCamera;

    [Header("旋转设置")]
    [Tooltip("水平旋转速度")]
    public float horizontalSpeed = 2.0f;

    private CameraViewSwitch viewSwitch;
    private Vector3 fixedPos;
    private void Start()
    {
        fixedPos = firstPersonCamera.transform.position;//固定相机位置
        viewSwitch = GetComponent<CameraViewSwitch>();
        if (viewSwitch == null)
        {
            Debug.LogError("未找到CameraViewSwitch组件！");
        }

        // 锁定并隐藏鼠标
        Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
    }

    private void Update()
    {
        firstPersonCamera.transform.position = fixedPos;
        if (viewSwitch != null && viewSwitch.IsInFirstPerson())
        {
            // 获取鼠标水平输入
            float mouseX = Input.GetAxis("Mouse X") * horizontalSpeed;

            // 只绕世界Y轴旋转
            firstPersonCamera.transform.Rotate(Vector3.up * mouseX, Space.World);
        }
    }

    private void OnDisable()
    {
        // 解锁鼠标
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
} 