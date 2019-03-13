using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankMotor : MonoBehaviour {

    private CharacterController cc;
    [HideInInspector] public Transform tf;
    private TankData tank;

    // Use this for initialization
    void Start () {

        tank = GetComponentInParent<TankData>();
        tf = GetComponent<Transform>();
        cc = GetComponentInParent<CharacterController>();
    }
	
	// Update is called once per frame
	void Update () {
	

	}

    public void Move(float forwardSpeed)
    {
        Vector3 speedVector = tf.forward;

        //SimpleMove() will apply Time.deltaTime, and convert to meters per second for us!
        cc.SimpleMove(speedVector *= forwardSpeed); 
    }

    public void Rotate(float rotatonSpeed)
    {
        // Vector3.up is the axis to rotate around
        Vector3 rotateVector = Vector3.up; 
        rotateVector *= rotatonSpeed;

        tf.Rotate(rotateVector);
    }

    public bool RotateTowards(Vector3 target)
    {
        //The difference/distance between this gameeObjects postion and the targets position
        Vector3 vectorToTarget = target - tf.position;

        //Find the Quaternion/Rotation that looks down the given vector 
        Quaternion desiredRotation = Quaternion.LookRotation(vectorToTarget);

        //If we dont need to rotate because we are already looking in that direction
        //return false
        if (tf.rotation == desiredRotation)
        {
            return false;
        }

        //Otherwise
        //Rotate towards the vector every second(Time.deltatime) by the turnSpeed
        float step = tank.turnSpeed * Time.deltaTime;
        tf.rotation = Quaternion.RotateTowards(tf.rotation, desiredRotation, step);

        //We rotated so return true
        return true;

    }
}
