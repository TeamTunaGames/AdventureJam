using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMaster : Singleton<GameMaster>
{
    public QuestManager QuestManagerInstance { get; private set; }
    public Player PlayerInstance { get; private set; }

    public int spawnLocation = 0;

    [field:SerializeField] public GameObject PlayerPrefab { get; private set; }

    

    public void SetQuestManager(QuestManager manager)
    {
        QuestManagerInstance = manager;
    }

    public void SetPlayer(Player player)
    {
        PlayerInstance = player;
    }

    public void GoToScene(string scene, int spawnLocation=0)
    {
        this.spawnLocation = spawnLocation;
        SceneManager.LoadScene(scene);
    }
}
