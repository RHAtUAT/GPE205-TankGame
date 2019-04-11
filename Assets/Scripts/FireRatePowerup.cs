using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireRatePowerup : Powerup
{
    [Range(0, 200)]
    public float fireRateModifier;
    float baseFireRate;

    public FireRatePowerup()
    {
        type = PickupType.FireRate;
        fireRateModifier = PowerupSettings.fireRatePowerup.fireRateModifier;
    }

    public override void Activate(TankData tankData)
    {
        baseFireRate = tankData.weaponData.fireRate;
        tankData.weaponData.fireRate += fireRateModifier;
    }

    public override void Deactivate(TankData tankData)
    {
        tankData.weaponData.fireRate = baseFireRate;
    }
}
