using System;
using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    public bool isInRange;
    public String chatname;
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
        
        // ����Ƿ���ʰȡ��Χ��
        if (isInRange && Input.GetKeyDown(KeyCode.E))
        {
            Flowchart flowchart = GameObject.Find("Flowchart").GetComponent<Flowchart>();
            if (flowchart.HasBlock(chatname))
            {
                flowchart.ExecuteBlock(chatname);
            }
            // ����ʰȡ��Ʒ
        }
    }
    public void DestroyGameobject()
    {
        Destroy(gameObject);
    }

}
