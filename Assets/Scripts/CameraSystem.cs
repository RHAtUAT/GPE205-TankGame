using UnityEngine;

public class CameraSystem : MonoBehaviour
{

    //TODO: Add First person camera

    //Crosshair image
    //https://www.flaticon.com/free-icon/gun-pointer_18554

    public enum CameraType { ThirdPerson, FirstPerson };
    public CameraType cameraType;
    public float mouseX;
    public float mouseY;
    public float sensitivityX = 1f;
    public float sensitivityY = 1f;
    public float smoothSpeed = 0.125f;
    public Player player;
    public Transform target;
    public Vector3 rotOffset;
    public Vector3 offset;

    //private float rotationX = 0.0f;
    //private float rotationY = 0.0f;
    private Transform cam;
    //private Quaternion rot;
    public Vector3 distanceToTarget;
    private Vector3 vectorForward;
    private Vector3 vectorToTurret;


    // Use this for initialization
    void Start()
    {
        distanceToTarget = new Vector3(0, -1.925f, 5.2f);
        //target = InputController.instance.pawn.pivot.transform;
        cam = GetComponent<Transform>();
        Vector3 rot = cam.localRotation.eulerAngles;
        //rotationY = rot.y;
        //rotationX = rot.x;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (player.pawn == null) return;
        else
        {
            if (target == null)
                target = player.pawn.pivot.transform;
        }

        ThirdPerson();
    }

    void FirstPerson()
    {

    }

    void ThirdPerson()
    {

        //Get the Y rotation of the object
        Quaternion targetRotationY = Quaternion.Euler(0, target.eulerAngles.y, 0);

        //Keep the camera the same distance away from the object when it moves
        // - (turretRotation * DistanceToTarget) makes the cameras rotation and
        //Distance stay the same in relation to the weapons own rotation and distance
        cam.position = target.position - (targetRotationY * distanceToTarget);

        //Rotate around the targets postion on the targets x-axis at the angle the target is facing
        cam.RotateAround(target.position, target.right, target.eulerAngles.x);
        //cam.Rotate(turret.transform.rotation.eulerAngles.y, 0, 0);

        //Rotate the camera every frame so it keeps looking at the target
        cam.LookAt(target);
    }

    //Testing for different camera styles
    void ThirdPerson2()
    {
        //vectorForward = target.forward;

        ////Make the cameras position at a vector behind the target and add desired offsets;
        //cam.position = (target.position - target.forward) + offset;

        ////Make the cameras rotation the same as the targets
        //cam.rotation = Quaternion.Euler(target.rotation.eulerAngles.x, target.rotation.eulerAngles.y, 0);


        //Debug.DrawRay(target.position, target.forward * offset.z, Color.green);

        //----------------------------------------------------------

    }
}