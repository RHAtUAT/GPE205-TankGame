using UnityEngine;


//TODO: Fix the way speed is reset so it doesnt have to be hard coded

[System.Serializable]
public class SpeedPowerup : Powerup
{
    [Range(0, 100)]
    public float forwardSpeedModifier;
    [Range(0, 100)]
    public float backwardSpeedModifier;
    [Range(0, 90)]
    public float turnSpeedModifier;

    public SpeedPowerup()
    {
        type = PickupType.Speed;
    }

    public override void Activate(TankData tankData)
    {
        tankData.forwardSpeed += forwardSpeedModifier;
        tankData.backwardSpeed += backwardSpeedModifier;
        tankData.turnSpeed += turnSpeedModifier;
        Debug.Log("Activate SpeedPowerup");
    }

    public override void Deactivate(TankData tankData)
    {
        tankData.forwardSpeed = tankData.baseForwardSpeed;
        tankData.backwardSpeed = tankData.baseBackwardSpeed;
        tankData.turnSpeed = tankData.baseTurnSpeed;
        Debug.Log("Deactivate SpeedPowerup");
    }

    public override void Update(TankData tankData)
    {
    }
}
