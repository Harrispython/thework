using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sethandle : MonoBehaviour
{
    //public Slider slider;  // �϶���
    public RectTransform handle; // Handle �� RectTransform
    public Transform Point;
    private Quaternion initialRotation;



    void Start()
    {
        // ��¼��ʼ��ת�Ƕ�
        initialRotation = handle.rotation;
    }

    void Update()
    {
        // ���� handle ��ת����
        handle.rotation = initialRotation;
        if(Point)
        this.transform.position = Point.position;
    }


}
