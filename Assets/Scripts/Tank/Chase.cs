using UnityEngine;

public class Chase : MonoBehaviour
{

    public enum AttackMode { Chase, Flee };
    public AttackMode attackMode;
    public bool canMove;
    public float fleeDistance = 1.0f;
    public float avoidanceTime = 2.0f;
    public Transform target;

    private int avoidanceStage = 0;
    private float exitTime;
    private Transform tf;
    private TankData tank;
    private TankMotor motor;


    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.instance.splitScreen == true)
        {
            //If player 1 is closer than player2
            if (Vector3.Distance(tf.position, GameManager.instance.player1.pawn.transform.position) < Vector3.Distance(tf.position, GameManager.instance.player2.pawn.transform.position))
                target = GameManager.instance.player1.pawn.transform;
            else
                target = GameManager.instance.player2.pawn.transform;
        }
        else
            target = GameManager.instance.player1.pawn.transform;

        tank = GetComponent<TankData>();
        motor = GetComponentInChildren<TankMotor>();
        tf = motor.transform;
    }

    // Update is called once per frame
    void Update()
    {

        target = SetTarget();
        if (target == null) return;

        canMove = CanMove(tank.forwardSpeed);
        //Attack
        if (attackMode == AttackMode.Chase)
        {
            if (avoidanceStage != 0)
            {
                Avoid();
            }
            else
            {
                DoChase();
            }
        }

        //Flee
        if (attackMode == AttackMode.Flee)
        {
            if (avoidanceStage != 0)
            {
                Avoid();
            }
            else
            {
                Flee();
            }
        }
    }

    Transform SetTarget()
    {
        //Set the AI's new target if it destroys the current one
        if (GameManager.instance.splitScreen == true)
        {
            //If player1 dies player2 becomes the target
            if (GameManager.instance.player1.pawn == null && GameManager.instance.player2.pawn != null)
                return GameManager.instance.player2.pawn.transform;

            //If player2 dies player1 becomes the target
            else if (GameManager.instance.player1.pawn != null && GameManager.instance.player1.pawn == null)
                return GameManager.instance.player1.pawn.transform;

            //If both die do nothing
            else if (GameManager.instance.player1.pawn == null && GameManager.instance.player2.pawn == null) return null;

            //If niether are null calculate the target based on distance
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
            if (GameManager.instance.player1.pawn == null) return null;
            return GameManager.instance.player1.pawn.transform;
        }
    }

    bool CanMove(float speed)
    {

        //If there is something in front of us
        RaycastHit hit;
        if (Physics.Raycast(tf.position, tf.forward, out hit, speed))
        {
            Debug.DrawRay(tf.position, tf.forward, Color.black);
            //And it isn't the player
            if (!hit.collider.CompareTag("Player"))
            {
                //We can't move
                return false;
            }
        }
        return true;
    }

    void Flee()
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
        motor.RotateTowards(fleePositon);
        motor.Move(tank.transform.forward, tank.forwardSpeed);
    }

    void DoChase()
    {
        //Rotate towards our target
        motor.RotateTowards(target.position);

        //Check if we can move tank.moveSpeed units away
        //We chose this distance, because that is how far we move in 1 second,
        //This means, we are looking for collisions "one second in the future."

        if (CanMove(tank.forwardSpeed))
        {
            //Move forward
            motor.Move(tank.transform.forward, tank.forwardSpeed);
        }
        else
        {
            //Enter obstacle avoidance stage 1
            avoidanceStage = 1;
        }
    }

    //Avoids obstacles
    void Avoid()
    {
        if (avoidanceStage == 1)
        {
            //Rotate Left
            motor.Rotate(-1 * tank.turnSpeed);

            //If this gameObject can move forward move to stage 2
            if (CanMove(tank.forwardSpeed))
            {
                avoidanceStage = 2;

                //Set the number of seconds we will stay in stage 2
                exitTime = avoidanceTime;
            }

            //Otherwise, we'll do this again next turn
        }
        else if (avoidanceStage == 2)
        {
            //If we can move forward, do it
            if (CanMove(tank.forwardSpeed))
            {
                //Subtract from our time and move
                exitTime -= Time.deltaTime;
                motor.Move(tank.transform.forward, tank.forwardSpeed);

                if (exitTime <= 0)
                {
                    avoidanceStage = 0;
                }
            }
            else
            {
                //The gameObject cant move forward so go back to stage 1
                avoidanceStage = 1;
            }
        }
    }
}
