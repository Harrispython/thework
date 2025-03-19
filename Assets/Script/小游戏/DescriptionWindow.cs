using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEditorInternal.Profiling.Memory.Experimental;

public class DescriptionWindow : MonoBehaviour
{

    public Action<ItemMessage> OnWindowSet; // 事件委托
    public static DescriptionWindow instance;
    public ItemMessage message;

    private void Start()
    {
        // 订阅事件
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
        this.transform.Find("名字").GetComponent<TextMeshProUGUI>().text = $"名字:{Message.message.ItemName}";
        this.transform.Find("重量").GetChild(0).GetComponent<TMP_InputField>().text = "";
        
    }


}
