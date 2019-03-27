using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponData : MonoBehaviour
{

    //Weapon properties
    [Header("Weapon Properties")]
    public float fireRate = 10.0f;
    [Tooltip("Automatically gets the gameObject with the weapon script attached")]
    public Weapon weapon;
    public GameObject turret;
    public GameObject owner;

    [Header("Projectile properties")]
    public int velocity = 25;
    public int damage;
    public float despawnTime = 3.0f;
    public Vector3 positionOffset = new Vector3(0.0f, 0.0f, 0.5f);
    public Projectile projectilePrefab;
    public ParticleSystem muzzleFlashPrefab;

    private new AudioSource audio;
    private TankData tank;
    private float nextShot;

    // Use this for initialization
    void Start()
    {
        audio = GetComponent<AudioSource>();
        weapon = GetComponentInChildren<Weapon>();
        tank = GetComponent<TankData>();
        nextShot = Time.time;
    }

    public void Fire()
    {
        if(Time.time > nextShot)
        {
            
            //Create the projectile to be fired
            Vector3 spawnLocation = weapon.transform.position + (weapon.transform.forward * positionOffset.y) + (weapon.transform.up * positionOffset.z);
            Projectile projectile = Instantiate<Projectile>(projectilePrefab, spawnLocation, Quaternion.Euler(weapon.transform.rotation.eulerAngles));
            Instantiate<ParticleSystem>(muzzleFlashPrefab, spawnLocation, Quaternion.Euler(weapon.transform.rotation.eulerAngles));
            audio.Play();

            //Set the projectiles information
            projectile.owner = tank.gameObject;
            projectile.despawnTime = despawnTime;
            projectile.velocity = velocity;
            projectile.damage = damage;


            //For this method of using a timer, we are storing the next time that an event can occur and checking each frame if it is time for that event to occur.
            //Therefore, we need to calculate the next time the event can occur as soon as the timer is instantiated.
            //Rounds Per Second
            nextShot = Time.time + 1/fireRate;
        }
    }
}
