using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupController : MonoBehaviour
{
    //TODO: Add logwarning if no wheels
    public int shieldHealth;
    public int maxHealth;
    public List<Powerup> powerups;
    public Dictionary<PickupType, Powerup> activePowerups = new Dictionary<PickupType, Powerup>();

    private TankRenderer tankRenderer;
    private TankData tankData;
    private List<Renderer> shieldRenderers;
    private GameObject meshObject;
    private Health health;
    private List<GameObject> shieldObjects = new List<GameObject>();

    void Start()
    {
        health = GetComponent<Health>();
        tankData = GetComponent<TankData>();
        tankRenderer = GetComponent<TankRenderer>();
        activePowerups.Add(PickupType.FireRate, null);
        activePowerups.Add(PickupType.Health, null);
        activePowerups.Add(PickupType.Shield, null);
        activePowerups.Add(PickupType.Speed, null);
    }

    void Update()
    {
        // Create a List to hold our expired powerups
        List<Powerup> expiredPowerups = new List<Powerup>();

        // Loop through all the powers in the List
        foreach (Powerup powerup in powerups)
        {
            // Subtract from the timer
            powerup.duration -= Time.deltaTime;

            // Assemble a list of expired powerups
            if (powerup.duration <= 0)
            {
                expiredPowerups.Add(powerup);
            }
        }
        // Now that we've looked at every powerup in our list, use our list of expired powerups to remove the expired ones.
        foreach (Powerup powerup in expiredPowerups)
        {
            powerup.Deactivate(tankData);
            if (powerup.visualModifier)
            {
                RestoreMaterial(tankRenderer, powerup);
            }

            powerups.Remove(powerup);
        }
        // Since our expiredPowerups is local, it will *poof* into nothing when this function ends, 
        // but let's clear it to learn how to empty an List
        expiredPowerups.Clear();
    }
   
    public void Add(Powerup powerup)
    {
        // Prevent powerups from stacking
        if (activePowerups[powerup.type] == null)
        {
            activePowerups[powerup.type] = powerup;
            powerup.Activate(tankData);
        }

        activePowerups[powerup.type].duration = powerup.duration;
        
        // Only add the non permanent ones to the list
        if (!powerup.isPermanent)
        {
            powerups.Add(powerup);
        }
        if (powerup.visualModifier)
        {
            ChangeMaterial(tankRenderer, powerup);
        }
    }

    public void ChangeMaterial(TankRenderer tankRenderer, Powerup powerup)
    {
        switch (powerup.vehiclePart)
        {
            case VehiclePart.Turret:
                tankRenderer.turret.SetMaterial(powerup.mainTexture, powerup.color);
                tankRenderer.barrel.SetMaterial(powerup.mainTexture, powerup.color);
                break;

            case VehiclePart.Body:
                tankRenderer.body.SetMaterial(powerup.mainTexture, powerup.color);
                break;

            case VehiclePart.Tracks:
                tankRenderer.tracks.SetMaterial(powerup.mainTexture, powerup.color);
                tankRenderer.wheels.SetMaterial(powerup.mainTexture, powerup.color);
                break;

            case VehiclePart.All:
                tankRenderer.turret.SetMaterial(powerup.mainTexture, powerup.color);
                tankRenderer.barrel.SetMaterial(powerup.mainTexture, powerup.color);
                tankRenderer.body.SetMaterial(powerup.mainTexture, powerup.color);
                tankRenderer.tracks.SetMaterial(powerup.mainTexture, powerup.color);
                tankRenderer.wheels.SetMaterial(powerup.mainTexture, powerup.color);
                break;
        }

    }

    public void RestoreMaterial(TankRenderer tankRenderer, Powerup powerup)
    {
        switch (powerup.vehiclePart)
        {
            case VehiclePart.Turret:
                tankRenderer.turret.RestoreMaterial();
                tankRenderer.barrel.RestoreMaterial();
                break;

            case VehiclePart.Body:
                tankRenderer.body.RestoreMaterial();
                break;

            case VehiclePart.Tracks:
                tankRenderer.tracks.RestoreMaterial();
                tankRenderer.wheels.RestoreMaterial();
                break;

            case VehiclePart.All:
                tankRenderer.turret.RestoreMaterial();
                tankRenderer.barrel.RestoreMaterial();
                tankRenderer.body.RestoreMaterial();
                tankRenderer.tracks.RestoreMaterial();
                tankRenderer.wheels.RestoreMaterial();
                break;

        }
    }
}