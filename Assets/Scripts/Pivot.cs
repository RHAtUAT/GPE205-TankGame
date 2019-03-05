using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pivot : MonoBehaviour
{
    public float turningSpeed;
    public GameObject turret;
    private Transform tf;
    public Quaternion startRotation;
    public float maxVerticalRotation = 40;
    public float minVerticalRotation = -10;
    private float totalVerticalRotation;
    Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        totalVerticalRotation = 0;
        tf = GetComponent<Transform>();
        //offset = turret.transform.position - camera.position;

    }

    void Update()
    {
        float horizontal = Input.GetAxis("Mouse X") * turningSpeed;
        float vertical = Input.GetAxis("Mouse Y") * turningSpeed;

        //Get the y rotation of the object
        float desiredAngle = turret.transform.eulerAngles.y;
        Quaternion rotation = Quaternion.Euler(0, desiredAngle, 0);

        turret.transform.Rotate(0, horizontal, 0);

        //Sets the min and max angles the barrel can move up and down
        float newRot = Mathf.Clamp(totalVerticalRotation + vertical, -maxVerticalRotation, -minVerticalRotation);

        //Get new postition after the barrel has moved
        float delta = newRot - totalVerticalRotation;
        totalVerticalRotation = newRot;

        tf.Rotate(delta, 0, 0);
    }
}
