using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//TODO: Figure out how to display the cursor after scene loads

public class GameOver : MonoBehaviour
{
    public Text score;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        score.text += PlayerPrefs.GetInt("Player1Score", 0).ToString();
    }

    public void Restart()
    {
        SceneManager.LoadScene("Game");
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
