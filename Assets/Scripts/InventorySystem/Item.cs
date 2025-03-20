using UnityEngine;

public class Item : MonoBehaviour
{
    [Header("物品基本信息")]
    [Tooltip("物品名称")]
    public string itemName;        // 物品名称
    
    [Tooltip("物品描述")]
    public string description;     // 物品描述
    
    [Tooltip("物品图标")]
    public Sprite itemIcon;        // 物品图标
    
    [Tooltip("物品预制体")]
    public GameObject itemPrefab;  // 物品预制体
    
    [Tooltip("是否可拾取")]
    public bool isPickupable = true; // 是否可拾取
    
    [Tooltip("拾取距离")]
    public float pickupDistance = 2f; // 拾取距离
} 