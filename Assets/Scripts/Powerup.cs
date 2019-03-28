using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Powerup
{
    public float moveSpeedModifier;
    public float turnSpeedModifier;
    public float shieldModifier;
    public float healthModifier;
    public float fireRateModifier;
    public float duration;
    public bool visualModifier;
    public bool isPermanent;
    public List<MeshRenderer> wheels;


    public void Activate(TankData tankData)
    {
        tankData.moveSpeed += moveSpeedModifier;
        tankData.weaponData.fireRate += fireRateModifier;
    }

    public void Deactivate(TankData tankData)
    {
        tankData.moveSpeed -= moveSpeedModifier;
        tankData.weaponData.fireRate -= fireRateModifier;
    }

    void SpeedPowerup(TankData tankData)
    {
        tankData.moveSpeed += moveSpeedModifier;
        tankData.turnSpeed += turnSpeedModifier;
    }

    void FireRatePowerup(TankData tankData)
    {
        tankData.weaponData.fireRate += fireRateModifier;
    }
}
