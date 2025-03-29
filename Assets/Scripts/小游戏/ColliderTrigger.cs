using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class ColliderTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    public bool isInRange;
    private void OnTriggerEnter(Collider other)
    {
        isInRange= true;
    }
    private void OnTriggerExit(Collider other)
    {
        isInRange=false;
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
            if (flowchart.HasBlock("销毁磁石"))
            {
                flowchart.ExecuteBlock("销毁磁石");
            }
            // 尝试拾取物品
        }
    }
    public void DestroyGameobject()
    {
        Destroy(this.transform.parent.gameObject);
    }

}
