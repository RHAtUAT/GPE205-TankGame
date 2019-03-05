using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponData : MonoBehaviour
{

    //Weapon properties
    [Header("Weapon Properties")]
    public float fireRate;
    private float nextShot;
    [Tooltip("Automatically gets the gameObject with the weapon script attached")]
    public Weapon weapon;
    private TankData tank;

    [Header("Projectile properties")]
    public int velocity;
    public int damage;
    public float despawnTime;
    public Projectile projectilePrefab;
    //public Projectile projectilePrefab2;

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
            //Set the rotation 
            //Quaternion rot = Quaternion.AngleAxis(90, Vector3.right);
            //Create the projectile to be fired
            Quaternion tr = tank.transform.rotation;
            Projectile projectile = Instantiate<Projectile>(projectilePrefab, weapon.tf.position + weapon.tf.forward * .5f, tr);
            projectile.transform.Rotate(90, 0, 0);
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
