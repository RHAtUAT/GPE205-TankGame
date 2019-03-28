using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public PickupType pickupType;
    public Powerup powerup;
    //public AudioClip pickupSound;
    private Transform tf;

    // Use this for initialization
    void Start()
    {
        tf = GetComponent<Transform>();
    }

    void ChangePowerupVariables()
    {
    }

    public void OnTriggerEnter(Collider other)
    {
        PowerupController powerupController = other.GetComponentInParent<PowerupController>();

        if (powerupController != null)
        {
            powerupController.Add(powerup);
            //AudioSource.PlayClipAtPoint(pickupSound, tf.position, 1.0f);
            Destroy(gameObject);
        }
    }
}


public enum PickupType { Speed, FireRate, Health, Shield }