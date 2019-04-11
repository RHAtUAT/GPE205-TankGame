using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealPowerup : Powerup
{
    public int amount;

    public HealPowerup()
    {
        type = PickupType.Health;
        amount = PowerupSettings.healPowerup.amount;
    }

    public override void Activate(TankData tankData)
    {
        Health health = tankData.health;
        if (health.currentHealth < health.maxHealth)
        {
            if (health.currentHealth + amount > health.maxHealth)
                health.currentHealth = health.maxHealth;
            else
                health.currentHealth += amount;
        }
    }

    public override void Deactivate(TankData tankData)
    {
        return;
    }
}
