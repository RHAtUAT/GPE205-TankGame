using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(TankMotor))]
//[RequireComponent(typeof(WeaponData))]

public class TankData : MonoBehaviour {

    //Vehicle properties
    [Header("Vehicle Properties")]
    public float moveSpeed;
    public float turnSpeed;
    public int damageDone;
    [HideInInspector] public TankMotor motor;
    [HideInInspector] public WeaponData weaponData;


    // Use this for initialization
    void Start () {

        motor = GetComponentInChildren<TankMotor>();
        weaponData = GetComponent<WeaponData>();
    }
}

