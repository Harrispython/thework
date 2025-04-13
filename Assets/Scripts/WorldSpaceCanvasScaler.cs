using UnityEngine;

public class WorldSpaceCanvasScaler : MonoBehaviour
{
    public Camera mainCamera;
    public float canvasDistance = 1f;
    public Vector2 referenceResolution = new Vector2(1920, 1080);

    private RectTransform canvasRect;

    void Start()
    {
        canvasRect = GetComponent<RectTransform>();
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        AdjustCanvas();
    }

    void AdjustCanvas()
    {
        float screenRatio = (float)Screen.width / Screen.height;
        float referenceRatio = referenceResolution.x / referenceResolution.y;

        float scaleFactor = screenRatio / referenceRatio;

        // 保持高宽比适应
        canvasRect.sizeDelta = referenceResolution;

        transform.position = mainCamera.transform.position + mainCamera.transform.forward * canvasDistance;
        transform.rotation = mainCamera.transform.rotation;

        // 缩放以适应分辨率变化
        transform.localScale = Vector3.one * (1f / referenceResolution.y) * canvasDistance * 100f;
    }

    void Update()
    {
        // 可选：持续适配分辨率变化（或者监听事件）
        AdjustCanvas();
    }
}
