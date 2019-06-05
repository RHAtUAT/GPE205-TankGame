using UnityEngine;

public class TankMotor : MonoBehaviour
{

    private CharacterController cc;
    private Rigidbody rb;
    [HideInInspector] public Transform tf;
    private TankData tank;

    void Awake()
    {
        tank = GetComponentInParent<TankData>();
        tf = GetComponent<Transform>();
        //cc = GetComponentInParent<CharacterController>();
        rb = GetComponentInParent<Rigidbody>();

    }

    // Use this for initialization
    void Start()
    {

    }

    public void Move(Vector3 direction, float speed)
    {
        Vector3 xzOnly = new Vector3(direction.x, 0, direction.z);
        rb.AddForce(xzOnly * speed, ForceMode.Force);
        //SimpleMove() will apply Time.deltaTime, and convert to meters per second for us!
        //cc.SimpleMove(direction *= tank.moveSpeed); 
    }

    public void Rotate(float speed)
    {
        // Vector3.up is the axis to rotate around
        Vector3 rotateVector = Vector3.up;

        //Rotate by turnSpeed degrees
        rotateVector *= speed;

        // Transform.Rotate() doesn't account for speed, so we need to change our rotation to "per second" instead of "per frame."
        rotateVector *= Time.deltaTime;

        tf.Rotate(rotateVector);
    }

    //Needs reworking(jittery)
    public bool RotateTowards(Vector3 targetPosition)
    {
        Vector3 xzOnly = new Vector3(targetPosition.x, 0, targetPosition.z);

        //The difference/distance between this gameObjects postion and the targets position
        Vector3 vectorToTarget = xzOnly - new Vector3(tf.position.x, 0, tf.position.z);

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
