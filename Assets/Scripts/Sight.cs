using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sight : MonoBehaviour
{

    public Vector3 vectorToTarget;
    public Transform tf;
    public Transform target;
    public float FieldOfView;
    public float viewDistance;

    // Start is called before the first frame update
    void Start()
    {
        tf = GetComponent<Transform>();    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FOV()
    {
        //Get distance this can see
    }

    public bool TargetInSight()
    {


        return false;
    }

}
