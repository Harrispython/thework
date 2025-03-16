using UnityEngine;

public class ZAxisController : MonoBehaviour
{
    [Header("移动设置")]
    [Tooltip("Z轴移动速度")]
    public float moveSpeed = -15.0f;

    [Tooltip("是否启用移动")]
    private bool isMoving = false;

    void Update()
    {
        // 按下F键切换移动状态
        if (Input.GetKeyDown(KeyCode.E))
        {
            isMoving = !isMoving;
            Debug.Log($"Z轴移动状态: {(isMoving ? "开启" : "关闭")}");
        }

        // 如果移动状态为开启，则在Z轴方向上移动
        if (isMoving)
        {
            // 使用Transform组件在Z轴方向上移动
            transform.Translate(0, 0, moveSpeed * Time.deltaTime);
        }
    }
} 