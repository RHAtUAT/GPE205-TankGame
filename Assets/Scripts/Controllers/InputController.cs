using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{

    public TankData pawn;
	// Use this for initialization
	void Start()
	{
		GameManager.instance.player = this.gameObject;
	}
 
	// Update is called once per frame
	void Update()
    {
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
    }

    private void FixedUpdate()
    {
                //Shoot
        if (Input.GetKey(KeyCode.Space))
        {
            pawn.weaponData.Fire();
        }
    }

}
