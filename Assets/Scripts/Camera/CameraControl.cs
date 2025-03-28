using Cinemachine;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; // 需要引入 Linq

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
        VirtualCameraList.ForEach(cam => cam.Priority = (cam == activeCam) ? 10 : 0);
    }
}
