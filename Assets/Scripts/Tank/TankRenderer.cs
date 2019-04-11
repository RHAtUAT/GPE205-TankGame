using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//generic<dataType> allows us to initialize a derived class that can take any dataType we choose to pass in
[System.Serializable]
public abstract class GenericRenderer
{
    //[HideInInspector] public List<dataType> renderersList = new List<dataType>();
    public List<GameObject> gameObjectsList = new List<GameObject>();
    public Dictionary<string, Texture> originalTextures = new Dictionary<string, Texture>();
    public Dictionary<string, Color> originalColors = new Dictionary<string, Color>();

    public void AddGameObject(GameObject gameObject)
    {
        if (gameObject != null)
        {
            if (gameObject.GetComponent<Renderer>() != null)
            {
                gameObjectsList.Add(gameObject);
            }
        }
    }

    public abstract void AddGameObjects();

    public void SetMaterial(Texture powerupTexture, Color powerupColor)
    {
        Debug.Log("SetMaterial");
        gameObjectsList.ForEach((gameObject) =>
        {
            gameObject.GetComponent<Renderer>().material.SetTexture("_MainTex", powerupTexture);
            gameObject.GetComponent<Renderer>().material.color = powerupColor;
        });
    }

    public void RestoreMaterial()
    {
        Debug.Log("RestoreMaterial");
        gameObjectsList.ForEach((gameObject) =>
        {
                gameObject.GetComponent<Renderer>().material.SetTexture("_MainTex", originalTextures[gameObject.name]);
                gameObject.GetComponent<Renderer>().material.SetColor("_Color", originalColors[gameObject.name]);
        });
    }

    public void SaveMaterials()
    {
        Debug.Log("Saving Materials");
        gameObjectsList.ForEach((gameObject) =>
        {
            originalTextures.Add(gameObject.name, gameObject.GetComponent<Renderer>().material.GetTexture("_MainTex"));
            originalColors.Add(gameObject.name, gameObject.GetComponent<Renderer>().material.GetColor("_Color"));
        });
    }

    public void DestroyShield()
    {

    }
}
[System.Serializable]
public class Body : GenericRenderer
{
    public GameObject bodyRoot;
    public GameObject doorRight;
    public GameObject doorLeft;
    public GameObject extra0;
    public GameObject extra1;
    public GameObject extra2;
    public GameObject extra3;

    public override void AddGameObjects()
    {
        AddGameObject(bodyRoot);
        AddGameObject(doorRight);
        AddGameObject(doorLeft);
        AddGameObject(extra0);
        AddGameObject(extra1);
        AddGameObject(extra2);
        AddGameObject(extra3);
    }
}

[System.Serializable]
public class Turret : GenericRenderer
{
    public GameObject turretBase;
    public GameObject armor;
    public GameObject hatch;
    public GameObject hatchDoor;
    public GameObject extra0;
    public GameObject extra1;


    public override void AddGameObjects()
    {
        AddGameObject(turretBase);
        AddGameObject(armor);
        AddGameObject(hatch);
        AddGameObject(hatchDoor);
        AddGameObject(extra0);
        AddGameObject(extra1);
    }
}

[System.Serializable]
public class Barrel : GenericRenderer
{
    public GameObject barrel;
    public GameObject extra0;
    public GameObject extra1;

    public override void AddGameObjects()
    {
        AddGameObject(barrel);
        AddGameObject(extra0);
        AddGameObject(extra1);
    }
}

[System.Serializable]
public class Tracks : GenericRenderer
{
    public GameObject right;
    public GameObject left;

    public override void AddGameObjects()
    {
        gameObjectsList.Add(right);
        gameObjectsList.Add(left);
    }
}

[System.Serializable]
public class Wheels : GenericRenderer
{
    public GameObject wheelBase;
    [Tooltip("The TankWheel script retrieves the MeshRender of the GameObject its attached to and adds it to this list automatically")]
    public List<GameObject> allWheels;


    public Wheels()
    {
        allWheels = new List<GameObject>();
    }

    public void addChildObjects(TankWheel[] tankWheels)
    {

        foreach (TankWheel wheel in tankWheels)
        {
            allWheels.Add(wheel.gameObject);
        }
    }

    public override void AddGameObjects()
    {
        AddGameObject(wheelBase);
        foreach (GameObject gameObject in allWheels)
        {
            AddGameObject(gameObject);
        }
    }
}

public class TankRenderer : MonoBehaviour
{
    public Body body = new Body();
    public Turret turret = new Turret();
    public Barrel barrel = new Barrel();
    public Wheels wheels = new Wheels();
    public Tracks tracks = new Tracks();
    public List<GameObject> renderedObjects;

    void Start()
    {
        TankWheel[] tankWheels = GetComponentsInChildren<TankWheel>();
        wheels.addChildObjects(tankWheels);

        body.AddGameObjects();
        turret.AddGameObjects();
        barrel.AddGameObjects();
        tracks.AddGameObjects();
        wheels.AddGameObjects();

        //Add all renderers to the activeRenderer list
        foreach (GameObject gameObject in body.gameObjectsList) renderedObjects.Add(gameObject);
        foreach (GameObject gameObject in turret.gameObjectsList) renderedObjects.Add(gameObject);
        foreach (GameObject gameObject in barrel.gameObjectsList) renderedObjects.Add(gameObject);
        foreach (GameObject gameObject in wheels.gameObjectsList) renderedObjects.Add(gameObject);
        foreach (GameObject gameObject in tracks.gameObjectsList) renderedObjects.Add(gameObject);

        body.SaveMaterials();
        turret.SaveMaterials();
        barrel.SaveMaterials();
        tracks.SaveMaterials();
        wheels.SaveMaterials();
    }
}