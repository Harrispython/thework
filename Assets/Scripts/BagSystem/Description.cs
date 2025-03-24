using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Description : MonoBehaviour
{
    public TextMeshProUGUI DescriptionText;
    public Image DescriptionImage;
    public TextMeshProUGUI DescriptionName;
 
    public void SetMessage(BagSystem.ItemMessage itemMessage)
    {
        DescriptionText.text = itemMessage.Description;
        DescriptionImage.sprite = itemMessage.Image;
        DescriptionName.text = $"{itemMessage.Name}\nÆðÔ´:{itemMessage.OriginalDate}";
    }

}
