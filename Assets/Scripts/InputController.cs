using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{

    public float mouseX;
    public float mouseY;
    public float sensitivityX = 1f;
    public float sensitivityY = 1f;
    private float rotationX;
    private float rotationY;


    public TankData pawn;

    // Use this for initialization
    void Start()
	{
		GameManager.instance.player = this;
        Debug.Log("Player: " + GameManager.instance.player.pawn);
        Debug.Log("Motor: " + pawn.motor);
        
    }
 
	// Update is called once per frame
	void Update()
    {
        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");

        rotationX = mouseX * sensitivityX;
        rotationY = mouseY * sensitivityY;

        //Move Forwards
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            pawn.motor.Move(pawn.moveSpeed);
        }
        //Move Backwards
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            pawn.motor.Move(-pawn.moveSpeed);
            //pawn.mover.Move(-pawn.mover.mTransform.forward);
        }
        //Turn Right
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            pawn.motor.Rotate(pawn.turnSpeed);
        }
        //Turn Left
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            pawn.motor.Rotate(-pawn.turnSpeed);
        }
        //Shoot on left mouse click
        if (Input.GetMouseButtonDown(0))
        {
            pawn.weaponData.Fire();
        }

        //Move the turret and barrel by the mouse postion
        pawn.pivot.RotateTurret(rotationX, rotationY);
    }

    private void FixedUpdate()
    {

    }

}
