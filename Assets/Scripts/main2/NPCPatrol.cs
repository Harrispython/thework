using UnityEngine;

public class NPCPatrol : MonoBehaviour
{
    [Header("巡逻设置")]
    [Tooltip("NPC移动速度")]
    public float moveSpeed = 5f;
    
    [Tooltip("转身速度（度/秒）")]
    public float rotateSpeed = 180f;

    [Tooltip("单个方向行走时间（秒）")]
    public float walkDuration = 10f;

    // 动画控制器组件
    private Animator animator;
    // 是否正在转身
    private bool isRotating = false;
    // 目标旋转角度
    private Quaternion targetRotation;
    // 计时器
    private float walkTimer = 0f;

    private void Start()
    {
        // 获取动画控制器组件
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (isRotating)
        {
            // 执行转身
            RotateToTarget();
        }
        else
        {
            // 前进并计时
            Walk();
        }
    }

    private void Walk()
    {
        // 向前移动
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        
        // 播放行走动画
        if (animator != null)
        {
            animator.SetBool("IsWalking", true);
        }

        // 计时
        walkTimer += Time.deltaTime;
        
        // 到达指定时间后转身
        if (walkTimer >= walkDuration)
        {
            walkTimer = 0f;
            StartRotation();
        }
    }

    private void StartRotation()
    {
        isRotating = true;
        // 计算目标旋转（180度）
        targetRotation = transform.rotation * Quaternion.Euler(0, 180, 0);
        
        // 播放转身动画
        if (animator != null)
        {
            animator.SetBool("IsWalking", false);
            animator.SetTrigger("Turn");
        }
    }

    private void RotateToTarget()
    {
        // 执行旋转
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
        
        // 检查是否完成旋转
        if (Quaternion.Angle(transform.rotation, targetRotation) < 0.1f)
        {
            isRotating = false;
            // 确保完全对齐
            transform.rotation = targetRotation;
        }
    }

    // 在Unity编辑器中绘制巡逻路线
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 10f);
    }
} 