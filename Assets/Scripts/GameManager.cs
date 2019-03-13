using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    //Player
    [Header("Player")]
    public InputController player;

    //Camera
    [Header("Camera")]
    public float cameraXMax;
    public float cameraYMax;
    public float cameraXMin;
    public float cameraYMin;
    public float yOffset;
    public float xOffset;

    //Audio
    [Header("Audio")]
    public AudioSource gameMusic;
    public AudioSource playerJumpSound;

    //UI elements
    [Header("UI Elements")]
    public Text playerLivesText;
    public Image playerLivesImage;
    //public Text currentLevelText;
    //[Tooltip("Enter the names of scenes to exclude the Lives UI elements from")]
    //public List<string> sceneExclusionList;

    // Use this for initialization
    void Awake()
    {

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else { Destroy(gameObject); }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
