using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lookatCamera : MonoBehaviour
{
    // 摄像机对象
    public Camera ca;

    void Update()
    {
        // 使UI对象的朝向与摄像机一致
        transform.LookAt(transform.position + ca.transform.rotation * -Vector3.forward,
            ca.transform.rotation * Vector3.up);
    }
}
