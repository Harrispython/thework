using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMessage : MonoBehaviour
{
    [System.Serializable]
    public   struct Message
    {
        public float ItemHeight;
        public string ItemName;
        public Sprite sprite;
        public string Description;
    }
    [SerializeField]
    public Message message;

    public void SetItemToSlider()
    {
        SliderContorler.instance.OnItemSet?.Invoke(this);
        DescriptionWindow.instance.OnWindowSet?.Invoke(this);
    }
}
