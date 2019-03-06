using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponData : MonoBehaviour
{

    //Weapon properties
    [Header("Weapon Properties")]
    public float fireRate;
    [Tooltip("Automatically gets the gameObject with the weapon script attached")]
    public Weapon weapon;
    public GameObject turret;
    private float nextShot;
    private TankData tank;

    [Header("Projectile properties")]
    public int velocity;
    public int damage;
    public float zOffset = 0.5f;
    public float yOffset;
    public float despawnTime;
    public Projectile projectilePrefab;

    // Use this for initialization
    void Start()
    {
        weapon = GetComponentInChildren<Weapon>();
        tank = GetComponent<TankData>();
        nextShot = Time.time;
    }

    public void Fire()
    {
        if(Time.time > nextShot)
        {
            //Create the projectile to be fired
            Projectile projectile = Instantiate<Projectile>(projectilePrefab, weapon.transform.position + (weapon.transform.forward * yOffset) + (weapon.transform.up * zOffset), Quaternion.identity);

            //Set the projectiles information
            projectile.transform.Rotate(90, weapon.tf.eulerAngles.y, 0);
            projectile.despawnTime = despawnTime;
            projectile.velocity = velocity;
            projectile.damage = damage;

           // Debug.Log("Tank Rotation: " + tank.transform.rotation);
            //Debug.Log("Projectile X Rotation" + projectile.transform.rotation.x);

            //For this method of using a timer, we are storing the next time that an event can occur and checking each frame if it is time for that event to occur.
            //Therefore, we need to calculate the next time the event can occur as soon as the timer is instantiated.
            nextShot = Time.time + 1/fireRate;
            //Debug.Log("Game Time: " + Time.time);
           // Debug.Log("Time: " + nextShot);
        }
    }
}
