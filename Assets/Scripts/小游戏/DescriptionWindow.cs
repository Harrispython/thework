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
    public TextMeshProUGUI Source;
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
    public void InitializedWindow()
    {
        this.transform.Find("����").GetComponent<TextMeshProUGUI>().text = $"����:";
        this.transform.Find("����").GetChild(0).GetComponent<TMP_InputField>().text = "";
        SliderContorler.instance.OnItemSet(null);
    }

    public void submitItem()
    {
        float TempHeight=float.Parse(this.transform.Find("����").GetChild(0).GetComponent<TMP_InputField>().text);
        float TempSource = string.IsNullOrEmpty(Source.text) ? 0f : float.Parse(Source.text);
        Source.text = (TempSource + 20 - Math.Abs(message.message.ItemHeight - TempHeight)).ToString();//ÿ��20����������������
        InitializedWindow();

    }




}
