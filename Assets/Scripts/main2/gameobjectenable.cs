using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameobjectenable : MonoBehaviour
{
    [SerializeField] private GameObject targetObject ;

    public void Enable()
    {
        targetObject.gameObject.SetActive(true);
    }
    
}