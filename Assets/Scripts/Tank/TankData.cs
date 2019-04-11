using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(TankMotor))]
//[RequireComponent(typeof(WeaponData))]

public class TankData : MonoBehaviour {

    //Vehicle properties
    [Header("Vehicle Properties")]
    public float moveSpeed = 5.0f;
    public float turnSpeed = 180.0f;
    public int damageDone;
    //public bool shield 
    public Transform centerOfGravity;
    [HideInInspector] public Health health;
    [HideInInspector] public TankMotor motor;
    [HideInInspector] public Transform motorTf;
    [HideInInspector] public WeaponData weaponData;
    [HideInInspector] public Pivot pivot;
    [HideInInspector] public TankRenderer tankRenderer;

    private Rigidbody rb;

    // Use this for initialization
    void Start () {
        tankRenderer = GetComponent<TankRenderer>();
        health = GetComponent<Health>();
        rb = GetComponent<Rigidbody>();
        pivot = GetComponentInChildren<Pivot>();
        motor = GetComponentInChildren<TankMotor>();
        weaponData = GetComponent<WeaponData>();
        motorTf = this.gameObject.transform.GetChild(0).GetChild(0).transform;
        rb.centerOfMass = centerOfGravity.localPosition;
    }
}