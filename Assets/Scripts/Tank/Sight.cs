using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sight : MonoBehaviour
{
     public float fieldOfView;
     public float viewDistance;

    [HideInInspector] public Transform target;
    private Transform tf;

    // Start is called before the first frame update
    void Start()
    {
        //target = GameManager.instance.player.pawn.transform;
        tf = GetComponent<Transform>();
    }

    public bool TargetInSight()
    {

        //Convert float viewDistance to Vector3 viewDistance so its a point in world space
        Vector3 viewDist = tf.forward * viewDistance;

        //The Vector from this gameObject to the target
        Vector3 targetDir = target.position - tf.position;

        //Get the angle from the Y axis of the gameObject
        Quaternion quaternionLeft = Quaternion.AngleAxis(-fieldOfView / 2, transform.up);
        Quaternion quaternionRight = Quaternion.AngleAxis(fieldOfView / 2, transform.up);

        //Convert from an angle to a vector pointing from the gameObjects 
        //position to the viewing distance at the angle given
        Vector3 angleLeftToVector = quaternionLeft * viewDist;
        Vector3 angleRightToVector = quaternionRight * viewDist;

        //Draw the right line
        Vector3 rightEndPoint = tf.forward + angleRightToVector;
        Debug.DrawLine(tf.position, tf.position + rightEndPoint, Color.red);

        //Draw the left line
        Vector3 leftEndPoint = tf.forward + angleLeftToVector;
        Debug.DrawLine(tf.position, tf.position + leftEndPoint, Color.red);

        //Draw the viewDistance
        Debug.DrawLine(tf.position, tf.position + viewDist, Color.green);

        RaycastHit hit;
        bool cast = Physics.Raycast(tf.position, targetDir, out hit, viewDistance);
        if (Vector3.Angle(transform.forward, targetDir) < fieldOfView / 2f)
        {
            if (cast)
            {
                if (hit.transform == target)
                {
                    Debug.DrawRay(tf.position, targetDir, Color.cyan);
                    return true;
                }
            }
            return false;
        }
        return false;
    }
}
