using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ItemCavans : MonoBehaviour
{
    public Animator Animator;
    public Image Image; // 主要的 Image 组件
    public List<Sprite> Images; // 预存的图片列表

    public void SetImage(string imageName)
    {
        // 在 Images 列表中查找匹配的图片
        Sprite foundImage = Images.FirstOrDefault(img => img.name == imageName);

        if (foundImage != null)
        {
            Image.sprite = foundImage; // 更新主 Image 组件的 sprite

            Animator.SetTrigger("AcitveItem");
        }
        else
        {
            Debug.LogWarning($"图片 {imageName} 未找到");
        }

    }

    public void CloseItem()
    {
        Animator.SetTrigger("CloseItem");
    }
    public void Disactive()
    {
        this.gameObject.SetActive(false);
    }
}
