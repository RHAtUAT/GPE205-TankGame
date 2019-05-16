using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void SaveScore()
    {
        PlayerPrefs.SetInt("Player1Score", GameManager.instance.player1Score);

        if (GameManager.instance.player1Score > PlayerPrefs.GetInt("Player1HighScore", 0))
        {
            PlayerPrefs.SetInt("Player1HighScore", GameManager.instance.player1Score);
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}
