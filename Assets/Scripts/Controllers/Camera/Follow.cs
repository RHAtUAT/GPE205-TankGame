using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{

    public float velocity = 100.0f;
    public float inputSensitivity = 150.0f;
    public float clampAngle = 80.0f;
    public float camDistanceXToPlayer;
    public float camDistanceYToPlayer;
    public float camDistanceZTPlayer;
    public float mouseX;
    public float mouseY;
    public float smoothX;
    public float smoothY;
    private float rotationY = 0.0f;
    private float rotationX = 0.0f;
    Vector3 cameraPosition;
    public GameObject target;
    public GameObject player;
    public GameObject camera;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 rot = transform.localRotation.eulerAngles;
        rotationY = rot.y;
        rotationX = rot.x;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }

    // Update is called once per frame
    void Update()
    {

        //Setup rotation of sticks
        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");

        rotationY += mouseX * inputSensitivity * Time.deltaTime;
        rotationX += mouseY * inputSensitivity * Time.deltaTime;

        rotationX = Mathf.Clamp(rotationX, -clampAngle, clampAngle);

        Quaternion localRotation = Quaternion.Euler(rotationX, rotationY, 0.0f);
        transform.rotation = localRotation;

    }

    private void LateUpdate()
    {
        CameraUpdate();

    }

    void CameraUpdate()
    {
        //Set the object to follow
        Transform targetTf = target.transform;

        //Move towards the target
        float step = velocity * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, targetTf.position, step);
    }

}
