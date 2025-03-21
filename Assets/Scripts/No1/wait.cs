using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wait : MonoBehaviour
{
    // 启动协程
    void Start()
    {
        StartCoroutine(waitSecond());
    }

    // 定义协程
    private System.Collections.IEnumerator waitSecond()
    {
        // 等待十秒
        yield return new WaitForSeconds(13.5f);
        
        // 调用目标方法
        gameObject.GetComponent<LightColorController>().enabled = true;
    }
}
