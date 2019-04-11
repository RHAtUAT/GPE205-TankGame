using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPowerup : Powerup
{

    public GameObject instancePrefab;
    private List<GameObject> shieldObjects = new List<GameObject>();

    public ShieldPowerup()
    {
        type = PickupType.Shield;
        instancePrefab = PowerupSettings.shieldPowerup.instancePrefab;
    }

    public override void Activate(TankData tankData)
    {
        //Copy every rendered mesh and overlay the tank with them
        foreach (GameObject gameObject in tankData.tankRenderer.renderedObjects)
        {
            if (gameObject.GetComponent<MeshFilter>() == null) return;
            
            GameObject shield = GameObject.Instantiate(instancePrefab, gameObject.transform.position, gameObject.transform.rotation);
            shieldObjects.Add(shield);
            shield.transform.parent = gameObject.transform;
            shield.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
            shield.GetComponent<MeshFilter>().mesh = gameObject.GetComponent<MeshFilter>().mesh;
            
        }
    }

    public override void Deactivate(TankData tankData)
    {
        shieldObjects.ForEach((shield) => {
            GameObject.Destroy(shield);
        });
    }
}
