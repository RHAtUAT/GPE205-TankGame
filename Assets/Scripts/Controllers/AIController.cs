using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    [Header("AI Senses")]

    //Prevent the gameObject from rotating towards the target while it rotates away from whats blocking it 
    //Fixes stuttering caused by the gameObject constantly trying to rotate between two positions
    private bool turningRight = false;
    private bool turningLeft = false;
    //Prevents using time as a condition unless it's been set 
    public bool targetInSight;
    public bool canHear;
    public bool searching = false;
    public enum CastHit { None, Right, Left, Both };
    public CastHit castHit;
    public Vector3 lastSeenLocation;

    [Header("AI Properties")]
    public float hearingDistance = 2.0f;
    public float sightDistance = 10.0f;
    public float fleeDistance = 10.0f;
    public float chaseDistance = 5.0f;
    public float fieldOfView = 90.0f;
    [Tooltip("How far away the AI can be from something for it to still count")]
    public float distanceOffset = 3.0f;
    public float collisionRaycastLength = 2.0f;
    public Vector3 collisionRaycastPosition = new Vector3(0.7f, 1.0f, -0.1f);
    public enum PatrolType { Stop, Loop, PingPong, Idle };
    public PatrolType currentPatrolType;
    public enum AttackMode { Chase, Flee, Searching, Idle };
    public AttackMode attackMode;
    public int healthToFlee = 20;

    public float fleeTime = 2.0f;
    public float searchTime = 5.0f;

    public float avoidTime = 1.0f;
    [Header("Pawn Properties")]
    public int maxHealth = 100;
    public int currentHealth;

    [Header("Motor Properties")]
    public float moveSpeed = 7.0f;
    public float angularSpeed = 50.0f;
    public float turretSpeed = 50.0f;

    [Header("GameObject Data")]
    public TankData pawn;
    public Transform target;
    public Transform[] waypoints;

    private PatrolType lastPatrolType;
    private AttackMode lastAttackMode;

    private bool timeSet = false;
    private bool isPatrollingForward = true;
    private int currentWaypoint;
    private float gameTime;
    private float exitTime;
    private float avoidanceResetTime;
    private Transform tf;
    private Health health;
    private Sight sight;

    //TODO: Make the AI shoot at the target
    //TODO: Make the AI's weapon and turret pivot 
    //TODO: Make an AI that moves slow and instantly kills you it it collides with you

    // Start is called before the first frame update
    void Start()
    {
        tf = pawn.transform;
        target = GameObject.FindGameObjectWithTag("Player").transform;
        //target = GameManager.instance.player.pawn.gameObject.transform;
        // TODO : Find game object with tag shoud be used only as a last resort, consider referencing the GM instead
        //Debug.Log(GameManager.instance);
        health = pawn.GetComponent<Health>();
        currentHealth = maxHealth;
        sight = pawn.GetComponentInChildren<Sight>();
        // TODO : Moved static values initialized to start
        pawn.turnSpeed = angularSpeed;
        pawn.moveSpeed = moveSpeed;
        sight.target = target;
    }

    // Update is called once per frame
    void Update()
    {
        MovementManager();

        //Show the values in the inspector
        castHit = (CastHit)CanMoveForward();
        canHear = CanHear();
        targetInSight = sight.TargetInSight();
        sight.fieldOfView = fieldOfView;
        sight.viewDistance = sightDistance;
    }

    //Handle movement depending on PatrolSate and AttackMode
    void MovementManager()
    {
        //If the AI sees or hears the target
        if (canHear || targetInSight)
        {

            if (targetInSight)
            {
                RotateTurret();
                Fire();
            }
            if (canHear)
            {
                RotateTurret();
            }
            //Keep track of time
            gameTime = Time.time;

            //This will be the targets current position
            //for as long as the target is seen
            lastSeenLocation = target.position;

            //Get the current states before setting the new ones
            //and wait until the AI isn't searching in order to set it again
            //This is preventing the lastPatrolType and lastAttackMode
            //from being overwritten every frame
            if (searching == false && attackMode != AttackMode.Idle)
            {
                //Store the last states
                lastPatrolType = currentPatrolType;
                lastAttackMode = attackMode;

                //And make the AI stop looking for waypoints to move towards
                currentPatrolType = PatrolType.Idle;
                //Debug.Log("Hear");

                //The AI is now set to search for the target if it loses track
                searching = true;
            }

            //If the AI's low on health and it's not currently in the flee state
            //set it to the flee State
            if (health.currentHealth <= healthToFlee && attackMode != AttackMode.Flee)
                attackMode = AttackMode.Flee;

            //If the AI is searching and it finds the target
            //Stop searching and put it in its lastAttackMode
            if (attackMode == AttackMode.Searching)
                attackMode = lastAttackMode;

            if (attackMode == AttackMode.Chase)
                Chase();

            if (attackMode == AttackMode.Flee)
                Flee();

            if (attackMode == AttackMode.Idle)
                Patrol();
        }
        //If AI cant see or hear target
        else if (!canHear && !targetInSight)
        {
            //If it's time for the AI to search and it isnt trying to to run away
            if (searching == true && Time.time < gameTime + searchTime && attackMode != AttackMode.Flee && attackMode != AttackMode.Idle)
            {
                attackMode = AttackMode.Searching;
            }
            else if (searching == true && Time.time > gameTime + searchTime)
            {
                //Give the AI its previous settings
                searching = false;
                attackMode = lastAttackMode;
                currentPatrolType = lastPatrolType;
                Debug.Log("lost track");
            }

            if (attackMode == AttackMode.Searching)
            {
                Search();
            }

            //Go back to patrolling
            Patrol();
        }
    }

    //Returns true if the target is within AI hearingDistance
    public bool CanHear()
    {
        float distance = Vector3.Distance(target.position, tf.position);
        //Debug.DrawLine(target.position, pawn.motor.transform.position, Color.white);
        return distance <= hearingDistance;
    }

    //Set how the different patrol types act
    void Patrol()
    {
        //If there are waypoints to patrol to 
        if (waypoints.Length <= 0)
        {
            //There are no Waypoints send a message
            Debug.LogWarning("No WayPoints Found");
            return;
        }
        //Show the vector to the waypoint

        if (currentPatrolType == PatrolType.Stop && currentWaypoint == waypoints.Length && Vector3.Distance(waypoints[currentWaypoint].position, pawn.motor.transform.position) >= distanceOffset)
        {
            MoveToTarget(waypoints[currentWaypoint].position);
            //Debug.Log("currentWaypoint " + currentWaypoint);
            //Debug.Log("Waypoint[] " + waypoints.Length);
            Debug.DrawLine(pawn.motor.transform.position, waypoints[currentWaypoint].position, Color.blue);

        }
        else if (currentPatrolType == PatrolType.Idle)
        {
            //Debug.Log("Idle");
            return;
        }
        else
        {
            Debug.DrawLine(pawn.motor.transform.position, waypoints[currentWaypoint].position, Color.blue);
            MoveToTarget(waypoints[currentWaypoint].position);
        }
        //Move to the Waypoint

        //If the AI is not close enough to the Waypoint, return
        if (Vector3.Distance(waypoints[currentWaypoint].position, tf.position) > distanceOffset) return;

        //Get the currentPatrolType
        switch (currentPatrolType)
        {
            case PatrolType.Stop:
                //If we're in range of the array
                if (currentWaypoint < waypoints.Length - 1)
                    //Get the next waypoint
                    currentWaypoint++;
                break;

            case PatrolType.Loop:
                if (currentWaypoint < waypoints.Length - 1)
                    currentWaypoint++;
                else
                    //Get the first waypoint (so start over)
                    currentWaypoint = 0;
                break;

            case PatrolType.PingPong:
                if (isPatrollingForward)
                {
                    //If we're in the range of the array
                    if (currentWaypoint < waypoints.Length - 1)
                        currentWaypoint++;
                    else
                    {
                        //We aren't patrolling forward
                        isPatrollingForward = false;
                        //Get the previous waypoint
                        currentWaypoint--;
                    }
                }
                //If we aren't patrolling forward
                else
                {
                    //And if we aren't at the last waypoint
                    if (currentWaypoint > 0)
                        currentWaypoint--;

                    //If we are at the last waypoint 
                    else
                    {
                        //Set isPatrollingForward to true to restart the loop
                        isPatrollingForward = true;
                        currentWaypoint++;
                    }
                }
                break;

            default:
                break;
        }
    }

    //Makes the AI move away from the target 
    void Flee()
    {
        //Make a Timer so the AI moves in the opposite direction during this time
        if (Time.time >= gameTime + fleeTime)
        {
            //Debug.Log("Hit");
            attackMode = lastAttackMode;
            return;
        }
        //Reverse lastSeenLocation so the AI moves in the opposite direction
        //We need to make it a vector before we can normalize
        Vector3 fleeVector = -1 * (lastSeenLocation - tf.position);

        //Normalize it so we can multiply by fleeDistance and tell the AI how far away to move
        fleeVector.Normalize();
        fleeVector *= fleeDistance;

        //This gives us a point that is "that vector away" from our current position.
        Vector3 fleePosition = fleeVector + tf.position;

        MoveToTarget(fleePosition);
        Debug.DrawLine(tf.position, fleePosition, Color.black);
        //Debug.Log("Flee");
    }

    void Search()
    {
        MoveToTarget(lastSeenLocation);
        Debug.DrawLine(tf.position, lastSeenLocation, Color.red);
        //Debug.Log("Searching");
    }

    //Makes AI move towards the targets position
    //Simply here for clearer readability in MovementManager
    void Chase()
    {
        lastAttackMode = attackMode;
        if (Vector3.Distance(pawn.motor.transform.position, target.transform.position) > chaseDistance )
        {
            MoveToTarget(lastSeenLocation);
        }
        Debug.DrawLine(pawn.motor.transform.position, target.position, Color.white);
        //Debug.Log("Chase");
    }

    void RotateTurret()
    {
        Vector3 vectorToTarget = target.transform.position - pawn.weaponData.turret.transform.position;
        Quaternion vectorToQuaternion = Quaternion.LookRotation(vectorToTarget);
        float step = turretSpeed * Time.deltaTime;
        pawn.weaponData.turret.transform.rotation = Quaternion.RotateTowards(pawn.weaponData.turret.transform.rotation, vectorToQuaternion, step);
    }

    void Fire()
    {
        Debug.Log(pawn.weaponData.fireRate);
        pawn.weaponData.Fire();
    }

    //Return whether the forward raycasts hit something or not  
    int CanMoveForward()
    {
        //Used for collecting data about what the raycasts hit
        RaycastHit forwardRightHit;
        RaycastHit forwardLeftHit;

        //Add the collisionRaycastOffset to the gameObjects position to get the positon in world space where the raycasts will begin, the origin
        Vector3 raycastRightPosition = pawn.motor.transform.position + new Vector3(collisionRaycastPosition.x, collisionRaycastPosition.y, collisionRaycastPosition.z);
        Vector3 raycastLeftPosition = pawn.motor.transform.position + new Vector3(-collisionRaycastPosition.x, collisionRaycastPosition.y, collisionRaycastPosition.z);

        //Get the Y Rotation of the gameObject
        Quaternion quaternionY = Quaternion.Euler(0, pawn.motor.transform.eulerAngles.y, 0);

        //Create a vector from the gameObjects position to the origin of each raycast
        Vector3 vectorToRightOrigin = pawn.motor.transform.position - raycastRightPosition;
        Vector3 vectorToLeftOrigin = pawn.motor.transform.position - raycastLeftPosition;

        //Multiply by quaternionY so the raycasts origin rotates with the gameObject
        //Create a vector from the gameObjects position to the origin of each raycast
        bool forwardRightCast = Physics.Raycast(pawn.motor.transform.position - (quaternionY * vectorToRightOrigin), pawn.motor.transform.forward, out forwardRightHit, collisionRaycastLength);
        bool forwardLeftCast = Physics.Raycast(pawn.motor.transform.position - (quaternionY * vectorToLeftOrigin), pawn.motor.transform.forward, out forwardLeftHit, collisionRaycastLength);

        //Display the raycasts (red for right)
        Debug.DrawRay(pawn.motor.transform.position - (quaternionY * vectorToRightOrigin), pawn.motor.transform.forward * collisionRaycastLength, Color.red);
        Debug.DrawRay(pawn.motor.transform.position - (quaternionY * vectorToLeftOrigin), pawn.motor.transform.forward * collisionRaycastLength, Color.yellow);

        //Return a varible depending on the raycast hit
        if (forwardRightCast && !forwardLeftCast)
            return (int)CastHit.Right;
        else if (forwardLeftCast && !forwardRightCast)
            return (int)CastHit.Left;
        else if (forwardRightCast && forwardRightCast)
            return (int)CastHit.Both;
        else
            return (int)CastHit.None;

        //Optional: Prevent raycast from triggering on player
    }

    //Avoid obstacles and walls while moving to targetPosition
    void MoveToTarget(Vector3 targetPosition)
    {
        //If AI is close enough to its target
        if (Vector3.Distance(targetPosition, tf.position) <= distanceOffset) return;

        //If there's nothing in front of us  
        if (CanMoveForward() == 0)
        {
            //If we aren't avoiding obstacles
            if (turningRight == false && turningLeft == false)
            {
                //RotateTowards the targets position
                pawn.motor.RotateTowards(targetPosition);
            }

            //Move forward
            pawn.motor.Move(pawn.motor.transform.forward);

        }
        else if (CanMoveForward() == 1 || turningLeft == true)
        {
            //Right cast hit something so turn left
            turningLeft = true;
            pawn.motor.Rotate(-1 * pawn.turnSpeed);
            //Debug.Log("Right Cast");

            if (CanMoveForward() == 0)
            {
                avoidanceResetTime = Time.time;
                timeSet = true;
            }
        }
        else if (CanMoveForward() == 2 || turningRight == true)
        {
            //Left cast or both casts hit something so turn right
            turningRight = true;
            pawn.motor.Rotate(pawn.turnSpeed);
            //Debug.Log("Left Cast");

            if (CanMoveForward() == 0)
            {
                avoidanceResetTime = Time.time;
                timeSet = true;
            }
        }
        else
        {
            //Rotate to the left
            turningLeft = true;
            pawn.motor.Rotate(-1 * pawn.turnSpeed);
            // Debug.Log("else");

            if (CanMoveForward() == 0)
            {
                avoidanceResetTime = Time.time;
                timeSet = true;
            }
        }

        if (timeSet && Time.time >= avoidanceResetTime + avoidTime)
        {
            Debug.Log("Reset");
            timeSet = false;
            turningLeft = false;
            turningRight = false;
        }
    }
}
