using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VLB;

public class lamplight : MonoBehaviour
{
    [Header("目标设置")]
    [Tooltip("目标发光范围")]
    [SerializeField] [Range(0, 200f)]
    private float enableRange = 123f;

    [Tooltip("目标发光强度")]
    [SerializeField] [Range(0, 10f)]
    private float Intensity = 2.75f;

    [Header("变化时长")]
    [Tooltip("从0变化到目标值所需的时间（秒）")]
    [SerializeField] private float duration = 3f;

    private float elapsedTime = 0f;

    private VolumetricLightBeamSD beam;
    private VolumetricDustParticles particles;

    void Start()
    {
        beam = GetComponent<VolumetricLightBeamSD>();
        particles= GetComponent<VolumetricDustParticles>();
        // 初始值设为0
        beam.spotAngle = 0f;
        beam.intensityGlobal = 0f;
        beam.tiltFactor.y = 0;
        
        particles.alpha = 0f;
        elapsedTime = 0f;
    }

    void Update()
    {
        if (beam == null || duration <= 0f) return;

        elapsedTime += Time.deltaTime;
        float t = Mathf.Clamp01(elapsedTime / duration); // 插值因子从 0 到 1

        // 平滑插值（可以换成 SmoothStep 或其他曲线）
        beam.spotAngle = Mathf.SmoothStep(0f, enableRange, t);
        beam.intensityGlobal = Mathf.SmoothStep(0f, Intensity, t);
        particles.alpha = Mathf.SmoothStep(0f, 1f, t);
        beam.tiltFactor.y = Mathf.SmoothStep(180f, 0, t);
    }
}