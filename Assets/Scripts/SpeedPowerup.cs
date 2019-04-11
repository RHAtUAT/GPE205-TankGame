using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedPowerup : Powerup
{
    [Range(0, 100)]
    public float moveSpeedModifier;
    [Range(0, 90)]
    public float turnSpeedModifier;

    float baseMoveSpeed;
    float baseTurnSpeed;

    public SpeedPowerup()
    {
        type = PickupType.Speed;
        moveSpeedModifier = PowerupSettings.speedPowerup.moveSpeedModifier;
        turnSpeedModifier = PowerupSettings.speedPowerup.turnSpeedModifier;
    }

    public override void Activate(TankData tankData)
    {
        baseMoveSpeed = tankData.moveSpeed;
        baseTurnSpeed = tankData.turnSpeed;

        tankData.moveSpeed += moveSpeedModifier;
        tankData.turnSpeed += moveSpeedModifier;
    }

    public override void Deactivate(TankData tankData)
    {
        tankData.moveSpeed = baseMoveSpeed;
        tankData.turnSpeed = baseTurnSpeed;
    }
}
