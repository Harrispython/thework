using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class SliderContorler : MonoBehaviour
{
    public Slider slider;
    public float itemHeight;//物体重量
    public float itemLenght;//物体力臂
    public float Coefficient;//倾斜系数
    public float Differencevalue;//差值
    public float minHeight;

    public TextMeshProUGUI DisplayScale;//显示刻度


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

}
