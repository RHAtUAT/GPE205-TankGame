using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chase : MonoBehaviour
{

    public enum AttackMode { Chase, Flee};
    public AttackMode attackMode;
    public float fleeDistance = 1.0f;
    public Transform target;

    private Transform tf;
    private TankData tank;
    private TankMotor motor;


    // Start is called before the first frame update
    void Start()
    {
        tf = GetComponent<Transform>();
        tank = GetComponent<TankData>();
        motor = GetComponentInChildren<TankMotor>();

        
    }

    // Update is called once per frame
    void Update()
    {
        //Attack
        if(attackMode == AttackMode.Chase)
        {
            //Rotate towards our target
            motor.RotateTowards(target.position, tank.moveSpeed);
            //Move forward
            motor.Move(tank.moveSpeed);
        }

        //Flee
        if (attackMode == AttackMode.Flee)
        {
            //Vector from Target to this gameObject
            Vector3 vectorToTarget = target.position - tf.position;

            //Make it the opposite direction of the target
            Vector3 vectorAwayFromTarget = vectorToTarget * -1;

            //Make it 1 unit in length
            Vector3.Normalize(vectorAwayFromTarget);

            //Flee to the distance we chose
            vectorAwayFromTarget *= fleeDistance;

            //The postion the AI will move to
            Vector3 fleePositon = tf.position + vectorAwayFromTarget;
            
            //Rotate towards the position and move to it
            motor.RotateTowards(fleePositon, tank.turnSpeed);
            motor.Move(tank.moveSpeed);
        }
        
    }

    bool TargetInSight(GameObject target)
    {
        if()

        return false;
    }
}
