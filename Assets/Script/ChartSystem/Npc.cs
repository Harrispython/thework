using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class Npc : MonoBehaviour
{
    public string NpcName;
    public bool canChart;

    private void OnTriggerEnter(Collider other)
    {
        canChart= true;
    }

    private void OnTriggerExit(Collider other)
    {
        canChart= false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)&&canChart)
        {
            Flowchart flowchart=GameObject.Find("Flowchart").GetComponent<Flowchart>();
            if (flowchart.HasBlock(NpcName))
            {
                flowchart.ExecuteBlock(NpcName);//²¥·Å¶Ô»°
            }
        }
    }
}
