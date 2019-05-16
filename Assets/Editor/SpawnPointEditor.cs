using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SpawnPoint))]
public class SpawnPointEditor : Editor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        SpawnPoint spawnPoint = (SpawnPoint)target;

        if (spawnPoint.team == SpawnPoint.Team.All)
        {
            spawnPoint.ChangeColor(spawnPoint.allColor);

            if (spawnPoint.pointType == SpawnPoint.PointType.Spawn)
                spawnPoint.name = "SpawnPoint";
            else
                spawnPoint.name = "RespawnPoint";
        }


        else if (spawnPoint.team == SpawnPoint.Team.Player)
        {
            spawnPoint.ChangeColor(spawnPoint.playerColor);

            if (spawnPoint.pointType == SpawnPoint.PointType.Spawn)
                spawnPoint.name = "PlayerSpawnPoint";
            else
                spawnPoint.name = "PlayerRespawnPoint";
        }

        else
        {
            spawnPoint.ChangeColor(spawnPoint.enemyColor);

            if (spawnPoint.pointType == SpawnPoint.PointType.Spawn)
                spawnPoint.name = "EnemySpawnPoint";
            else
                spawnPoint.name = "EnemyRespawnPoint";
        }
    }

    public void OnSceneGUI()
    {
        SpawnPoint spawnPoint = (SpawnPoint)target;

        if (spawnPoint.team == SpawnPoint.Team.All)
        {
            spawnPoint.ChangeColor(spawnPoint.allColor);

            if (spawnPoint.pointType == SpawnPoint.PointType.Spawn)
                spawnPoint.name = "SpawnPoint";
            else
                spawnPoint.name = "RespawnPoint";
        }


        else if (spawnPoint.team == SpawnPoint.Team.Player)
        {
            spawnPoint.ChangeColor(spawnPoint.playerColor);

            if (spawnPoint.pointType == SpawnPoint.PointType.Spawn)
                spawnPoint.name = "PlayerSpawnPoint";
            else
                spawnPoint.name = "PlayerRespawnPoint";
        }

        else
        {
            spawnPoint.ChangeColor(spawnPoint.enemyColor);

            if (spawnPoint.pointType == SpawnPoint.PointType.Spawn)
                spawnPoint.name = "EnemySpawnPoint";
            else
                spawnPoint.name = "EnemyRespawnPoint";
        }
    }


}
