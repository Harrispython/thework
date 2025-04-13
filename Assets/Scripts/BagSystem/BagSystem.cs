using echo17.EndlessBook;
using echo17.EndlessBook.Demo02;
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
        public Material ImageMateral;
        public Material DescriptionMateral;
        [TextArea]
        public string Description;
        [TextArea]
        public string GameDescription;
    }
    public EndlessBook book;
    public Demo02 Demo02;
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
        int TempInex = (int.Parse(Example08.instance.selectIndexInputField.text));
        if (book && Demo02)
        {
            book.InsertPageData(book.CurrentLeftPageNumber, Items[TempInex].DescriptionMateral);
            book.InsertPageData(book.CurrentLeftPageNumber, Items[TempInex].ImageMateral);
            Demo02.pageViews[0].gameObject.name = $"PageView_{book.LastPageNumber - 1}";
        }
        Items.RemoveAt(TempInex);
        Example08.instance.GenerateCells(Items, Items.Count);
        CloseCavans();
        //Inout.SetCounter();

    }

    public void CloseCavans()
    {
        if (TouchPad.instance.gameObject.activeSelf)
        {
            StartCoroutine(DelaySetTouchMask());
        }
        else
        {
            this.transform.parent.gameObject.SetActive(false);
            UIManager.Instance.IsUIVisible = false;
        }
    }

    private IEnumerator DelaySetTouchMask()
    {
        yield return new WaitForSeconds(0.1f);
        Debug.Log("SetTrue");
        TouchPad.instance.SetTouchMask(true);
        this.transform.parent.gameObject.SetActive(false);
    }
    public void ActiveCavans()
    {
        Animator.SetTrigger("ActiveTrigger");
    }




}
