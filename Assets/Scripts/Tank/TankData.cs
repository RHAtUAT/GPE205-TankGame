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

    //Vehicle properties
    [Header("Vehicle Body")]
    public GameObject rightTrack;
    public GameObject leftTrack;
    public TankWheel[] wheels;
    
    public GameObject wheelBase;
    private GameObject temp;

    [HideInInspector] public List<MeshRenderer> wheelRenderers;
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
        wheels = GetComponentsInChildren<TankWheel>();

        foreach(TankWheel wheel in wheels)
        {
            wheelRenderers.AddRange(wheel.GetComponents<MeshRenderer>());
        }
    }

    void Update()
    {
    }
}

