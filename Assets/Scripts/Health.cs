using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//TODO: Replace Destroy with explosion effect
public class Health : MonoBehaviour, Damagable {

    public int maxHealth = 100;
    public int currentHealth;

    // Use this for initialization
    void Start () {

        currentHealth = maxHealth;
    }
	
	// Update is called once per frame
	void Update () {
		
        if(currentHealth <= 0)
        {
            Destroy(this.gameObject);
        }
	}

    public void Damage(int amount)
    {
        currentHealth -= amount;
    }

}
