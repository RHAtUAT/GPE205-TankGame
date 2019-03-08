using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSystem : MonoBehaviour {

    //Crosshair image
    //https://www.flaticon.com/free-icon/gun-pointer_18554

    public float mouseX;
    public float mouseY;
    public float sensitivityX = 5f;
    public float sensitivityY = 5f;
    public GameObject target;
    public GameObject turret;
    public Weapon weapon;

    private float rotationX = 0.0f;
    private float rotationY = 0.0f;
    private Transform cam;
    private Quaternion rot;
    private Vector3 DistanceToTarget;


    // Use this for initialization
    void Start () {
        cam = GetComponent<Transform>();
        DistanceToTarget = target.transform.position - cam.position;
        
        Vector3 rot = cam.localRotation.eulerAngles;
        rotationY = rot.y;
        rotationX = rot.x;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        //Setup the rotation of the mouse
        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");

        rotationY += mouseX * sensitivityY * Time.deltaTime;
        rotationX += mouseY * sensitivityX * Time.deltaTime;

        //Quaternion localRotation = Quaternion.Euler(rotationX, rotationY, 0.0f);
        //cam.rotation = localRotation;
    }

    // Update is called once per frame
    void LateUpdate () {

        //Get the Y rotation of the object
        float turretEulerAngleY = turret.transform.eulerAngles.y;
        
        Quaternion turretRotationY = Quaternion.Euler(0, turretEulerAngleY, 0);

        //Keep the camera the same distance away from the object when it moves
        // - (turretRotation * DistanceToTarget) makes the cameras rotation and
        //Distance stay the same in relation to the weapons own rotation and distance
        cam.position = target.transform.position - (turretRotationY * DistanceToTarget);

        //Rotate around the targets postion on the targets x-axis at the angle the target is facing
        cam.RotateAround(target.transform.position,  target.transform.right, target.transform.eulerAngles.x);
        cam.Rotate(turret.transform.rotation.eulerAngles.y, 0, 0);

        //Rotate the camera every frame so it keeps looking at the target
        cam.LookAt(target.transform);
    }
}