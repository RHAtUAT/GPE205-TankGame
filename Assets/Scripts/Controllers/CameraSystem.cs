using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSystem : MonoBehaviour {



    public GameObject target;
    public GameObject turret;
    public Weapon weapon;
    private Transform camera;
    private Quaternion rot;
    public float xOffset;
    public float yOffset;
    public float zOffset;
    public float turningSpeed = 5f;
    private float rotateXMin;
    private float rotateXMax;
    public float minYBarrelConstaint;
    Vector3 offset;

    // Use this for initialization
    void Start () {
        camera = GetComponent<Transform>();

        //Distance between the camera and the object its looking at
        offset = target.transform.position - camera.position;

    }
    //0, 1.2, -1.5S
    //20, 0, 0
	
	// Update is called once per frame
	void LateUpdate () {

      //  Debug.Log("MouseX: " + Input.mousePosition.x);
      //  Debug.Log("MouseY: " + Input.mousePosition.y);
      

        float vertical = Input.GetAxis("Mouse Y") * turningSpeed;

        //Get the y rotation of the object
        float desiredAngleY = turret.transform.eulerAngles.y;
        float desiredAngleX = weapon.transform.eulerAngles.x;
        
        Quaternion rotationY = Quaternion.Euler(0, desiredAngleY, 0);
        Quaternion rotationX = Quaternion.Euler(desiredAngleX, 0, 0);
        float offsetX = camera.transform.localRotation.x + weapon.transform.localRotation.x;
        Vector3 lookUp = weapon.transform.position + (rotationX * offset);

        //Keep the camera the same distance away from the object when it moves4
        camera.position = turret.transform.position - (rotationY * offset);

        //Rotate the camera every frame so it keeps looking at the target
        //camera.LookAt(weapon.transform);
        camera.transform.eulerAngles = target.transform.eulerAngles;
        //camera.LookAt(weapon.transform);
    }
}