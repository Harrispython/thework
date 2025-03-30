using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System;

public class SliderContorler : MonoBehaviour
{
    public static SliderContorler instance;
    public Slider slider;
    public float itemHeight; // ��������
    public string itemName; // ��������
    public float itemLenght; // ��������
    public Image itemImage;
    public float Coefficient; // ��бϵ��
    public float Differencevalue; // ��ֵ
    public float minHeight;

    public TextMeshProUGUI DisplayScale; // ��ʾ�̶�

    public Action<ItemMessage> OnItemSet; // �¼�ί��

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
        OnItemSet += SetItem;
    }

    public void SetSliderRoation()
    {
        Differencevalue = (minHeight * slider.value * 10) - (itemHeight * itemLenght);
        slider.transform.parent.DORotate(new Vector3(0, 0, Differencevalue * 6), 1f);
    }

    public void SetScale()
    {
        DisplayScale.text = $"{(minHeight * slider.value * 10):F2}克";
    }

    public void enableScale()
    {
        DisplayScale.gameObject.SetActive(!DisplayScale.gameObject.activeSelf);
        Debug.Log("enter Or Exit");
    }

    public void SetItem(ItemMessage itemMessage)
    {
        if (itemMessage == null)
        {
            itemHeight= 0;
            itemImage.sprite= null;
            itemImage.gameObject.SetActive(false);
            itemName = null;
            SetSliderRoation();
            return;
        }
        itemHeight = itemMessage.message.ItemHeight;
        itemImage.sprite = itemMessage.message.sprite;
        itemImage.gameObject.SetActive(true);
        itemName = itemMessage.message.ItemName;
        SetSliderRoation();
    }
}
