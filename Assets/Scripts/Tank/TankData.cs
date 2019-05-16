using UnityEngine;

//[RequireComponent(typeof(TankMotor))]
//[RequireComponent(typeof(WeaponData))]

//TODO: Fix the way the tank is killed

public class TankData : MonoBehaviour, Damagable
{

    //Vehicle properties
    public int maxHealth;
    public int currentHealth;
    public int damageDone;
    [HideInInspector] public int score = 0;
    [HideInInspector] public int kills = 0;
    [HideInInspector] public int deaths = 0;

    [Header("Vehicle Properties")]
    public float forwardSpeed = 25.0f;
    public float backwardSpeed = 20.0f;
    public float turnSpeed = 50.0f;
    public float baseForwardSpeed { get; private set; }
    public float baseBackwardSpeed { get; private set; }
    public float baseTurnSpeed { get; private set; }

    public Transform centerOfGravity;
    //[HideInInspector] public Controller controller;
    [HideInInspector] public Stats stats;
    [HideInInspector] public TankMotor motor;
    [HideInInspector] public Transform motorTf;
    [HideInInspector] public WeaponData weaponData;
    [HideInInspector] public Pivot pivot;
    [HideInInspector] public TankRenderer tankRenderer;
    [HideInInspector] public PowerupController powerupController;

    private Rigidbody rb;

    void Awake()
    {
        baseForwardSpeed = forwardSpeed;
        baseBackwardSpeed = backwardSpeed;
        baseTurnSpeed = turnSpeed;

    }

    // Use this for initialization
    void Start()
    {
        currentHealth = maxHealth;
        powerupController = GetComponent<PowerupController>();
        tankRenderer = GetComponent<TankRenderer>();
        rb = GetComponent<Rigidbody>();
        pivot = GetComponentInChildren<Pivot>();
        motor = GetComponentInChildren<TankMotor>();
        weaponData = GetComponent<WeaponData>();
        motorTf = this.gameObject.transform.GetChild(0).GetChild(0).transform;
        rb.centerOfMass = centerOfGravity.localPosition;
    }


    //Return a kill and 100 score to the tank that killed this one
    public int[] Damage(int amount)
    {
        if (powerupController.activePowerups[PickupType.Shield] == null)
        {
            currentHealth -= amount;

            if (currentHealth <= 0)
            {
                //Lose a life if they are limited
                if (GameManager.instance.limitedLives == true)
                    stats.lives -= 1;

                stats.deaths += 1;
                SpawnManager.instance.activeTanks.Remove(this);
                powerupController.enabled = false;
                gameObject.SetActive(false);
                Destroy(gameObject);

                //Return 1 kill and 100 score if this tank is dead
                return new int[] { 1, 100 };
            }
        }
        else
            powerupController.shieldPowerup.currentHealth -= amount;

        //Return 0 kills and 0 score if this tank isnt dead
        return new int[] { 0, 0 };
    }
}