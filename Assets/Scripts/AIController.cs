using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    public bool targetInSight;
    public bool canMove;
    public bool canHear;
    public bool searching = false;

    public float closeEnough = 1.0f;
    public float hearingDistance = 2.0f;
    public float fleeDistance = 10.0f;
    public float avoidanceTime = 2.0f;
    public float fieldOfView = 90f;
    public float viewDistance = 10f;
    public float lostTrackTime = 5.0f;
    public float time = 5.0f;

    public enum PatrolType { Stop, Loop, PingPong, None };
    public PatrolType patrolType;
    public enum AttackMode { Chase, Flee, None };
    public AttackMode attackMode;
    public enum AvoidType { Turn, Move, };
    public AvoidType avoidType;

    public Transform target;
    public TankData pawn;
    public Transform[] waypoints;

    private PatrolType lastPatrolType;
    private PatrolType currentPatrolType;
    private bool isPatrolingForward = true;
    private int avoidStage = 0;
    private int currentWaypoint;
    private float exitTime;
    private Transform tf;
    private Sight sight;

    // Start is called before the first frame update
    void Start()
    {
        time = Time.time;
        tf = pawn.transform;
        target = GameObject.FindGameObjectWithTag("Player").transform;
        sight = pawn.GetComponentInChildren<Sight>();
        sight.fieldOfView = fieldOfView;
        sight.viewDistance = viewDistance;
        sight.target = target;
        currentPatrolType = patrolType;
        //Ideally we could inizialize target once at the beginning
    }

    // Update is called once per frame
    void Update()
    {

        patrolType = currentPatrolType;
        canMove = CanMoveForward();
        canHear = CanHear();
        targetInSight = sight.TargetInSight();
        mPatrolType();

        //This is returning Null
        //target = GameManager.instance.player.pawn.transform;

        if (canHear || targetInSight)
        {
            time = Time.time;
            if (!searching)
            {
                lastPatrolType = currentPatrolType;
                currentPatrolType = PatrolType.None;
            }
            searching = true;

            if (attackMode == AttackMode.Chase)
                Chase();
            if (attackMode == AttackMode.Flee)
                Flee();
            if (attackMode == AttackMode.None) { }

        }

        if (searching && Time.time > time + lostTrackTime)
        {
            searching = false;
            Debug.Log("lost track");
            currentPatrolType = lastPatrolType;
        }
    }

    //TODO: CanHear
    public bool CanHear()
    {
        float distance = Vector3.Distance(target.position, tf.position);
        if (distance > hearingDistance)
            return false;
        else
            return true;
    }

    //TODO: getPatrolSate
    void mPatrolType()
    {
        if (waypoints.Length > 0)
        {

            Vector3 vectorToTarget = waypoints[currentWaypoint].position - tf.position;

            Debug.DrawRay(pawn.motor.tf.position, vectorToTarget, Color.blue);

            MoveToTarget(waypoints[currentWaypoint].position);

            if(Vector3.Distance(waypoints[currentWaypoint].position, tf.position) < closeEnough)
            {
                switch (currentPatrolType)
                {
                    case PatrolType.Stop:
                        //If we're in range of the array
                        if (currentWaypoint < waypoints.Length - 1)
                            currentWaypoint++;
                        break;

                    case PatrolType.Loop:
                        if (currentWaypoint < waypoints.Length - 1)
                            currentWaypoint++;
                        else
                            currentWaypoint = 0;
                        break;

                    case PatrolType.PingPong:
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
        else
            Debug.Log("No WayPoints Found");
    }

    //TODO: Flee
    void Flee()
    {
        //get a vector to the target
        //reverse it so the AI moves in the opposite direction
        //Make a Timer so the AI moves in the opposite direction during this timer
        //Use can move forward to determine which way the AI can go during
        //Use Avoid to make the AI not run into walls while fleeing
        //Vector3 vectorToTarget = 

    }

    //TODO: CanMoveForward
    bool CanMoveForward()
    {
        //Raycast forward, backwards, left and right
        RaycastHit forwardHit;
        RaycastHit backwardHit;
        RaycastHit leftHit;
        RaycastHit rightHit;

        Vector3 forwardVector = tf.forward + tf.position;
        Vector3 backwardVector = tf.forward - tf.position;
        Vector3 rightVector = tf.right + tf.position;
        Vector3 leftVector = tf.right - tf.position;

        bool forwardCast = Physics.Raycast(tf.position, forwardVector, out forwardHit, pawn.moveSpeed);
        bool backwardCast = Physics.Raycast(tf.position, forwardVector, out backwardHit, pawn.moveSpeed);
        bool rightCast = Physics.Raycast(tf.position, forwardVector, out rightHit, pawn.moveSpeed);
        bool leftCast = Physics.Raycast(tf.position, forwardVector, out leftHit, pawn.moveSpeed);

        //We hit something so we cant move
        Debug.DrawRay(tf.position, pawn.motor.transform.forward, Color.black);
        if (forwardCast)
            return false;
        else
            return true;

        //Prevent raycast from triggering on hitting player

    }


    //TODO: Chase

    void Chase()
    {
        //The vector from this to the Target
        Vector3 vectorToTarget = target.position - tf.position;
        Debug.DrawRay(tf.position, vectorToTarget, Color.white);
        //If we cant rotate because we are already facing the target
        MoveToTarget(target.position);

    }

    //Avoid obstacles and walls
    void MoveToTarget(Vector3 target)
    {

        //If there's nothing in front of us move forward
        if (CanMoveForward())
        {
            pawn.motor.RotateTowards(target);


                pawn.motor.Move(pawn.moveSpeed);
        }
        else
        {
            pawn.motor.Rotate(-1 * pawn.turnSpeed);
            pawn.motor.Move(pawn.moveSpeed);

            //Assume we cant move forward whats the next step?
            //It's to rotate until we can
        }

        //Physics.Raycast

    }

    //TODO: Avoid
    //TODO:
    //TODO:
    //TODO:
    //TODO:

}
