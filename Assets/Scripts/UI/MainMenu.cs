using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//TODO: Clean tons of code up
//TODO: Move all non-input related code to other classes

public class MainMenu : MonoBehaviour
{
    public bool isMapOfTheDay;
    public static bool splitScreen = false;
    public static int mapSeed;
    public OptionsMenu optionsMenu;
    public Toggle mapOfTheDayToggle;
    public Toggle splitScreenToggle;
    public InputField seedInputField;
    public Button randomSeedButton;
    public TextMeshProUGUI score;
    public TextMeshProUGUI highScore;

    void Start()
    {
        if (Input.GetJoystickNames().Length > 0)
        {

        }
        highScore.text += PlayerPrefs.GetInt("Player1HighScore", 0).ToString();
        score.text += PlayerPrefs.GetInt("Player1Score", 0).ToString();
        Debug.Log("CursorVisible: " + Cursor.visible);
        Debug.Log("Player1Score: " + PlayerPrefs.GetInt("Player1Score", 0).ToString());
    }

    void Update()
    {

    }

    public void SplitScreen()
    {
        AudioManager.instance.Play("buttonPressed");
        Debug.Log("OptionsMenu: " + optionsMenu.splitScreenOptions.gameObject);

        //Disable/Enable the splitScreen Options
        optionsMenu.splitScreenOptions.gameObject.SetActive(splitScreenToggle.isOn);
        splitScreen = splitScreenToggle.isOn;
    }

    public void MapOfTheDay()
    {
        AudioManager.instance.Play("buttonPressed");

        //Disable/Enable the seedInputfield and the rndSeedButton
        seedInputField.gameObject.SetActive(!mapOfTheDayToggle.isOn);
        seedInputField.enabled = !mapOfTheDayToggle.isOn;

        randomSeedButton.gameObject.SetActive(!mapOfTheDayToggle.isOn);
        randomSeedButton.enabled = !mapOfTheDayToggle.isOn;
    }

    public void RandomSeed()
    {
        AudioManager.instance.Play("buttonPressed");

        //Generate a random seed
        GenerateSeed();

        //Display seed in the inputField
        seedInputField.text = mapSeed.ToString();
    }

    public void CustomSeed()
    {
        if (seedInputField.text == "") return;

        mapSeed = int.Parse(seedInputField.text);
    }

    public void Play()
    {
        AudioManager.instance.Play("buttonPressed");

        //If no seed has been set, return
        if (mapOfTheDayToggle.isOn == false && seedInputField.text == "") return;

        //If the map of the day is set and the user clicks play
        if (mapOfTheDayToggle.isOn == true)
        {
            isMapOfTheDay = true;
            GenerateSeed();
            SceneManager.LoadScene("Game");
        }
        else
        {
            isMapOfTheDay = false;
            if (seedInputField.text != "")
            {
                SceneManager.LoadScene("Game");
            }
        }

    }

    public void Options()
    {
        AudioManager.instance.Play("buttonPressed");

        if (optionsMenu.gameObject.activeSelf == false)
            optionsMenu.gameObject.SetActive(true);
        else
            optionsMenu.gameObject.SetActive(false);
    }

    public int DateToInt(System.DateTime date)
    {
        //Add the date up and return it
        return (int)date.Year + date.Month + date.Day;
    }


    public void GenerateSeed()
    {
        if (isMapOfTheDay)
            mapSeed = DateToInt(System.DateTime.UtcNow);
        else
            mapSeed = System.Guid.NewGuid().GetHashCode();
    }

}
