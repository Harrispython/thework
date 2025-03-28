using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ItemCavans : MonoBehaviour
{
    public Animator Animator;
    public Image Image; // ��Ҫ�� Image ���
    public List<Sprite> Images; // Ԥ���ͼƬ�б�
    public static ItemCavans instacnce;


    private void Awake()
    {
         if(instacnce == null)
        {
            instacnce = this;
            this.gameObject.SetActive(false);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void SetImage(string imageName)
    {
        // �� Images �б��в���ƥ���ͼƬ
        Sprite foundImage = Images.FirstOrDefault(img => img.name == imageName);

        if (foundImage != null)
        {
            Image.sprite = foundImage; // ������ Image ����� sprite
            BagSystem.instance.AddItem(imageName);//��ӵ�����
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
        Image.rectTransform.localScale = Vector3.one;
    }
    public void Disactive()
    {
        this.gameObject.SetActive(false);
    }
}
