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
      
    }
}
