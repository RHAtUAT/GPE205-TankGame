using System.Collections;
using System.Collections.Generic;
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
    private Health health;
    private Health healthInParent;
    public ParticleSystem impactEffectPrefab;

    // Use this for initialization
    private void Start()
    {
        tf = GetComponent<Transform>();
        cd = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
        rb.AddForce(tf.up * velocity, ForceMode.Impulse);
        Destroy(this.gameObject, despawnTime);
        //Debug.Log("I'm alive");
        //Debug.Log("Projectile GameObject: " + this.gameObject);
        //Debug.Log("Projectile Postion: " + tf.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        health = other.gameObject.GetComponent<Health>();
        healthInParent = other.gameObject.GetComponentInParent<Health>();

        //If the gameObject the projectile hits can take damage
        if (health != null) {
            health.currentHealth -= damage;
            Debug.Log("Damaged");
            Destroy(this.gameObject);
        }

        if (healthInParent != null)
        {
            healthInParent.currentHealth -= damage;
            Debug.Log("Damaged");
            Destroy(this.gameObject);
        }
        if (other.gameObject.GetComponent<Projectile>())
            //Debug.Log("Projectile Hit: " + this.gameObject);
            return;
        else
        {
            Instantiate<ParticleSystem>(impactEffectPrefab, tf.position, Quaternion.Euler(tf.rotation.eulerAngles));
            Destroy(this.gameObject);
        }
    }
}
