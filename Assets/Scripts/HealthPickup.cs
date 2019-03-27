using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public int healthAmount = 100;
    private Health health;
    private Health healthInParent;

    private void OnTriggerEnter(Collider other)
    {
        health = GetComponent<Health>();
        healthInParent = GetComponentInParent<Health>();

        if(health != null)
        {
            if (health.currentHealth + healthAmount > health.maxHealth)
                health.currentHealth = health.maxHealth;
            else
                health.currentHealth += healthAmount;
        }

        if (healthInParent != null)
        {
            if (healthInParent.currentHealth + healthAmount > healthInParent.maxHealth)
                healthInParent.currentHealth = healthInParent.maxHealth;
            else
                healthInParent.currentHealth += healthAmount;
        }
    }
}
