using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ShieldPowerup : Powerup
{

    public int maxHealth = 100;
    public int currentHealth = 100;
    public HealthBars healthBarsUI;
    public GameObject shieldPrefab;

    private List<GameObject> shieldObjects = new List<GameObject>();

    public ShieldPowerup()
    {
        currentHealth = maxHealth;
        type = PickupType.Shield;
        //shieldPrefab = GameManager.instance.powerupSettings.shieldPowerup.shieldPrefab;
    }

    public override void Activate(TankData tankData)
    {
        Debug.Log("Shield Actvated");

        //Copy every rendered mesh and overlay the tank with them
        foreach (GameObject gameObject in tankData.tankRenderer.renderedObjects)
        {
            if (gameObject.GetComponent<MeshFilter>() == null) return;

            GameObject shield = GameObject.Instantiate(shieldPrefab, gameObject.transform.position, gameObject.transform.rotation);
            shieldObjects.Add(shield);
            shield.transform.parent = gameObject.transform;
            shield.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
            shield.GetComponent<MeshFilter>().mesh = gameObject.GetComponent<MeshFilter>().mesh;
        }
    }

    public override void Deactivate(TankData tankData)
    {
        Debug.Log("Shield Deactvated");

        shieldObjects.ForEach((shield) =>
        {
            GameObject.Destroy(shield);
        });
    }

    public override void Update(TankData tankData)
    {

    }

}
