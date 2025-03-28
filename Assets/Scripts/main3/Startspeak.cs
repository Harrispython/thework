using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class Startspeak : MonoBehaviour
{
    public string NpcName;
    public bool canChart;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Speak()
    {
        StopCoroutine(MoveUpCoroutine());
        Flowchart flowchart = GameObject.Find("Flowchart").GetComponent<Flowchart>();
        if (flowchart.HasBlock(NpcName))
        {
            canChart = false;
            flowchart.ExecuteBlock(NpcName); // 播放对话
        }
    }
    
    
    
    private System.Collections.IEnumerator MoveUpCoroutine()
    {
        yield return new WaitForSeconds(1.5f);
    }
}
