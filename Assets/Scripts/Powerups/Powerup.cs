using UnityEngine;

public abstract class Powerup
{
    [HideInInspector] public PickupType type;
    public float duration;
    public bool isPermanent;
    public bool visualModifier;
    public AudioClip audio;

    [Header("Vehicle Visuals")]
    public VehiclePart vehiclePart;
    public Texture mainTexture;
    public Color color;
    public GameObject HologramPrefab;

    public abstract void Activate(TankData tankData);
    public abstract void Update(TankData tankData);
    public abstract void Deactivate(TankData tankData);
}

public enum VehiclePart { Turret, Body, Tracks, All };

