using FancyScrollView.Example08;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BagSystem : MonoBehaviour
{
    public static BagSystem instance;
    public Animator Animator;
    [System.Serializable]
    public struct ItemMessage
    {
        public int Inex;
        public string Name;
        public string OriginalDate;
        public Sprite Image;
        public string Description;
        public string GameDescription;
    }
    [SerializeField]
    public List<ItemMessage> Items;
    public List<ItemMessage> Itemlibrary;

    void Start()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
            this.transform.parent.gameObject.SetActive(false);
        }
    }
    private void OnEnable()
    {
        ActiveCavans();
    }

    public void SetDescripiton(BagSystem.ItemMessage itemMessage)
    {
        this.transform.Find("BackGround").Find("Description").GetComponent<Description>().SetMessage(itemMessage);
    }

    //public void SetItemList()
    //{
    //    Example08.instance.GenerateCells(Items, Items.Count);

    //}
    public void AddItem(string ItemName)
    {
        // 在 Itemlibrary 中查找匹配的物品
        ItemMessage? foundItem = Itemlibrary.Find(item => item.Name == ItemName);

        if (foundItem.HasValue)
        {
            // 确保不重复添加
            if (!Items.Exists(item => item.Name == ItemName))
            {
                Items.Add(foundItem.Value);
            }
            else
            {
                Debug.LogWarning($"Item '{ItemName}' already exists in the bag.");
            }
        }
        else
        {
            Debug.LogWarning($"Item '{ItemName}' not found in the library.");
        }
    }

    public void CloseCavans()
    {
        this.transform.parent.gameObject.SetActive(false);
        UIManager.Instance.IsUIVisible= false;
    }
    public void ActiveCavans()
    {
        Animator.SetTrigger("ActiveTrigger");
    }


}
