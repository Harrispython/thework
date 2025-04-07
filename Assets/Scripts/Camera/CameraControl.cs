using Cinemachine;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening; // ��Ҫ���� Linq


public class CameraControl : MonoBehaviour
{
    public List<CinemachineVirtualCamera> VirtualCameraList;

    public void ActivateCamera(string cameraName)
    {
        var targetCamera = VirtualCameraList.Find(cam => cam.name == cameraName);

        if (targetCamera != null)
        {
            SetActiveCamera(targetCamera);
        }
        else
        {
            Debug.LogWarning($"Camera with name {cameraName} not found in the list.");
        }
    }

    private void SetActiveCamera(CinemachineVirtualCamera activeCam)
    {
        VirtualCameraList.ForEach(cam => cam.Priority = (cam == activeCam) ? 25 : 0);
    }

    public void SetDsecriptionCamera()
    {
        var muralCamera = VirtualCameraList.Find(cam => cam.name == "壁画相机");
        if (muralCamera == null)
        {
            Debug.LogWarning("MuralCamera not found.");
            return;
        }

        Transform camTransform = muralCamera.transform;

        // 创建顺序动画
        Sequence seq = DOTween.Sequence();
        seq.Append(camTransform.DOMoveZ(camTransform.position.z + 9, 1.8f).SetEase(Ease.InOutQuad)) // 先慢后快再慢
            .AppendInterval(1f) // 停留 1 秒
            .Append(camTransform.DOMoveZ(camTransform.position.z, 1.8f).SetEase(Ease.InOutQuad))     // 先慢后快再慢
            .AppendInterval(0.5f) // 停留 0.5 秒
            .Append(camTransform.DOMoveZ(camTransform.position.z - 16, 1.8f).SetEase(Ease.InOutQuad)) // 先慢后快再慢
            .AppendInterval(1f) // 停留 1 秒
            .Append(camTransform.DOMoveZ(camTransform.position.z, 1.8f).SetEase(Ease.InOutQuad))     // 先慢后快再慢
            .Play();
    }
    
}
