using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using PixelCrushers.DialogueSystem;

public class GameMaster : Singleton<GameMaster>
{
    public QuestManager QuestManagerInstance { get; private set; }
    public Player PlayerInstance { get; private set; }

    public int spawnLocation = 0;

    [field:SerializeField] public GameObject PlayerPrefab { get; private set; }

    [SerializeField] private SceneReference theEndScreen;

    private void Start()
    {
        DialogueManager.instance.conversationEnded += CheckIfEndGame;
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }


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

    private void CheckIfEndGame(Transform actor)
    {
        if (DialogueLua.GetVariable("GameFinished").asBool)
        {
            if (DialogueLua.GetVariable("QuestsFinished").asInt == 0)
            {
                AudioManager.Instance.ChangeBGM(3);
            }
            else
            {
                AudioManager.Instance.ChangeBGM(0);
            }
            GoToScene(theEndScreen);
        }
    }
}
