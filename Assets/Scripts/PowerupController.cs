using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupController : MonoBehaviour
{
    public TankData tankData;
    public List<Powerup> powerups;

    void Start()
    {
        tankData = GetComponent<TankData>();
        //powerups = new List<Powerup>();
    }

    void Update()
    {
        // Create a List to hold our expired powerups
        List<Powerup> expiredPowerups = new List<Powerup>();

        // Loop through all the powers in the List
        foreach (Powerup power in powerups)
        {
            // Subtract from the timer
            power.duration -= Time.deltaTime;

            // Assemble a list of expired powerups
            if (power.duration <= 0)
            {
                expiredPowerups.Add(power);
            }
        }
        // Now that we've looked at every powerup in our list, use our list of expired powerups to remove the expired ones.
        foreach (Powerup power in expiredPowerups)
        {
            power.Deactivate(tankData);
            powerups.Remove(power);
        }
        // Since our expiredPowerups is local, it will *poof* into nothing when this function ends, 
        // but let's clear it to learn how to empty an List
        expiredPowerups.Clear();
    }

    public void Add(Powerup powerup)
    {
        // Run the Activate function of the powerup
        powerup.Activate(tankData);

        // Only add the non permanent ones to the list
        if (powerup.isPermanent)
        {
            powerups.Add(powerup);
        }
    }
}
