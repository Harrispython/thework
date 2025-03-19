using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEditorInternal.Profiling.Memory.Experimental;

public class DescriptionWindow : MonoBehaviour
{

    public Action<ItemMessage> OnWindowSet; // �¼�ί��
    public static DescriptionWindow instance;
    public ItemMessage message;

    private void Start()
    {
        // �����¼�
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        OnWindowSet += SetWindow;
    }
    public void SetWindow(ItemMessage Message)
    {
        message = Message;
        this.transform.Find("����").GetComponent<TextMeshProUGUI>().text = $"����:{Message.message.ItemName}";
        this.transform.Find("����").GetChild(0).GetComponent<TMP_InputField>().text = "";
        
    }


}
