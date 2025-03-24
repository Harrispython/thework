using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIControler : MonoBehaviour
{
    /// <summary>
    /// ���û����ָ�����Ƶ�������
    /// </summary>

    /// 

    public void SetChildActive(string childName, bool isActive)
    {
        Transform child = transform.Find(childName);
        if (child != null)
        {

            child.gameObject.SetActive(isActive);
            Debug.Log($"{child.gameObject.activeSelf}");
        }
        else
        {
            Debug.LogWarning($"������ '{childName}' δ�ҵ���");
        }
    }

    public void activeObject(string childName)
    {
        Transform child = transform.Find(childName);
        if (child != null)
        {

            child.gameObject.SetActive(true);
            Debug.Log($"{child.gameObject.activeSelf}");
            return;
        }
        else
        {
            Debug.LogWarning($"������ '{childName}' δ�ҵ���");
        }
        if (this.gameObject.name == childName)
        {
            this.gameObject.SetActive(true);
        }

    }

    public void DisactiveObject(string childName)
    {
        Transform child = transform.Find(childName);
        if (child != null)
        {

            child.gameObject.SetActive(false);
            Debug.Log($"{child.gameObject.activeSelf}");
        }
        else
        {
            Debug.LogWarning($"������ '{childName}' δ�ҵ���");
        }
        if (this.gameObject.name == childName)
        {
            this.gameObject.SetActive(false);
            UIManager.Instance.IsUIVisible= false;
        }
    }
}

