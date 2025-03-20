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
    public TextMeshProUGUI Source;
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
    public void InitializedWindow()
    {
        this.transform.Find("名字").GetComponent<TextMeshProUGUI>().text = $"名字:";
        this.transform.Find("重量").GetChild(0).GetComponent<TMP_InputField>().text = "";
        SliderContorler.instance.OnItemSet(null);
    }

    public void submitItem()
    {
        float TempHeight=float.Parse(this.transform.Find("重量").GetChild(0).GetComponent<TMP_InputField>().text);
        float TempSource = string.IsNullOrEmpty(Source.text) ? 0f : float.Parse(Source.text);
        Source.text = (TempSource + 20 - Math.Abs(message.message.ItemHeight - TempHeight)).ToString();//每次20分数与所称误差相减
        InitializedWindow();

    }




}
