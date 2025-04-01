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
        [TextArea]
        public string Description;
        [TextArea]
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
        // �� Itemlibrary �в���ƥ�����Ʒ
        ItemMessage? foundItem = Itemlibrary.Find(item => item.Name == ItemName);

        if (foundItem.HasValue)
        {
            // ȷ�����ظ�����
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

    public void DelectItem()
    {
        Items.RemoveAt(int.Parse(Example08.instance.selectIndexInputField.text));
        Example08.instance.GenerateCells(Items, Items.Count);
        CloseCavans();
        Inout.SetCounter();

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
