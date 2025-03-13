using UnityEngine;

public class GradualAppearance : MonoBehaviour
{
    [Header("效果设置")]
    [Tooltip("完成显现效果所需的时间（秒）")]
    public float duration = 2f;

    [Tooltip("显现的方向")]
    public Vector3 direction = Vector3.right;

    [Tooltip("显现的起始位置偏移")]
    public float startOffset = -0.5f;

    [Tooltip("显现的结束位置偏移")]
    public float endOffset = 0.5f;

    private Material material;
    private float currentTime = 0f;
    private bool isAnimating = false;

    void Start()
    {
        // 获取物体的材质
        material = GetComponent<Renderer>().material;
        
        // 确保材质支持透明
        material.SetFloat("_Mode", 3); // Transparent模式
        material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        material.SetInt("_ZWrite", 0);
        material.DisableKeyword("_ALPHATEST_ON");
        material.EnableKeyword("_ALPHABLEND_ON");
        material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        material.renderQueue = 3000;

        // 初始化shader属性
        material.SetVector("_GradientDirection", direction.normalized);
        material.SetFloat("_StartPosition", startOffset);
        material.SetFloat("_EndPosition", endOffset);
        material.SetFloat("_Progress", 0);

        // 开始动画
        isAnimating = true;
    }

    void Update()
    {
        if (!isAnimating) return;

        // 更新时间和进度
        currentTime += Time.deltaTime;
        float progress = Mathf.Clamp01(currentTime / duration);
        
        // 更新材质的进度属性
        material.SetFloat("_Progress", progress);

        // 检查是否完成
        if (progress >= 1f)
        {
            isAnimating = false;
        }
    }

    // 重置动画
    public void ResetAnimation()
    {
        currentTime = 0f;
        material.SetFloat("_Progress", 0);
        isAnimating = true;
    }
} 