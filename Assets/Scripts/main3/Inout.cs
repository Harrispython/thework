using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inout : MonoBehaviour
{
    public static int counter=0;
    private bool wasChart;

 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public static void SetCounter()
    {
        counter++;
        if (counter == 3)
        {
            Flowchart flowchart = GameObject.Find("Flowchart").GetComponent<Flowchart>();
            if (flowchart.HasBlock("DescriptionWindow"))
            {
                flowchart.ExecuteBlock("DescriptionWindow");
            }
            else
            {
                Debug.Log("DOn't have block");
            }
        }

    }
    // Update is called once per frame
    void Update()
    {
       

        //if (counter==3)
        //{
        //    StartCoroutine(MoveUpCoroutine());
        //    counter++;
        //}
    }


    private System.Collections.IEnumerator MoveUpCoroutine()
    {
        yield return new WaitForSeconds(3f);
        final();
    }

    public void final()
    {
        LightColorController cc = gameObject.GetComponent<LightColorController>();
        if (cc!=null)
        {
            cc.EnableParentComponents();
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            LightColorController cc = gameObject.GetComponent<LightColorController>();
            cc.Startani();
        }
    }
}
