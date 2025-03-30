using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MoveUp : MonoBehaviour
{
    [Header("移动参数")]
    [Tooltip("上移速度（米/秒）")]
    private float moveSpeed = 0.001f;     // 上移速度
    
    [Tooltip("上移距离（米）")]
    public float moveDistance = 20f;   // 上移距离

    public GameObject target;
    private bool isup = false;
    
    // 调用此方法使物体向上移动
    public void StartMoveUp()
    {
        // 使用协程实现平滑移动
        
        StartCoroutine(MoveUpCoroutine());
    }
    
    // private void Update()
    // {
    //     if (isup)
    //     {
    //         float movedDistance = 0;
    //
    //         // 当移动距离小于目标距离时继续移动
    //         while (movedDistance < moveDistance)
    //         {
    //
    //         
    //         }
    //     }
    // }

    private System.Collections.IEnumerator MoveUpCoroutine()
    {
        yield return new WaitForSeconds(1f);
        target.transform.DOMove(new Vector3(target.transform.position.x, target.transform.position.y+20, target.transform.position.z), 5f);
        isup = true;
    }
}
