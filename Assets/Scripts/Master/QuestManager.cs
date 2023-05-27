using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : Singleton<QuestManager>
{


    private void Start()
    {
        GameMaster.Instance.SetQuestManager(this);
    }
}
