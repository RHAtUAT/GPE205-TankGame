using System.Collections.Generic;
using UnityEngine;

public class PowerupSpawner : MonoBehaviour
{
    public float spawnTime;
    public GameObject powerupPrefab;
    public GameObject spawnedObject;
    public List<Transform> spawnLocations = new List<Transform>();

    private float countDown;


    // Start is called before the first frame update
    void Start()
    {
        countDown = spawnTime;
    }

    // Update is called once per frame
    void Update()
    {
        //If no powerup has spawned
        if (spawnedObject == null)
        {
            //Start the timer
            //Debug.Log("Countdown: " + countDown);
            countDown -= Time.time;

            //And its time for one to spawn
            if (countDown <= 0)
            {
                if (spawnLocations.Count > 0)
                {
                    //Spawn a powerup in a random location
                    int locID = Random.Range(0, spawnLocations.Count);
                    spawnedObject = Instantiate<GameObject>(powerupPrefab, spawnLocations[locID].position, spawnLocations[locID].rotation);

                    //Reset timer
                    countDown = spawnTime;
                }
                else
                {
                    Debug.LogWarning("No Spawn Locations set for PowerupSpawner");
                }
            }
        }
    }
}
