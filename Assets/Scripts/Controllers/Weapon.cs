using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {
    [HideInInspector] public Transform tf;
    // Use this for initialization
    void Start () {
        tf = GetComponent<Transform>();
    }
	
}
