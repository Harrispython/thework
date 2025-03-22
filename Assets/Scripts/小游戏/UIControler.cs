using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIControler : MonoBehaviour
{
    /// <summary>
    /// ���û����ָ�����Ƶ�������
    /// </summary>
    /// <param name="childName">����������</param>
    /// <param name="isActive">�Ƿ�����</param>
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
            Debug.LogWarning($"������ '{childName}' δ�ҵ���");
        }
    }
}

