using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSystem : MonoBehaviour {



    public float mouseX;
    public float mouseY;
    public float smoothX;
    public float smoothY;
    public float sensitivityX = 5f;
    public float sensitivityY = 5f;
    public float clampAngle;
    public GameObject target;
    public GameObject turret;
    public Weapon weapon;

    private float rotationX = 0.0f;
    private float rotationY = 0.0f;
    private float rotateXMin;
    private float rotateXMax;
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
        //Setup rotation of sticks
        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");

        rotationY += mouseX * sensitivityY * Time.deltaTime;
        rotationX += mouseY * sensitivityX * Time.deltaTime;

        rotationX = Mathf.Clamp(rotationX, -clampAngle, clampAngle);

        Quaternion localRotation = Quaternion.Euler(rotationX, rotationY, 0.0f);
        cam.rotation = localRotation;
    }

    // Update is called once per frame
    void LateUpdate () {

        //Get the Y rotation of the object
        //Get the X rotation of the object
        float turretEulerAngleY = turret.transform.eulerAngles.y;
        float turretEulerAngleX = turret.transform.eulerAngles.x;
        float weaponEulerAngleX = weapon.transform.eulerAngles.x;
        
        Quaternion turretRotationY = Quaternion.Euler(0, turretEulerAngleY, 0);
        Quaternion turretRotationX = Quaternion.Euler(turretEulerAngleX, 0, 0);

        //Keep the camera the same distance away from the object when it moves
        // - (turretRotation * DistanceToTarget) makes the cameras rotation and
        //Distance stay the same in relation to the weapons own rotation and distance
        cam.position = weapon.transform.position - (turretRotationY * DistanceToTarget);
        cam.Rotate(weaponEulerAngleX, turretEulerAngleY, 0);

        //Rotate the camera every frame so it keeps looking at the target
        cam.eulerAngles = target.transform.eulerAngles;
        //camera.LookAt(weapon.transform);
    }
}