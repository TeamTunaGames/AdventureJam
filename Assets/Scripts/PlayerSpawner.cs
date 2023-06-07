using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private int spawnerID;

    private void Start()
    {
        GameMaster gameMaster = GameMaster.Instance;

        if (spawnerID == gameMaster.spawnLocation)
        {
            if (gameMaster.PlayerInstance != null)
                gameMaster.PlayerInstance.transform.SetPositionAndRotation(transform.position, Quaternion.identity);
            else
                Instantiate(gameMaster.PlayerPrefab, transform.position, Quaternion.identity);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawIcon(transform.position + Vector3.up, "Fumo.png", true);
    }
}
