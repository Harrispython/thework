using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Knowledge : MonoBehaviour
{
    public BagSystem.ItemMessage itemMessage;
    public Button button;
    void Start()
    {
        button.onClick.AddListener(() => BagSystem.instance.SetDescripiton(itemMessage));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
