using System;
using System.Collections;
using System.Collections.Generic;
using MoonSharp.Interpreter;
using UnityEngine;
using UnityEngine.InputSystem;

public class Scriptenable : MonoBehaviour
{
    
    public void InputEnable(bool enable)
    {
        GameObject player = GameObject.Find("Player");
        player.GetComponent<PlayerInput>().enabled = enable;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GetComponent<gameobjectenable>().enableStart();
            InputEnable(false);
        }
    }
}
