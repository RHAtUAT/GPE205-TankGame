using UnityEngine;

[RequireComponent(typeof(InputController))]
[RequireComponent(typeof(Stats))]
[System.Serializable]
public class Player : Controller
{
    public Stats stats;
    public InputController inputController;
    public Camera camera;
    public TankData pawn;

    // Start is called before the first frame update
    void Awake()
    {
        stats = GetComponent<Stats>();
        inputController = GetComponent<InputController>();
    }

    void Start()
    {
        SpawnManager.instance.players.Add(this);
        SetLives(GameManager.instance.playerLives);
    }

    // Update is called once per frame
    void Update()
    {
        if (pawn == null) return;
        pawn.stats = stats;
        inputController.pawn = pawn;

    }
}
