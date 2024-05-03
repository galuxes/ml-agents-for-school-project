using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateManager : MonoBehaviour
{
    public List<GameObject> gates = new List<GameObject>();
    public static GateManager _instance;

    private void Awake()
    {
        if (!_instance)
        {
            _instance = this;
        }
        else
        {
            Debug.Log("Too many Gate Managers!!");
        }
    }
}
