using System.Collections.Generic;
using UnityEngine;
using Logger = Assets.Scripts.Utilities.Logger;

//TODO: Refine the way stats are displayed
//TODO: Check for all players dead
//TODO: Make an input controller list to keep track all players
//TODO: Fix how gameManager is used
//TODO: Fix OptionsMenu bug 

//https://opengameart.org/content/synthesized-explosion     - tank_shot[audio]
//https://Tank-SoundBible.com-1359027625                    - tank_motor[audio] 

public class GameManager : MonoBehaviour
{

    public static GameManager instance;
    private static readonly Logger Logger = new Logger("GameManager");


    [Header("Game Settings")]
    public int mapSeed;

    //Screen Settings
    public CameraConfig cameraConfig;
    public int screenHeight = Screen.height;
    public int screenWidth = Screen.width;

    //Spawn Settings
    [Header("Spawn Settings")]
    public bool splitScreen = false;
    public bool enableRespawning;
    public bool limitedLives;
    public int playerLives = 10;
    public int AILives = 10;
    public float inUseRadius;

    public List<Player> players;

    //Player 1
    [Header("Player 1")]
    public Player player1;
    public int player1Kills;
    public int player1Deaths;
    public int player1Score;


    //Player 2
    [Header("Player 2")]
    public Player player2;
    public int player2Kills;
    public int player2Deaths;
    public int player2Score;

    //Prefabss
    [Header("Prefabs")]
    public TankData AIPrefab;
    public TankData playerPrefab;

    [Header("Map")]
    public MapGenerator mapGenerator;
    public Room[,] mapGrid;

    // Use this for initialization
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        splitScreen = MainMenu.splitScreen;

        if (splitScreen == true)
        {
            player2.gameObject.SetActive(true);
            Logger.Info("ScreenLayout GM: " + PlayerPrefs.GetInt("ScreenLayout", 1));
            cameraConfig = (CameraConfig)PlayerPrefs.GetInt("ScreenLayout", 1);
        }
        else
        {
            player2.gameObject.SetActive(false);
        }



        //So that the game will never be paused if you go back to the main menu after opening the options menu
        Time.timeScale = 1;

        mapSeed = MainMenu.mapSeed;
        if (mapGenerator != null)
            mapGenerator.GenerateGrid();

        int screenHeight = Screen.height;
        int screenWidth = Screen.width;
    }

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetJoystickNames().Length > 0)
        {
            Debug.Log("GamePadName: " + Input.GetJoystickNames()[0]);
        }


        if (player1 != null)
        {
            if (player1.pawn != null)
            {
                //Send player stats to the game manager
                player1Score = player1.pawn.stats.score;
                player1Kills = player1.pawn.stats.kills;
                player1Deaths = player1.pawn.stats.deaths;
            }
        }

        if (player2 != null)
        {
            if (player2.pawn != null)
            {
                player2Score = player2.pawn.score;
                player2Kills = player2.pawn.kills;
                player2Deaths = player2.pawn.deaths;
            }
        }
    }
}

public enum CameraConfig { SplitScreenV, SplitScreenH, SinglePlayer };
