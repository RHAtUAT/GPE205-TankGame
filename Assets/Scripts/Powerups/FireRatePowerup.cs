using UnityEngine;

[System.Serializable]
public class FireRatePowerup : Powerup
{
    [Range(0, 200)]
    public float fireRateModifier;
    float baseFireRate;

    public FireRatePowerup()
    {
        type = PickupType.FireRate;
        //fireRateModifier = GameManager.instance.powerupSettings.fireRatePowerup.fireRateModifier;
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

    public override void Update(TankData tankData)
    {
    }
}
