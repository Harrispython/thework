using System.Collections;
using System.Collections.Generic;
using MoonSharp.Interpreter;
using UnityEngine;
using UnityEngine.InputSystem;

public class Scriptenable : MonoBehaviour
{
    public GameObject Player;
    
    public void InputEnable(bool enable)
    {
        Player.GetComponent<PlayerInput>().enabled = enable;
    }
}
