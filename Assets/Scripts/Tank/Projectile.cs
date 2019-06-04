using UnityEngine;

public class Projectile : MonoBehaviour
{

    [HideInInspector] public Transform tf;
    [HideInInspector] public Collider cd;
    [HideInInspector] public Rigidbody rb;
    [HideInInspector] public float despawnTime;
    [HideInInspector] public float velocity;
    [HideInInspector] public int damage;
    public TankData owner;
    public ParticleSystem impactEffectPrefab;

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
        Instantiate(impactEffectPrefab, tf.position, Quaternion.Euler(tf.rotation.eulerAngles));
        gameObject.SetActive(false);

        //If the gameObject the projectile hits can take damage
        Damagable damagable = other.gameObject.GetComponentInParent<Damagable>() == null ? other.gameObject.GetComponent<Damagable>() : other.gameObject.GetComponentInParent<Damagable>();

        //If the gameObject dies add to the players score
        if (damagable != null)
        {
            //Debug.Log("Damage: " + damagable.Damage(damage));
            int[] stats = damagable.Damage(damage);
            owner.stats.kills += stats[0];
            owner.stats.score += stats[1];

        }
    }
}

interface Damagable
{
    int[] Damage(int amount);
}
