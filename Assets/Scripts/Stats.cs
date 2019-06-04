using UnityEngine;


public class Stats : MonoBehaviour
{
    public int score;
    public int kills;
    public int deaths;
    public int lives;

    /// <summary>
    /// Sets the amount of lives until the controlled pawn will no longer respawn
    /// </summary>
    /// <param name="livesAmount"> The amount of lives until the pawn is destroyed.</param>
    public void SetLives(int livesAmount)
    {
        if (GameManager.instance.limitedLives == true)
            lives = livesAmount;
    }
}
