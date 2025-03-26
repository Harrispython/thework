using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveUp : MonoBehaviour
{
    [Header("移动参数")]
    [Tooltip("上移速度（米/秒）")]
    public float moveSpeed = 0.2f;     // 上移速度
    
    [Tooltip("上移距离（米）")]
    public float moveDistance = 3f;   // 上移距离

    public GameObject target;
    private bool isup = false;
    
    // 调用此方法使物体向上移动
    public void StartMoveUp()
    {
        // 使用协程实现平滑移动
        StartCoroutine(MoveUpCoroutine());
    }
    
    private void Update()
    {
        if (isup)
        {
            float movedDistance = 0;

            // 当移动距离小于目标距离时继续移动
            while (movedDistance < moveDistance)
            {
                // 计算本帧移动距离
                float delta = moveSpeed * Time.deltaTime;
            
                // 移动物体
                target.transform.Translate(Vector3.up * delta);
            
                // 更新已移动距离
                movedDistance += delta;
            
            }
        }
    }

    private System.Collections.IEnumerator MoveUpCoroutine()
    {
        yield return new WaitForSeconds(1f);
        isup = true;
    }
}
