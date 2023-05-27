using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : Singleton<GameMaster>
{
    public QuestManager QuestManagerInstance { get; private set; }
    public Player PlayerInstance { get; private set; }

    

    public void SetQuestManager(QuestManager manager)
    {
        QuestManagerInstance = manager;
    }

    public void SetPlayer(Player player)
    {
        PlayerInstance = player;
    }
}
