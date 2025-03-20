using UnityEngine;
using System.Collections;

public class AnimationEventHandler : MonoBehaviour
{
    [Header("【组件引用】")]
    [Tooltip("灯光控制器组件")]
    [SerializeField]
    private LightColorController lightController;
    
    [Header("【材质控制】")]
    [Tooltip("需要更换的目标材质")]
    [SerializeField]
    private Material targetMaterial;
    
    [Tooltip("需要更换材质的渲染器")]
    [SerializeField]
    private Renderer targetRenderer;
    
    [Header("【Dissolve控制】")]
    [Tooltip("Dissolve效果组件")]
    [SerializeField]
    private Dissolve dissolveEffect;
    
    [Header("【事件设置】")]
    [Tooltip("动画完成后等待时间")]
    [SerializeField]
    private float delayAfterAnimation = 1.8f;
    
    [Tooltip("Dissolve效果后等待时间")]
    [SerializeField]
    private float delayAfterDissolve = 2.5f;
    
    private Light lightComponent;
    private LensFlareController lensFlareController;
    
    private void Start()
    {
        // 如果没有指定灯光控制器，尝试在子物体中查找
        if (lightController == null)
        {
            lightController = GetComponentInChildren<LightColorController>();
        }
        
        // 如果没有指定渲染器，尝试获取当前物体的渲染器
        if (targetRenderer == null)
        {
            targetRenderer = GetComponent<Renderer>();
        }
        
        // 如果没有指定Dissolve效果组件，尝试获取
        if (dissolveEffect == null)
        {
            dissolveEffect = GetComponent<Dissolve>();
        }
        
        // 初始时禁用Dissolve效果
        if (dissolveEffect != null)
        {
            dissolveEffect.enabled = false;
        }
        
        if (lightController == null)
        {
            Debug.LogError("未找到LightColorController组件！请确保已正确设置引用。");
            enabled = false;
            return;
        }
        
        // 获取LightController对象上的组件
        lightComponent = lightController.GetComponent<Light>();
        lensFlareController = lightController.GetComponent<LensFlareController>();
        if (lensFlareController != null)
        {
            lensFlareController.enabled = false; // 初始时禁用
        }
    }
    
    // 由LightColorController调用的方法
    public void OnAnimationSequenceComplete()
    {
        StartCoroutine(HandleAnimationComplete());
    }
    
    private IEnumerator HandleAnimationComplete()
    {
        Debug.Log("开始处理动画完成后的事件");
        
        // 等待指定时间
        yield return new WaitForSeconds(delayAfterAnimation);
        
        // 更换材质
        if (targetRenderer != null && targetMaterial != null)
        {
            targetRenderer.material = targetMaterial;
            Debug.Log("已更换为目标材质");
        }
        else
        {
            Debug.LogWarning("未设置目标渲染器或材质！");
        }
        
        // 启用Dissolve效果
        if (dissolveEffect != null)
        {
            dissolveEffect.enabled = true;
            Debug.Log("已启用Dissolve效果");
            
            // 等待Dissolve效果后的处理
            yield return new WaitForSeconds(delayAfterDissolve);
            
            // 启用LensFlare效果并改变灯光颜色
            if (lensFlareController != null)
            {
                lensFlareController.enabled = true;
                Debug.Log("已启用LensFlare效果");
            }
            else
            {
                Debug.LogWarning("未找到LensFlareController组件！");
            }
            
            if (lightComponent != null)
            {
                lightComponent.color = Color.white;
                Debug.Log("已将灯光颜色设置为黑色");
            }
            else
            {
                Debug.LogWarning("未找到Light组件！");
            }
        }
        else
        {
            Debug.LogWarning("未找到Dissolve效果组件！");
        }
        Debug.Log("动画序列处理完成");
    }
} 