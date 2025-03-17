using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class SliderContorler : MonoBehaviour
{
    public Slider slider;
    public float itemHeight;//��������
    public float itemLenght;//��������
    public float Coefficient;//��бϵ��
    public float Differencevalue;//��ֵ
    public float minHeight;

    public TextMeshProUGUI DisplayScale;//��ʾ�̶�


    public void SetSliderRoation()
    {
        Differencevalue = (minHeight * slider.value * 10) - (itemHeight * itemLenght);
        slider.transform.parent.DORotate(new Vector3(0, 0, Differencevalue * 6), 1f);
    }

    public void SetScale()
    {
        DisplayScale.text = $"{(minHeight * slider.value * 10):F2}��";
    }

    public void enableScale()
    {
        DisplayScale.gameObject.SetActive(!DisplayScale.gameObject.activeSelf);
        Debug.Log("enter Or Exit");
    }

}
