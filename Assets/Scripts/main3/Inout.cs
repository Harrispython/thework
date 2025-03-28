using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inout : MonoBehaviour
{
    private int counter=0;
    private bool wasChart;

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       

        if (counter==0)
        {
            final();
            counter++;
        }
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
