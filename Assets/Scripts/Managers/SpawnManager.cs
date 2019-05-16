using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//TODO: Refine how available spawnpoints are retrieved
//TODO: Clean up code and optimize
//TODO: Add dynamic spawning for amount of players
//TODO: Fix camera2 and Resize player2's UI


public class SpawnManager : MonoBehaviour
{
    public static SpawnManager instance;

    public List<TankData> activeTanks = new List<TankData>();
    public List<AIController> aIControllers;
    public List<Player> players;

    [Header("Player Spawn Points")]
    public List<SpawnPoint> playerSpawnPoints;
    public List<SpawnPoint> playerRespawnPoints = new List<SpawnPoint>();
    public List<SpawnPoint> availablePlayerRespawnPoints = new List<SpawnPoint>();
    public List<SpawnPoint> availablePlayerSpawnPoints = new List<SpawnPoint>();

    [Header("AI Spawn Points")]
    public List<SpawnPoint> AISpawnPoints = new List<SpawnPoint>();
    public List<SpawnPoint> AIRespawnPoints = new List<SpawnPoint>();
    public List<SpawnPoint> availableAIRespawnPoints = new List<SpawnPoint>();
    public List<SpawnPoint> availableAISpawnPoints = new List<SpawnPoint>();

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        GetPlayerSpawnPoints();
        GetEnemySpawnPoints();

        players.Add(GameManager.instance.player1);
        players.Add(GameManager.instance.player2);

