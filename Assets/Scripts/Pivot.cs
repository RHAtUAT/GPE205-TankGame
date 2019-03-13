using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pivot : MonoBehaviour
{
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
    }

    void Update()
    {

    }

    public void RotateTurret(float inputX, float inputY)
    {



        turret.transform.Rotate(0, inputX, 0);

        //Sets the min and max angles the weapon can move up and down
        float newRot = Mathf.Clamp(totalVerticalRotation + inputY, -maxVerticalRotation, -minVerticalRotation);

        //Get new postition after the weapon has moved
        float delta = newRot - totalVerticalRotation;
        totalVerticalRotation = newRot;

        tf.Rotate(delta, 0, 0);
    }
}
