using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    [field:SerializeField] public bool IsInteractable { get; private set; }
    [SerializeField] private SceneReference scene;
    [SerializeField] private int spawnLocation = 0;

    public void Interact()
    {
        GameMaster.Instance.GoToScene(scene, spawnLocation);
    }
}
