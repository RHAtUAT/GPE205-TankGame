using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankWheel : MonoBehaviour
{
    public GameObject gObject;

    private void Awake()
    {
        gObject = GetComponent<GameObject>();
    }
}
