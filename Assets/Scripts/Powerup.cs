using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Powerup
{
    public enum PowerupType {Speed, FireRate, Health, Shield };
    public PowerupType powerupType;
    public float modifier;
    public bool visualModifier;
    public float duration;
    public bool isPermanent;
    public List<MeshRenderer> wheels;

    public void Activate(TankData tankData)
    {
        tankData.moveSpeed += modifier;
        tankData.weaponData.fireRate += modifier;
    }

    public void Deactivate(TankData tankData)
    {
        tankData.moveSpeed -= modifier;
        tankData.weaponData.fireRate -= modifier;
    }
}
