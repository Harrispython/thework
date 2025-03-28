using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Fungus;
using UnityEngine;
using UnityEngine.SceneManagement;

public class uitrigger : MonoBehaviour
{
    public string NpcName;
    public bool canChart=true;

    public CinemachineVirtualCamera cinemachine;
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

    public void obj()
    {
        if (cinemachine != null)
        {
            cinemachine.gameObject.SetActive(false);
            StartCoroutine(wiatsecond(1f));
        }
        
    }

    public void Main3Start()
    {
        StartCoroutine(wiatsecond(1.5f));
        GetComponent<Scriptenable>().InputEnable(true);
        
    }
    
    private IEnumerator wiatsecond(float f)
    {
        Debug.Log("开始处理动画完成后的事件");

        // 等待指定时间
        yield return new WaitForSeconds(f);
        uibool();
    }

}
