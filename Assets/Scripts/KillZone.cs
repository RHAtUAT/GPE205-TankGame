using UnityEngine;

public class KillZone : MonoBehaviour
{
    public float killRadius;
    public int damage = 100;
    Transform tf;
    // Start is called before the first frame update
    void Start()
    {
        tf = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (GameManager.instance.inputController.pawn == null) return;

        //if (Vector3.Distance(GameManager.instance.inputController.pawn.transform.position, tf.position) < killRadius)
        //{
        //    //If the gameObject the projectile hits can take damage
        //    GameManager.instance.inputController.pawn.Damage(damage);
        //}
    }
}
