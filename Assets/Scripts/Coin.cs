using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;

public class Coin : MonoBehaviour, IInteractable
{
    [QuestPopup, SerializeField] private string quest;
    [ConversationPopup, SerializeField] private string conversation;
    [DialogueEntryPopup, SerializeField] private int unassignedPrompt;
    [DialogueEntryPopup, SerializeField] private int assignedPrompt;

    [field:SerializeField] public bool IsInteractable { get; private set; }

    private void Start()
    {
        if (QuestLog.GetQuestState(quest) == QuestState.Success || QuestLog.GetQuestState(quest) == QuestState.Failure)
        {
            Destroy(gameObject);
        }
    }

    public void Interact()
    {
        if(QuestLog.GetQuestState(quest) != QuestState.Active)
        {
            if(DialogueLua.GetVariable("TalkedToCollector").asBool)
                DialogueManager.StartConversation("Flower Quest", transform, transform, 6);
            else
                DialogueManager.StartConversation("Flower Quest", transform, transform, unassignedPrompt);
        }
        else
        {
            DialogueManager.StartConversation("Flower Quest", transform, transform, assignedPrompt);
            Destroy(gameObject);
        }
    }
}
