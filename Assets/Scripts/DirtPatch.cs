using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;

public class DirtPatch : MonoBehaviour, IInteractable
{
    [field:SerializeField] public bool IsInteractable { get; private set; }

    [SerializeField] private GameObject flower;

    [QuestPopup, SerializeField] private string questName;
    [ConversationPopup, SerializeField] private string conversation;
    [DialogueEntryPopup, SerializeField] private int noFlowerDialogue;
    [DialogueEntryPopup, SerializeField] private int hasFlowerDialogue;
    [VariablePopup, SerializeField] private string flowersGrownRef;

    private bool flowerIsGrown = false;

    private static int flowersGrown = 0;

    private void Start()
    {
        QuestState state = QuestLog.GetQuestState(questName);

        if(state == QuestState.Success)
        {
            flowerIsGrown = true;
        }
        flower.SetActive(flowerIsGrown);
    }

    public void Interact()
    {
        QuestState state = QuestLog.GetQuestState(questName);
        if (state == QuestState.Active)
        {
            if (!flowerIsGrown)
            {
                flowerIsGrown = true;
                flowersGrown += 1;
                DialogueLua.SetVariable(flowersGrownRef, flowersGrown);
                flower.SetActive(true);
            }
            else
            {
                DialogueManager.StartConversation(conversation, transform, transform, hasFlowerDialogue);
            }
        }
        else
        {
            if (flowerIsGrown)
            {
                DialogueManager.StartConversation(conversation, transform, transform, hasFlowerDialogue);
            }
            else
            {
                DialogueManager.StartConversation(conversation, transform, transform, noFlowerDialogue);
            }
        }


    }
}
