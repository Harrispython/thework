using System.Collections;
using System.Collections.Generic;
using Fungus;
using UnityEngine;
using UnityEngine.SceneManagement;

public class uitrigger : MonoBehaviour
{
    public string NpcName;
    public bool canChart=true;

    public void uibool()
    {
        if (canChart)
        {
            Flowchart flowchart = GameObject.Find("Flowchart").GetComponent<Flowchart>();
            if (flowchart.HasBlock(NpcName))
            {
                print("播放");
                flowchart.ExecuteBlock(NpcName); // 播放对话
            }
        }
    }
    
    
}
