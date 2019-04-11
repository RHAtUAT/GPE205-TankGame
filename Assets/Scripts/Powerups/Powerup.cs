using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Powerup
{
    public PickupType type;
    public float duration;
    public bool isPermanent;
    public bool visualModifier;

    [Header("Vehicle Visuals")]
    public VehiclePart vehiclePart;
    public Texture mainTexture;
    public Color color;
    public bool outerShell;
    public GameObject ShieldPrefab;

    public abstract void Activate(TankData tankData);
    public abstract void Deactivate(TankData tankData);
}

public enum VehiclePart { Turret, Body, Tracks, All };

[System.Serializable]
public static class PowerupSettings
{
    public static SpeedPowerup speedPowerup;
    public static ShieldPowerup shieldPowerup;
    public static FireRatePowerup fireRatePowerup;
    public static HealPowerup healPowerup;
}