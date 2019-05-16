using UnityEngine;


public class Stats : MonoBehaviour
{
    public int score;
    public int kills;
    public int deaths;
    public int lives;

    public void SetLives(int livesAmount)
    {
        if (GameManager.instance.limitedLives == true)
            lives = livesAmount;
    }
}
