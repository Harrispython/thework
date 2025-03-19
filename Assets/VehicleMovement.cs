using UnityEngine;

public class VehicleMovement : MonoBehaviour
{
    [Header("移动设置")]
    [Tooltip("移动速度")]
    public float moveSpeed = 5f;
    [Tooltip("转向速度")]
    public float rotationSpeed = 100f;
    
    [Header("轮子设置")]
    [Tooltip("轮子对象数组")]
    public Transform[] wheels;
    [Tooltip("轮子旋转速度")]
    public float wheelRotationSpeed = 200f;

    private void Update()
    {
        // 获取输入
        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");
        
        // // 移动物体
        // Vector3 movement = transform.forward * verticalInput * moveSpeed * Time.deltaTime;
        // transform.position += movement;
        
        // // 转向
        // float rotation = horizontalInput * rotationSpeed * Time.deltaTime;
        // transform.Rotate(Vector3.up * rotation);
        
        // 旋转轮子
        if (wheels != null && wheels.Length > 0)
        {
            foreach (Transform wheel in wheels)
            {
                // 根据移动方向旋转轮子
                wheel.Rotate(Vector3.right * verticalInput * wheelRotationSpeed * Time.deltaTime);
            }
        }
    }
} 