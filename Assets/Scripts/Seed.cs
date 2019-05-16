using UnityEngine;

public class Seed : MonoBehaviour
{
    public static Seed instance;
    public int seed;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
}
