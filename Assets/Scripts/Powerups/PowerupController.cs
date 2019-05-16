using System.Collections.Generic;
using UnityEngine;

public class PowerupController : MonoBehaviour
{
    //TODO: Add logwarning if no wheels
    [HideInInspector] public ShieldPowerup shieldPowerup;
    public Dictionary<PickupType, Powerup> activePowerups = new Dictionary<PickupType, Powerup>();

    private TankRenderer tankRenderer;
    private TankData tankData;
    private List<Renderer> shieldRenderers;
    private GameObject meshObject;

    void Start()
    {
        tankData = GetComponent<TankData>();
        tankRenderer = GetComponent<TankRenderer>();
        activePowerups.Add(PickupType.FireRate, null);
        activePowerups.Add(PickupType.Health, null);
        activePowerups.Add(PickupType.Shield, null);
        activePowerups.Add(PickupType.Speed, null);
    }

    void Update()
    {
        if (tankData == null) return;
        RunPowerups();
        //foreach (Powerup powerup in activePowerups.Values) powerup.Update(tankData);
    }

    public void Add(Powerup powerup)
    {
        // If there is currently an active powerup of this type, deactivate it
        if (activePowerups[powerup.type] != null) activePowerups[powerup.type].Deactivate(tankData);

        // Prevent powerups from stacking
        activePowerups[powerup.type] = null;
        activePowerups[powerup.type] = powerup;
        activePowerups[powerup.type].Activate(tankData);

        // Apply all visual modifiers if any
        if (powerup.visualModifier) ChangeMaterial(tankRenderer, powerup);
    }

    void RunPowerups()
    {

        //Create a buffer for the powerups since so we can modify the values while iterating over them 
        var buffer = new List<Powerup>(activePowerups.Values);

        // Loop through all the powers in the List
        foreach (Powerup powerup in buffer)
        {
            // Check to see if there is a stored instance of this powerup
            if (powerup == null) continue;

            // Subtract from the timer
            if (!powerup.isPermanent) { powerup.duration -= Time.deltaTime; }

            if (powerup.type == PickupType.Shield)
            {
                shieldPowerup = (ShieldPowerup)powerup;

                if (shieldPowerup.currentHealth <= 0)
                {
                    if (shieldPowerup.visualModifier) RestoreMaterial(tankRenderer, shieldPowerup);
                    shieldPowerup.Deactivate(tankData);
                    activePowerups[shieldPowerup.type] = null;
                }
            }

            // Assemble a list of expired powerups
            if (powerup.duration <= 0)
            {
                if (powerup.visualModifier)
                {
                    RestoreMaterial(tankRenderer, powerup);
                }

                powerup.Deactivate(tankData);
                activePowerups[powerup.type] = null;
            }
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