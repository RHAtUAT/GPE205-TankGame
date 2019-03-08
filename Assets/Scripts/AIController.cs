using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    public float closeEnough = 1.0f;
    public Transform[] waypoints;
    public TankData tank;
    //public TankMotor motor;
    public enum LoopType { Stop, Loop, PingPong };
    public LoopType loopType;
    private Transform tf;
    private int currentWaypoint;
    private bool isPatrolingForward = true;

    // Start is called before the first frame update
    void Start()
    {
        tf = GetComponent<Transform>();
        //motor = GetComponentInChildren<TankMotor>();
        tank = GetComponent<TankData>();
    }

    // Update is called once per frame
    void Update()
    {
        //If tank is no longer rotating because it is facing towards the checkpoint 
        if(!tank.motor.RotateTowards(waypoints[currentWaypoint].position, tank.moveSpeed))
        {
            //Move Forward
            tank.motor.Move(tank.moveSpeed);
        }
        //Using square roots weas once resource heavy, CPUs are better now a days, but some programmers recommend not to use them
        //Vector3.Distance(tank.transform.position, waypoints[currentWaypoint].position) < closeEnough
        //This will also give us the distance without using square roots
        //Vector3.SqrMagnitude(waypoints[currentWaypoint].position - transform.position) < (closeEnough * closeEnough)
        //If tank has reached the currentcheckpoint look for next checkpoint
        Debug.DrawRay(gameObject.transform.position, waypoints[currentWaypoint].position, Color.blue);
        if (Vector3.SqrMagnitude(waypoints[currentWaypoint].position - tf.position) < (closeEnough * closeEnough))
        {
            Debug.DrawRay(waypoints[currentWaypoint].position, tf.position, Color.red);

            switch (loopType)
            {

                case LoopType.Stop:
                    //If we're in range of the array
                    if (currentWaypoint < waypoints.Length - 1)
                        currentWaypoint++;
                    break;

                case LoopType.Loop:
                    if (currentWaypoint < waypoints.Length - 1)
                        currentWaypoint++;
                    else
                        currentWaypoint = 0;
                    break;

                case LoopType.PingPong:
                    if (isPatrolingForward)
                    {
                        if (currentWaypoint < waypoints.Length - 1)
                            currentWaypoint++;
                        else
                        {
                            isPatrolingForward = false;
                            currentWaypoint--;
                        }
                    }
                    else
                    {
                        if (currentWaypoint > 0)
                            currentWaypoint--;
                        else
                        {
                            isPatrolingForward = true;
                            currentWaypoint++;
                        }
                    }

                    break;


                default: 
                    break;
            }
        }
    }
}
