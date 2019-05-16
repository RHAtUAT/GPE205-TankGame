using UnityEngine;

public class Test : MonoBehaviour
{

    public Transform target;
    public Transform barrel;
    public Transform barrelPivot;
    public float turretSpeed = 50f;
    public Vector3 positionOffset;
    public float maxVerticalRotation = 40;
    public float minVerticalRotation = -10;
    private float currentVerticalRotation;

    // Start is called before the first frame update
    void Start()
    {
        currentVerticalRotation = 0;
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    //TODO: Get this working
    //public void ClampBarrel(float inputY)
    //{
    //    //Sets the min and max angles the barrel can move up and down
    //    float newRot = Mathf.Clamp(totalVerticalRotation + inputY, -maxVerticalRotation, -minVerticalRotation);

    //    //Get new postition after the barrel has moved
    //    float delta = newRot - totalVerticalRotation;
    //    totalVerticalRotation = newRot;

    //    tf.Rotate(delta, 0, 0);
    //}

    // Update is called once per frame
    void Update()
    {

        //This will be transforms of course, but just for the example:
        Vector3 targetPosition = target.transform.position;
        Vector3 turretPosition = transform.position;

        //Move Turret
        Vector3 turretVectorToTarget = target.position - (transform.position - positionOffset);
        turretVectorToTarget.y = 0;
        Quaternion turretVectorToQuaternion = Quaternion.LookRotation(turretVectorToTarget);
        float step = turretSpeed * Time.deltaTime;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, turretVectorToQuaternion, step);

        //Move Barrel
        /*
         * Only rotate the barrel up and down if the turret is looking at the player,
         * even though the barrel can still rotate on all axis the y and z will already
         * be aligned as a result of the turrets rotatation
         * so the barrel will effectively only rotate on the x axis
         */
        if (transform.rotation == turretVectorToQuaternion)
        {
            //Get the direction from the barrel to the target
            Vector3 targetDir = target.transform.position - (barrel.transform.position - positionOffset);
            //targetDir.y = barrel.transform.rotation.y;

            //Convert the vector to a rotation in degrees
            Vector3 rotation = Quaternion.LookRotation(targetDir).eulerAngles;

            Debug.Log("BarrelX " + barrel.transform.localEulerAngles);
            Debug.Log("RotationX " + rotation.x);

            //Clamp rotation.x so that the barrel can only go so high and so low
            float newXRotation = Mathf.Clamp(rotation.x, maxVerticalRotation, minVerticalRotation);

            Debug.Log("newXRotation " + newXRotation);

            //Create a new Quaternion using the clamped rotation for the x value
            Quaternion newRotation = Quaternion.Euler(new Vector3(newXRotation, rotation.y, rotation.z));

            //Rotate the barrel to look at the target
            barrel.transform.rotation = Quaternion.Lerp(barrel.transform.rotation, newRotation, step);
            currentVerticalRotation = newXRotation;

            //Show the vector
            Debug.DrawRay(barrel.transform.position, targetDir, Color.blue);
        }

        ////Debug.Log("TurretVector(" + turretVectorToTarget.x + ", " + turretVectorToTarget.y + ", " + turretVectorToTarget.z + ")");
        //Debug.DrawRay(transform.position, turretVectorToTarget, Color.red);

        ////Move Barrel
        ////if (transform.rotation == turretVectorToQuaternion)
        ////{
        //    //Vector3 test = target.transform.position - new Vector3(transform.position.x, barrel.transform.position.y, transform.position.z);

        //    Vector3 barrelVectorToTarget = target.position - (barrel.transform.position - positionOffset);
        //    //Vector3 barrelVectorToTargetLevel = target.position - (barrel.transform.position - positionOffset);
        //    //barrelVectorToTargetLevel.y = 0;
        //   //float AngleIWantToRotateBy = Vector3.Angle(barrelVectorToTargetLevel, barrelVectorToTarget);

        //    //Debug.Log("Angle Size: " + AngleIWantToRotateBy);
        //    //Angle between the two
        //    //float sign = Mathf.Sign(barrelVectorToTarget.y - barrelVectorToTargetLevel.y);
        //    //float angleBetween = Vector3.Angle(barrelVectorToTarget, barrel.transform.forward) * sign;

        //    //barrel.transform.rotation = Quaternion.Euler(angleBetween, 0, 0);

        //    //Setting these to zero will result in the barrel pointing directly up or down
        //    //barrelVectorToTarget.x = 0;
        //    //barrelVectorToTarget.z = 0;

        //    //Get the rotation that matches the vector
        //    Quaternion barrelVectorToQuaternion = Quaternion.LookRotation(barrelVectorToTarget);
        //Vector3 eulerAngles = Quaternion.LookRotation(barrelVectorToTarget).eulerAngles;

        //barrel.transform.rotation = Quaternion.Euler(eulerAngles.x + 90, 0, 0);
        ////barrel.localEulerAngles = new Vector3(barrel.localEulerAngles.x, 0, 0);

        ////barrel.transform.rotation = Quaternion.Euler(angleBetween, 0, 0);//
        ////barrel.transform.rotation = Quaternion.RotateTowards(barrel.transform.rotation, barrelVectorToQuaternion, step);
        //Debug.Log("BarrelVector(" + barrelVectorToTarget.x + ", " + barrelVectorToTarget.y + ", " + barrelVectorToTarget.z + ")");
        //Debug.Log("BarrelVectorQuaternion(" + barrelVectorToQuaternion.x + ", " + barrelVectorToQuaternion.y + ", " + barrelVectorToQuaternion.z + ", " + barrelVectorToQuaternion.w + ")");

        //Debug.DrawRay(barrel.transform.position, barrelVectorToTarget, Color.blue);
        //    //Debug.DrawRay(barrel.transform.position, barrelVectorToTargetLevel, Color.cyan);
        ////}


    }
}
