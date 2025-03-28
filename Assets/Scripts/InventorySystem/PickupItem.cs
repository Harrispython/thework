using Fungus;
using UnityEngine;

public class PickupItem : MonoBehaviour
{
    private ItemMessage item;                    // 物品组件
    public bool isInRange;               // 是否在拾取范围内
    private GameObject player;            // 玩家对象
    private Inventory inventory;          // 背包系统

    private void Start()
    {
        // 获取物品组件
        item=this.GetComponent<ItemMessage>();
        // 获取玩家对象
        player = GameObject.FindWithTag("Player");
        // 获取背包系统
        inventory = player.GetComponent<Inventory>();
        print(player.name);
    }

    private void Update()
    {
        
        if (isInRange)
        {
            print("在拾取范围内");
        }
        // 检查是否在拾取范围内
        if (isInRange && Input.GetKeyDown(KeyCode.E))
        {
            Flowchart flowchart = GameObject.Find("Flowchart").GetComponent<Flowchart>();
            if (flowchart.HasBlock("交互获取物品"))
            {
                flowchart.ExecuteBlock("交互获取物品"); // 播放对话
                if (ItemCavans.instacnce)
                {
                    ItemCavans.instacnce.SetImage(item.message.ItemName);//修改获取UI图片，添加物品到背包
                }

                if (item.message.ItemName == "木头")
                {
                    flowchart.SetBooleanVariable("npc3", true);
                };
            }

            // 尝试拾取物品
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // 当玩家进入拾取范围时
        if (other.CompareTag("Player"))
        {
            isInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // 当玩家离开拾取范围时
        if (other.CompareTag("Player"))
        {
            isInRange = false;
        }
    }
} 