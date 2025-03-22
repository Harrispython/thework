using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIControler : MonoBehaviour
{
    /// <summary>
    /// 启用或禁用指定名称的子物体
    /// </summary>
    /// <param name="childName">子物体名称</param>
    /// <param name="isActive">是否启用</param>
    public void SetChildActive(string childName, bool isActive)
    {
        Transform child = transform.Find(childName);
        if (child != null)
        {

            child.gameObject.SetActive(isActive);
            Debug.Log($"{gameObject.activeSelf}");
        }
        else
        {
            Debug.LogWarning($"子物体 '{childName}' 未找到！");
        }
    }
}