        foreach (Player player in players)
        {
            SpawnPlayer(player);

        }
    }

    // Update is called once per frame
    void Update()
    {
        //SpawnAI();
        if (players.Count != 0)
        {
            if (GameManager.instance.splitScreen == true)
            {
                if (players[0].lives <= 0 && GameManager.instance.limitedLives == true)
                {
                    Debug.Log("P1Score: " + GameManager.instance.player1Score);
                    UIManager.instance.DeathScreen(players[0]);
                    SaveManager.instance.SaveScore();
                    Debug.Log("PlayerPrefsP1Score: " + PlayerPrefs.GetInt("Player1Score", 0).ToString());
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                }

                if (players[1].lives <= 0 && GameManager.instance.limitedLives == true)
                {
                    UIManager.instance.DeathScreen(players[1]);
                    SaveManager.instance.SaveScore();
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                }

                if (players[0].lives <= 0 && players[1].lives <= 0)
                    SceneManager.LoadScene("Game Over");
            }
            else
            {
                if (players[0].lives <= 0 && GameManager.instance.limitedLives == true)
                {
                    Debug.Log("P1Score: " + GameManager.instance.player1Score);
                    SaveManager.instance.SaveScore();
                    Debug.Log("PlayerPrefsP1Score: " + PlayerPrefs.GetInt("Player1Score", 0).ToString());
                    SceneManager.LoadScene("Game Over");
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                }
                else
                {
                    if (GameManager.instance.player1.pawn == null)
                    {
                        RespawnPlayer(players[0]);
                        Debug.Log("Respawning Player");
                    }
                }
            }
        }

        //RespawnAI();
        //Debug.Log("Running RespawnAI()");

        if (SceneManager.GetActiveScene().name != "Game") return;
    }


    //Get the Player Spawn Points and Respawn Points
    public void GetPlayerSpawnPoints()
    {
        SpawnPoint[] spawnPoints = GameObject.FindObjectsOfType<SpawnPoint>();
        foreach (SpawnPoint spawnPoint in spawnPoints)
        {

            //Add Spawns
            if (spawnPoint.team == SpawnPoint.Team.Player && spawnPoint.pointType == SpawnPoint.PointType.Spawn)
                playerSpawnPoints.Add(spawnPoint);

            //Add Respawns
            if (spawnPoint.team == SpawnPoint.Team.Player && spawnPoint.pointType == SpawnPoint.PointType.Respawn)
                playerRespawnPoints.Add(spawnPoint);
        }
        if (spawnPoints == null)
            Debug.LogWarning("No Player Spawn Points found!");
    }

    //Get the Enemy Spawn Points and Respawn Points
    public void GetEnemySpawnPoints()
    {
        SpawnPoint[] spawnPoints = GameObject.FindObjectsOfType<SpawnPoint>();
        foreach (SpawnPoint spawnPoint in spawnPoints)
        {
            //Add Spawns
            if (spawnPoint.team == SpawnPoint.Team.Enemy && spawnPoint.pointType == SpawnPoint.PointType.Spawn)
                AISpawnPoints.Add(spawnPoint);

            //Add Respawns
            if (spawnPoint.team == SpawnPoint.Team.Enemy && spawnPoint.pointType == SpawnPoint.PointType.Respawn)
                AIRespawnPoints.Add(spawnPoint);
        }
        if (spawnPoints == null)
            Debug.LogWarning("No Enemy Spawn Points found!");
    }


    void SpawnAI()
    {
        if (aIControllers.Count <= 0)
        {
            Debug.LogWarning("No AI Controllers found!");
            return;
        }

        //Check if there are any AI spawn points in the scene
        if (AISpawnPoints.Count <= 0)
        {
            Debug.LogWarning("No AI Spawn Points found!");
            return;
        }

        //Choose an available spawn/respawn point for the tank
        foreach (AIController aIController in aIControllers)
        {

            //Spawn the AI pawn
            if (aIController.pawn != null && aIController.firstSpawn == true)
            {
                //Check for available spawn points
                AvailableSpawnPoint(AISpawnPoints, availableAISpawnPoints);
                if (availableAISpawnPoints.Count <= 0) return;

                Debug.Log("Spawning AI");
                int locationID = Random.Range(0, availableAISpawnPoints.Count);

                //Disable the AIController script to prevent errors
                aIController.enabled = false;

                //The aIcontroller now controls this pawn
                aIController.pawn = Instantiate(GameManager.instance.AIPrefab, availableAISpawnPoints[locationID].transform.position, availableAISpawnPoints[locationID].transform.rotation);
                availableAISpawnPoints.Remove(availableAISpawnPoints[locationID]);

                //Enable it once it has a new pawn
                aIController.enabled = true;
                activeTanks.Add(aIController.pawn);

                aIController.firstSpawn = false;
            }
        }
    }

    void RespawnAI()
    {
        if (aIControllers.Count <= 0)
        {
            Debug.LogWarning("No AI Controllers found!");
            return;
        }

        //Check if there are any AI respawn points in the scene
        if (AIRespawnPoints.Count <= 0)
        {
            Debug.LogWarning("No AI Respawn Points found!");
            return;
        }

        //Choose an available spawn/respawn point for the tank
        foreach (AIController aIController in aIControllers)
        {
            //if (aIController.lives <= 0 && GameManager.instance.limitedLives == true)
            //{
            //    aIController.alive = false;
            //    aIController.enabled = false;
            //    aIController.gameObject.SetActive(false);
            //    break;
            //}

            //Respawn the AI pawn
            if (aIController.pawn != null && GameManager.instance.enableRespawning == true)
            {

                //Check for available respawn points
                AvailableSpawnPoint(AIRespawnPoints, availableAIRespawnPoints);
                if (availableAIRespawnPoints.Count <= 0) return;

                Debug.Log("Respawning AI");

                int locationID = Random.Range(0, availableAIRespawnPoints.Count - 1);

                //Disable the AIController script to prevent errors
                aIController.enabled = false;


                //The aIcontroller now controls this pawn
                aIController.pawn = Instantiate(GameManager.instance.AIPrefab, availableAIRespawnPoints[locationID].transform.position, availableAIRespawnPoints[locationID].transform.rotation);

                //Enable it once it has a new pawn
                aIController.enabled = true;

                activeTanks.Add(aIController.pawn);
            }
        }
    }


    void SpawnPlayer(Player player)
    {
        if (player == null)
        {
            Debug.LogWarning("No Players found!");
            return;
        }

        //Check if there are any AI spawn points in the scene
        if (playerSpawnPoints.Count <= 0)
        {
            Debug.LogWarning("No Player Spawn Points found!");
            return;
        }

        //Spawn the inputcontroller pawn
        if (player.pawn == null)
        {
            //Check for available spawn points
            AvailableSpawnPoint(playerSpawnPoints, availablePlayerSpawnPoints);
            if (availablePlayerSpawnPoints.Count <= 0) return;

            Debug.Log("Spawning Player");
            int locationID = Random.Range(0, availablePlayerSpawnPoints.Count);

            //Disable the inputController script to prevent errors
            player.inputController.enabled = false;

            //The inputController now controls this pawn
            player.pawn = Instantiate(GameManager.instance.playerPrefab, availablePlayerSpawnPoints[locationID].transform.position, availablePlayerSpawnPoints[locationID].transform.rotation);

            //Enable it once it has a new pawn
            player.inputController.enabled = true;
            activeTanks.Add(player.pawn);
        }
    }

    void RespawnPlayer(Player player)
    {
        if (player == null) return;

        if (playerRespawnPoints.Count <= 0)
        {
            Debug.LogWarning("No Player Respawn Points found!");
            return;
        }

        //Check if there are any AI spawn points in the scene
        if (playerSpawnPoints.Count <= 0)
        {
            Debug.LogWarning("No Player Spawn Points found!");
            return;
        }

        //Respawn the inputcontroller pawn
        if (player.pawn == null && GameManager.instance.enableRespawning == true)
        {

            //Check for available respawn points
            AvailableSpawnPoint(playerRespawnPoints, availablePlayerRespawnPoints);
            if (availablePlayerRespawnPoints.Count <= 0) return;

            Debug.Log("Respawning Player");

            int locationID = Random.Range(0, availablePlayerRespawnPoints.Count);
            Debug.Log("Location ID: " + locationID);

            //Disable the inputController script to prevent errors
            player.inputController.enabled = false;

            //The InputController now controls this pawn
            player.pawn = Instantiate(GameManager.instance.playerPrefab, availablePlayerRespawnPoints[locationID].transform.position, availablePlayerRespawnPoints[locationID].transform.rotation);

            //Enable it once it has a new pawn
            player.inputController.enabled = true;
            activeTanks.Add(player.pawn);
        }
    }

    //Only populates the list after a player or AI pawn dies
    public void AvailableSpawnPoint(List<SpawnPoint> spawnPoints, List<SpawnPoint> listToPopulate)
    {
        Debug.Log("Checking for available spawn points");
        foreach (SpawnPoint spawnPoint in spawnPoints)
        {

            //If there are no tanks all spawn points are available so add them until a tank exists
            if (activeTanks.Count <= 0)
            {
                //Debug.Log("Added");
                spawnPoint.available = true;
                listToPopulate.Add(spawnPoint);
            }
            //See if any tanks are too close to the spawn point
            foreach (TankData activeTank in activeTanks)
            {
                if (Vector3.Distance(spawnPoint.transform.position, activeTank.transform.position) > GameManager.instance.inUseRadius)
                {
                    spawnPoint.available = true;
                }
                //If a single tank is too close, the spawn point isnt available
                else
                {
                    spawnPoint.available = false;

                    //Break out of the loop and check if the next spawn point is available
                    break;
                }
            }

            //Debug.Log("SpawnPoint: " + spawnPoint);
            //Debug.Log("SpawnPointName: " + spawnPoint.name);
            //Debug.Log("SpawnPointAvailable: " + spawnPoint.available);

            if (spawnPoint.available == true)
            {
                //If the list doesn't already contain the available spawn point
                //May have to change this once multiplayer is working
                if (!listToPopulate.Contains(spawnPoint))
                {
                    Debug.Log("Added");
                    listToPopulate.Add(spawnPoint);
                }
            }

            //If the spawn point is unavailable
            else
            {
                Debug.Log("Removed");
                listToPopulate.Remove(spawnPoint);
            }
        }
    }
}
