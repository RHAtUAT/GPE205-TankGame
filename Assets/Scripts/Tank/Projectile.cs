using UnityEngine;

public class Projectile : MonoBehaviour
{

    [HideInInspector] public Transform tf;
    [HideInInspector] public Collider cd;
    [HideInInspector] public Rigidbody rb;
    [HideInInspector] public float despawnTime;
    [HideInInspector] public float velocity;
    [HideInInspector] public int damage;
    [HideInInspector] public GameObject owner;
    public ParticleSystem impactEffectPrefab;
    private Health health;
    private PowerupController powerupController;

    // Use this for initialization
    private void Start()
    {
        tf = GetComponent<Transform>();
        cd = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
        rb.AddForce(tf.up * velocity, ForceMode.Impulse);
        Destroy(this.gameObject, despawnTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        //If the gameObject the projectile hits can take damage
        Damagable damagable = other.gameObject.GetComponentInParent<Damagable>() == null ? other.gameObject.GetComponentInParent<Damagable>() : other.gameObject.GetComponent<Damagable>();
        damagable.Damage(damage);
    }
}

interface Damagable
{
    void Damage(int amount);
}