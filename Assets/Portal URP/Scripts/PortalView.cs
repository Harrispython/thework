using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Knife.Portal
{
    // 设置脚本执行顺序为80，确保在其他脚本之后执行
    [DefaultExecutionOrder(80)]
    public class PortalView : MonoBehaviour
    {
        // 当渲染纹理改变时触发的事件
        public event UnityAction OnRenderTextureChanged;

        [SerializeField] private Camera playerCamera;      // 玩家相机引用
        [SerializeField] private Camera portalViewCamera;  // 传送门视角相机引用
        [SerializeField] private PortalViewResolution portalViewResolution = PortalViewResolution.High;  // 传送门视图分辨率设置
        [SerializeField] private Transform portalRoot1;    // 传送门入口的根变换
        [SerializeField] private Transform portalRoot2;    // 传送门出口的根变换
        [SerializeField] private float nearClipPlaneThreshold = 1f;  // 近裁剪面阈值

        // 用于存储渲染纹理的私有变量
        private RenderTexture renderTexture;

        // 渲染纹理的属性访问器
        public RenderTexture RenderTexture
        {
            get { return renderTexture; }
            private set { renderTexture = value; }
        }

        private void Start()
        {
            CreateRT(); // 创建渲染纹理
            
            // 确保相机被激活
            if (!portalViewCamera.gameObject.activeSelf)
            {
                portalViewCamera.gameObject.SetActive(true);
            }
        }

        // 创建渲染纹理
        private void CreateRT()
        {
            // 如果已存在渲染纹理，则销毁它
            if (RenderTexture != null)
                DestroyImmediate(RenderTexture, true);

            int resolution = (int)portalViewResolution;

            // 根据设置创建对应分辨率的渲染纹理
            if (portalViewResolution == PortalViewResolution.ScreenSize)
            {
                RenderTexture = new RenderTexture(Screen.width, Screen.height, 32, RenderTextureFormat.ARGBFloat);
            }
            else
            {
                RenderTexture = new RenderTexture(resolution, resolution, 32, RenderTextureFormat.ARGBFloat);
            }

            // 设置传送门相机的渲染目标和参数
            portalViewCamera.targetTexture = RenderTexture;
            portalViewCamera.enabled = true;
            portalViewCamera.aspect = playerCamera.aspect;
            
            // 触发渲染纹理改变事件
            if (OnRenderTextureChanged != null)
                OnRenderTextureChanged();
        }

        // 开始渲染
        public void StartRendering()
        {
            portalViewCamera.gameObject.SetActive(true);
            portalViewCamera.Render();
        }

        // 停止渲染
        public void StopRendering()
        {
            portalViewCamera.gameObject.SetActive(false);
        }

        // 清理资源
        private void OnDestroy()
        {
            if (this != null)
            {
                if (portalViewCamera != null)
                    portalViewCamera.targetTexture = null;

                if (RenderTexture != null)
                    DestroyImmediate(RenderTexture, true);
            }
        }

        // 添加设置相机位置和旋转的方法
        public void SetupCamera(Vector3 position, Quaternion rotation)
        {
            portalViewCamera.transform.position = position;
            portalViewCamera.transform.rotation = rotation;
        }

        // 传送门视图分辨率的枚举定义
        public enum PortalViewResolution : int
        {
            Low = 256,        // 低分辨率
            Medium = 512,     // 中等分辨率
            High = 1024,      // 高分辨率
            VeryHigh = 2048,  // 超高分辨率
            ScreenSize = -1   // 屏幕分辨率
        }
    }
}