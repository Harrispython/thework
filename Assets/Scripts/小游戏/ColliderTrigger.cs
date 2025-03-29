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
            print("��ʰȡ��Χ��");
        }
        // ����Ƿ���ʰȡ��Χ��
        if (isInRange && Input.GetKeyDown(KeyCode.E))
        {
            Flowchart flowchart = GameObject.Find("Flowchart").GetComponent<Flowchart>();
            if (flowchart.HasBlock("���ٴ�ʯ"))
            {
                flowchart.ExecuteBlock("���ٴ�ʯ");
            }
            // ����ʰȡ��Ʒ
        }
    }
    public void DestroyGameobject()
    {
        Destroy(this.transform.parent.gameObject);
    }

}
