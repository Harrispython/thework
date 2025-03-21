using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [Header("背包设置")]
    [Tooltip("背包容量")]
    public int maxItems = 20;             // 背包最大容量
    
    [Tooltip("当前物品列表")]
    public List<Item> items = new List<Item>();  // 当前背包中的物品

    // 添加物品到背包
    public bool AddItem(Item item)
    {
        // 检查背包是否已满
        if (items.Count >= maxItems)
        {
            Debug.Log("背包已满！");
            return false;
        }

        // 添加物品到背包
        items.Add(item);
        Debug.Log($"拾取了物品：{item.itemName}");
        return true;
    }

    // 从背包中移除物品
    public void RemoveItem(Item item)
    {
        if (items.Contains(item))
        {
            items.Remove(item);
            Debug.Log($"移除了物品：{item.itemName}");
        }
    }

    // 检查背包中是否有特定物品
    public bool HasItem(string itemName)
    {
        return items.Exists(item => item.itemName == itemName);
    }
} 