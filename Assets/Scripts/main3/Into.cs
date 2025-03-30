using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;
using Collision = UnityEngine.Collision;

public class Into : MonoBehaviour
{
    private bool wasChart;
    // Start is called before the first frame update
    void Start()
    {
        // StartCoroutine(MoveUpCoroutine());
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && GetComponent<Npc>()!=null)
        {
            if (!wasChart&&GetComponent<Npc>().canChart)
            {
                BagSystem.instance.transform.parent.gameObject.SetActive(true);
                GetComponent<gameobjectenable>().enableStart();
                UIManager.Instance.IsUIVisible = true;
                Inout.SetCounter();
                wasChart = true;
            }
        }

        
    }

    

   

    private System.Collections.IEnumerator MoveUpCoroutine()
    {
        yield return new WaitForSeconds(3.5f);
    }
}
