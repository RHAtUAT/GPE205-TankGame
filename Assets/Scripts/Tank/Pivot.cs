using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pivot : MonoBehaviour
{
    private Transform tf;
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

    public void Barrel(float inputY)
    {
        //Sets the min and max angles the barrel can move up and down
        float newRot = Mathf.Clamp(totalVerticalRotation + inputY, -maxVerticalRotation, -minVerticalRotation);

        //Get new postition after the barrel has moved
        float delta = newRot - totalVerticalRotation;

        //What the rotation will be after it gets rotated by the function below
        totalVerticalRotation = newRot;

        tf.Rotate(delta, 0, 0);
    }
}
