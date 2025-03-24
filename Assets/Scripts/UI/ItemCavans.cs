using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ItemCavans : MonoBehaviour
{
    public Animator Animator;
    public Image Image; // ��Ҫ�� Image ���
    public List<Sprite> Images; // Ԥ���ͼƬ�б�

    public void SetImage(string imageName)
    {
        // �� Images �б��в���ƥ���ͼƬ
        Sprite foundImage = Images.FirstOrDefault(img => img.name == imageName);

        if (foundImage != null)
        {
            Image.sprite = foundImage; // ������ Image ����� sprite

            Animator.SetTrigger("AcitveItem");
        }
        else
        {
            Debug.LogWarning($"ͼƬ {imageName} δ�ҵ�");
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
