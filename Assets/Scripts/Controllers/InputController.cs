using UnityEngine;
using Logger = Assets.Scripts.Utilities.Logger;

public class InputController : MonoBehaviour
{
    private static readonly Logger Logger = new Logger("InputController");

    public enum Player { _1, _2 }
    public enum InputType { Keyboard, GamePad }
    public InputType inputType;
    public Player player;
    [HideInInspector] public bool firstSpawn = true;
    [Header("Mouse Information")]
    public float mouseX;
    public float mouseY;
    public float sensitivityX = 1f;
    public float sensitivityY = 1f;
    public float rotationX;
    public float rotationY;
    public TankData pawn;

    void Start()
    {

    }


    // Update is called once per frame
    void Update()
    {
        PlayerInput();
    }

    void FixedUpdate()
    {

        if (pawn.isAlive == false)
        {
            return;
        }
        //Move Forwards
        if (Input.GetKey(KeyCode.W))
        {
            pawn.motor.Move(pawn.motorTf.forward, pawn.forwardSpeed);
            //AudioManager.instance.Play("tankMotor");
        }

        //Move Backwards
        if (Input.GetKey(KeyCode.S))
        {
            pawn.motor.Move(-pawn.motorTf.forward, pawn.backwardSpeed);
            //AudioManager.instance.Play("tankMotor");
        }
    }

    void PlayerInput()
    {
        if (pawn == null) return;

        if (pawn.isAlive == false) return;

        if (inputType == InputType.Keyboard)
        {

            mouseX = Input.GetAxis("Mouse X");
            mouseY = Input.GetAxis("Mouse Y");

            rotationX = mouseX * sensitivityX;
            rotationY = mouseY * sensitivityY;

            //Open pause menu
            if (Input.GetKeyDown(KeyCode.Escape))
                OptionsMenu();

            //Disable player input if the game is paused
            if (Time.timeScale == 0) return;

            //Show Game stats
            if (Input.GetKeyDown(KeyCode.Tab))
                StatsMenu();

            //Turn Right
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
                pawn.motor.Rotate(pawn.turnSpeed);

            //Turn Left
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                pawn.motor.Rotate(-pawn.turnSpeed);
            }



            //Shoot on left mouse click
            if (Input.GetMouseButtonDown(0))
                pawn.weaponData.Fire();

            //Move the turret and barrel by the mouse postion
            pawn.pivot.Barrel(rotationY);

            pawn.weaponData.turret.transform.Rotate(0, rotationX, 0);
        }

        if (inputType == InputType.GamePad)
        {

            mouseY = Input.GetAxis("GamePad" + (int)player + " Right Joystick Y");
            mouseX = Input.GetAxis("GamePad" + (int)player + " Right Joystick X");

            rotationX = mouseX * sensitivityX;
            rotationY = mouseY * sensitivityY;

            //Open pause menu
            if (Input.GetButtonUp("GamePad" + (int)player + " StartButton"))
                OptionsMenu();

            //Disable player input if the game is paused
            if (Time.timeScale == 0) return;

            //Show Game stats
            if (Input.GetButtonUp("GamePad" + (int)player + " BackButton"))
                StatsMenu();

            //Turn Right
            if (Input.GetAxis("GamePad" + (int)player + " Left Joystick X") > 0)
                pawn.motor.Rotate(pawn.turnSpeed);

            //Turn Left
            if (Input.GetAxis("GamePad" + (int)player + " Left Joystick X") < 0)
                pawn.motor.Rotate(-pawn.turnSpeed);

            //Move Forwards
            if (Input.GetAxis("GamePad" + (int)player + " Left Joystick Y") < 0)
            {
                pawn.motor.Move(pawn.motorTf.forward, pawn.forwardSpeed);
                //AudioManager.instance.Play("tankMotor");
            }

            //Move Backwards
            if (Input.GetAxis("GamePad" + (int)player + " Left Joystick Y") > 0)
            {
                pawn.motor.Move(-pawn.motorTf.forward, pawn.backwardSpeed);
                //AudioManager.instance.Play("tankMotor");
            }

            //Shoot on Right Trigger
            if (Input.GetAxis("GamePad" + (int)player + " RT") == 1)
                pawn.weaponData.Fire();

            //Move the turret and barrel by the Right Joystick
            Logger.Debug(pawn.pivot.ToString());

            pawn.pivot.Barrel(rotationY);
            pawn.weaponData.turret.transform.Rotate(0, rotationX, 0);
        }
    }

    void OptionsMenu()
    {
        if (player == Player._1)
        {

            if (UIManager.instance.optionsMenu1.gameObject.activeSelf == false)
            {
                UIManager.instance.optionsMenu1.gameObject.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                Time.timeScale = 0;
            }
            else
            {
                UIManager.instance.optionsMenu1.gameObject.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                if (UIManager.instance.optionsMenu2.gameObject.activeSelf == false && UIManager.instance.optionsMenu1.gameObject.activeSelf == false)
                    Time.timeScale = 1;
            }
        }
        else
        {

            if (UIManager.instance.optionsMenu2.gameObject.activeSelf == false)
            {
                UIManager.instance.optionsMenu2.gameObject.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                Time.timeScale = 0;

            }
            else
            {
                UIManager.instance.optionsMenu2.gameObject.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                Time.timeScale = 1;
                if (UIManager.instance.optionsMenu2.gameObject.activeSelf == false && UIManager.instance.optionsMenu1.gameObject.activeSelf == false)
                    Time.timeScale = 1;
            }
        }
    }

    void StatsMenu()
    {
        if (player == Player._1)
        {
            if (UIManager.instance.scoreUI1.gameObject == null) return;

            if (UIManager.instance.scoreUI1.gameObject.activeSelf == false)
                UIManager.instance.scoreUI1.gameObject.SetActive(true);

            else
                UIManager.instance.scoreUI1.gameObject.SetActive(false);
        }
        else
        {
            if (UIManager.instance.scoreUI2.gameObject == null) return;

            if (UIManager.instance.scoreUI2.gameObject.activeSelf == false)
                UIManager.instance.scoreUI2.gameObject.SetActive(true);

            else
                UIManager.instance.scoreUI2.gameObject.SetActive(false);
        }
    }
}