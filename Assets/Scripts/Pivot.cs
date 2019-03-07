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
    public float mouseX;
    public float mouseY;
    public float sensitivityX = 1f;
    public float sensitivityY = 1f;
    private float rotationX = 0.0f;
    private float rotationY = 0.0f;
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
        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");

        rotationY += mouseX * sensitivityY;
        rotationX += mouseY * sensitivityX;



        Debug.Log("localRotation" + turret.transform.localRotation);
        Debug.Log("Rotation" + turret.transform.rotation);
        Debug.Log("ParentRotation" + transform.parent.rotation);
        turret.transform.Rotate(0, mouseX, 0);

        //Sets the min and max angles the barrel can move up and down
        float newRot = Mathf.Clamp(totalVerticalRotation + mouseY, -maxVerticalRotation, -minVerticalRotation);

        //Get new postition after the barrel has moved
        float delta = newRot - totalVerticalRotation;
        totalVerticalRotation = newRot;

        tf.Rotate(delta, 0, 0);
    }
}
