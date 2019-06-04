using Assets.Scripts;
using UnityEngine;

/*
 * TODO: Make barrel pivot on x axis to look at target
 * TODO: Clamp Barrel
 * TODO: Optimise, clean up code, move functions to different classes 
 */
public class AIController : MonoBehaviour
{
    [Header("AI Senses")]

    //Fixes stuttering caused by the gameObject constantly trying to rotate between two positions
    private bool turningRight = false;
    private bool turningLeft = false;
    public bool targetInSight;
    public bool canHear;
    public bool searching = false;
    [HideInInspector] public bool firstSpawn = true;
    public enum CastHit { None, Right, Left, Both };
    public CastHit castHit;
    public Vector3 lastSeenLocation;

    [Header("AI Properties")]
    public float hearingDistance = 14.0f;
    public float sightDistance = 10.0f;
    public float fleeDistance = 10.0f;
    public float chaseDistance = 5.0f;
    public float fieldOfView = 90.0f;
    [Tooltip("How far away the AI can be from something for it to still count")]
    public float waypointRadius = 1.0f;
    public float collisionRaycastLength = 8.0f;
    public Vector3 collisionRaycastPosition = new Vector3(1.5f, 1.0f, -0.1f);
    public enum PatrolType { Stop, Loop, PingPong, Idle };
    public PatrolType currentPatrolType;
    public enum AttackMode { Chase, Flee, Searching, Idle };
    public AttackMode attackMode;
    public int healthToFlee = 20;

    public float fleeTime = 5.0f;
    public float searchTime = 4.0f;

    public float avoidTime = 1.0f;
    [Header("Pawn Properties")]
    public int maxHealth = 100;
    public int currentHealth;

    [Header("Motor Properties")]
    public float forwardSpeed = 15.0f;
    public float angularSpeed = 50.0f;
    public float turretSpeed = 50.0f;
    public float barrelSpeed = 5.0f;

    [Header("GameObject Data")]
    public TankData pawn;
    public Stats stats;
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
    public Transform tf;
    public Sight sight;
    DeathEvent deathEvent = new DeathEvent();

    void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //Prevent AIController from updating anything if the game is paused 
        if (Time.timeScale == 0) return;

        //If the pawn is null and respawning is allowed create a new one    
        if (pawn == null) return;
        if (pawn.isAlive == false) return;


        target = SetTarget();
        if (target == null) return;

        //Set all desired values
        sight = pawn.GetComponentInChildren<Sight>();
        tf = pawn.transform;
        pawn.stats = stats;
        currentHealth = maxHealth;
        pawn.turnSpeed = angularSpeed;
        pawn.forwardSpeed = forwardSpeed;

        MovementManager();

