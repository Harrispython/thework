using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System;

public class SliderContorler : MonoBehaviour
{
    public static SliderContorler instance;
    public Slider slider;
    public float itemHeight; // 物体重量
    public string itemName; // 物体名字
    public float itemLenght; // 物体力臂
    public Image itemImage;
    public float Coefficient; // 倾斜系数
    public float Differencevalue; // 差值
    public float minHeight;

    public TextMeshProUGUI DisplayScale; // 显示刻度

    public Action<ItemMessage> OnItemSet; // 事件委托

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
        OnItemSet += SetItem;
    }

    public void SetSliderRoation()
    {
        Differencevalue = (minHeight * slider.value * 10) - (itemHeight * itemLenght);
        slider.transform.parent.DORotate(new Vector3(0, 0, Differencevalue * 6), 1f);
    }

    public void SetScale()
    {
        DisplayScale.text = $"{(minHeight * slider.value * 10):F2}斤";
    }

    public void enableScale()
    {
        DisplayScale.gameObject.SetActive(!DisplayScale.gameObject.activeSelf);
        Debug.Log("enter Or Exit");
    }

    public void SetItem(ItemMessage itemMessage)
    {
        itemHeight = itemMessage.message.ItemHeight;
        itemImage.sprite = itemMessage.message.sprite;
        itemImage.gameObject.SetActive(true);
        itemName = itemMessage.message.ItemName;
        SetSliderRoation();
    }
}
