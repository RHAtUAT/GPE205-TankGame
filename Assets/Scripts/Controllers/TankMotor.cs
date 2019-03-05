using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankMotor : MonoBehaviour {

    private CharacterController cc;
    private Transform tf;

    // Use this for initialization
    void Start () {
        tf = GetComponent<Transform>();
        cc = GetComponent<CharacterController>();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Move(float forwardSpeed)
    {
        Vector3 speedVector = tf.forward;

        // SimpleMove() will apply Time.deltaTime, and convert to meters per second for us!
        cc.SimpleMove(speedVector *= forwardSpeed); 
    }

    public void Rotate(float rotatonSpeed)
    {
        // Vector3.up is the axis to rotate around
        Vector3 rotateVector = Vector3.up; 
        rotateVector *= rotatonSpeed;

        tf.Rotate( rotateVector, Space.Self);
    }
}