        //Show the values in the inspector
        sight.target = target;
        targetInSight = sight.TargetInSight();
        sight.viewDistance = sightDistance;
        castHit = (CastHit)CanMoveForward();
        sight.fieldOfView = fieldOfView;
        canHear = CanHear();
    }

    //Set the AI's new target if it destroys the current one
    Transform SetTarget()
    {
        if (GameManager.instance.splitScreen == true)
        {
            //If player1 dies player2 becomes the target
            if (GameManager.instance.player1.pawn.isAlive == false && GameManager.instance.player2.pawn != null)
                return GameManager.instance.player2.pawn.transform;

            //If player2 dies player1 becomes the target
            else if (GameManager.instance.player1.pawn != null && GameManager.instance.player2.pawn.isAlive == false)
                return GameManager.instance.player1.pawn.transform;

            //If both die do nothing
            else if (GameManager.instance.player1.pawn.isAlive == false && GameManager.instance.player2.pawn.isAlive == false) return null;

            //If neither are null calculate the target based on distance
            else
            {
                //If player 1 is closer than player2
                if (Vector3.Distance(tf.position, GameManager.instance.player1.pawn.transform.position) < Vector3.Distance(tf.position, GameManager.instance.player2.pawn.transform.position))
                    return GameManager.instance.player1.pawn.transform;
                else
                    return GameManager.instance.player2.pawn.transform;
            }
        }
        else
        {
            if (GameManager.instance.player1.pawn.isAlive == false) return null;
            return GameManager.instance.player1.pawn.transform;
        }
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
                RotateTurret();

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
            if (currentHealth <= healthToFlee && attackMode != AttackMode.Flee)
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
                //Debug.Log("lost track");
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

        if (currentPatrolType == PatrolType.Stop && currentWaypoint == waypoints.Length && Vector3.Distance(waypoints[currentWaypoint].position, pawn.motor.transform.position) >= waypointRadius)
        {
            MoveToTarget(waypoints[currentWaypoint].position);
            //Show the vector to the waypoint
            Debug.Log("currentWaypoint " + currentWaypoint);
            Debug.Log("Waypoint[] " + waypoints.Length);
            Debug.DrawLine(pawn.motor.transform.position, waypoints[currentWaypoint].position, Color.blue);

        }
        else if (currentPatrolType == PatrolType.Idle)
        {
            //Debug.Log("Idle");
            return;
        }
        else
        {
            //Debug.DrawLine(pawn.motor.transform.position, waypoints[currentWaypoint].position, Color.blue);
            MoveToTarget(waypoints[currentWaypoint].position);
        }

        //Move to the Waypoint
        //If the AI is not close enough to the Waypoint, return
        if (Vector3.Distance(waypoints[currentWaypoint].position, tf.position) > waypointRadius) return;

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

    //Makes the AI move towards the targets position
    //Simply here for clearer readability in MovementManager
    void Chase()
    {
        lastAttackMode = attackMode;
        if (Vector3.Distance(pawn.motor.transform.position, target.transform.position) > chaseDistance)
        {
            MoveToTarget(lastSeenLocation);
        }
        Debug.DrawLine(pawn.motor.transform.position, target.position, Color.white);
        //Debug.Log("Chase");
    }

    void RotateTurret()
    {
        //MoveTurret
        Vector3 turretVectorToTarget = target.position - pawn.weaponData.turret.transform.position;
        turretVectorToTarget.y = 0;
        Quaternion turretVectorToQuaternion = Quaternion.LookRotation(turretVectorToTarget);
        float step = turretSpeed * Time.deltaTime;
        pawn.weaponData.turret.transform.rotation = Quaternion.RotateTowards(pawn.weaponData.turret.transform.rotation, turretVectorToQuaternion, step);

        //Movebarrel
        //If the turret is rotated towards the target
        if (pawn.weaponData.turret.transform.rotation == turretVectorToQuaternion)
        {

            //Get the direction from the barrel to the target
            Vector3 targetDir = target.transform.position - pawn.weaponData.barrel.transform.position;
            //targetDir.y = barrel.transform.rotation.y;

            //Convert the vector to a quaternion rotation
            Quaternion rotation = Quaternion.LookRotation(targetDir);

            //Rotate the barrel to look at the target
            pawn.weaponData.barrel.transform.rotation = Quaternion.Lerp(pawn.weaponData.barrel.transform.rotation, rotation, step);

            Debug.DrawRay(pawn.weaponData.barrel.transform.position, targetDir, Color.blue);
        }
    }

    void Fire()
    {
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
        if (forwardLeftCast && !forwardRightCast)
            return (int)CastHit.Left;
        if (forwardRightCast && forwardRightCast)
            return (int)CastHit.Both;
        //Implicit else
        return (int)CastHit.None;

        //Optional: Prevent raycast from triggering on player
    }

    //Avoid obstacles and walls while moving to targetPosition
    void MoveToTarget(Vector3 targetPosition)
    {
        //If AI is close enough to its target
        if (Vector3.Distance(targetPosition, tf.position) <= waypointRadius) return;

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
            pawn.motor.Move(pawn.motor.transform.forward, pawn.forwardSpeed);

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
            //Debug.Log("Reset");
            timeSet = false;
            turningLeft = false;
            turningRight = false;
        }
    }
}
