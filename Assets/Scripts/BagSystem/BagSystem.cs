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
    }
    [SerializeField]
    public List<ItemMessage> Items;
     
    void Start()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        else
        {
            instance = this;

        }
        DontDestroyOnLoad(gameObject);
    }
    private void OnEnable()
    {
        ActiveCavans();
    }

    public void SetDescripiton(BagSystem.ItemMessage itemMessage)
    {
        this.transform.Find("BackGround").Find("Description").GetComponent<Description>().SetMessage(itemMessage);
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
