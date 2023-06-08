using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;

public class Door : MonoBehaviour, IInteractable
{
    [field:SerializeField] public bool IsInteractable { get; private set; }
    [SerializeField] private SceneReference scene;
    [SerializeField] private int spawnLocation = 0;

    public void Interact()
    {
        if (DialogueLua.GetVariable("InAQuest").AsBool)
            DialogueManager.StartConversation("Flower Quest", transform, transform, 3);
        
        else
            GameMaster.Instance.GoToScene(scene, spawnLocation);
    }
}
