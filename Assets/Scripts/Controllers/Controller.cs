using UnityEngine;

public class Controller : MonoBehaviour
{
    [HideInInspector] public int score = 0;
    [HideInInspector] public int kills = 0;
    [HideInInspector] public int deaths = 0;
    [HideInInspector] public int lives;
    [HideInInspector] public bool alive = true;

    public void SetLives(int livesAmount)
    {
        if (GameManager.instance.limitedLives == true)
            lives = livesAmount;
    }
}
