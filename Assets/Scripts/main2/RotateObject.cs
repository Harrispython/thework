using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    // 旋转速度（每秒旋转的角度）
    [SerializeField] private float rotationSpeed = 50f;
    
    void Update()
    {
        // 使用Transform.Rotate方法使物体绕z轴旋转
        // Vector3.forward 等同于 new Vector3(0, 0, 1)
        transform.Rotate(Vector3.forward * (rotationSpeed * Time.deltaTime));
    }  
    
}