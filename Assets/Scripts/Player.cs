using UnityEngine;

[RequireComponent(typeof(InputController))]
[System.Serializable]
public class Player : MonoBehaviour
{
    public InputController inputController;
    public TankData pawn;

    // Start is called before the first frame update
    void Awake()
    {
        inputController = GetComponent<InputController>();
    }

    void Start()
    {
        //SpawnManager.instance.players.Add(this);
        //pawn = GameManager.instance.playerPrefab;
    }

    // Update is called once per frame
    void Update()
    {
    }
}
