using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public Powerup powerup;
    //public AudioClip pickupSound;
    private Transform tf;
    public PickupType pickupType;

    // Use this for initialization
    void Start()
    {
        tf = GetComponent<Transform>();
    }

    public void OnTriggerEnter(Collider other)
    {
        PowerupController powerupController = other.GetComponentInParent<PowerupController>();

        if (powerupController != null)
        {
            powerupController.Add(powerup);
            Destroy(gameObject);
        }
    }

}

public enum PickupType { Speed, FireRate, Health, Shield }