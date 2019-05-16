using UnityEngine;

[System.Serializable]
public class SpawnPoint : MonoBehaviour
{
    public bool available;
    public enum Team { All, Player, Enemy };
    public Team team;
    public enum PointType { Spawn, Respawn };
    public PointType pointType;
    public Material allColor;
    public Material enemyColor;
    public Material playerColor;

    private Renderer[] renderers;


    private void Start()
    {
        renderers = this.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            renderer.enabled = false;
        }
    }

    public void ChangeColor(Material material)
    {

        renderers = this.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            renderer.sharedMaterial = material;
            //renderer.sharedMaterial.color = color;
        }
    }

}
