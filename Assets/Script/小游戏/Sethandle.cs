using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sethandle : MonoBehaviour
{
    //public Slider slider;  // 拖动条
    public RectTransform handle; // Handle 的 RectTransform
    public Transform Point;
    private Quaternion initialRotation;



    void Start()
    {
        // 记录初始旋转角度
        initialRotation = handle.rotation;
    }

    void Update()
    {
        // 保持 handle 旋转不变
        handle.rotation = initialRotation;
        if(Point)
        this.transform.position = Point.position;
    }


}
