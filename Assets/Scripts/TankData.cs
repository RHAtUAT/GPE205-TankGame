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
    [HideInInspector] public Transform motorTf;
    [HideInInspector] public WeaponData weaponData;
    [HideInInspector] public Pivot pivot;


    // Use this for initialization
    void Start () {

        pivot = GetComponentInChildren<Pivot>();
        motor = GetComponentInChildren<TankMotor>();
        weaponData = GetComponent<WeaponData>();
        motorTf = this.gameObject.transform.GetChild(0).GetChild(0).transform;
        Debug.Log("MotorTrans: " + motorTf);
    }
}

