using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ArcadeCabinet : MonoBehaviour, IInteractable
{
    [field:SerializeField] public bool IsInteractable { get; private set; }
    [SerializeField] private SceneReference arcadeGame;

    public void Interact()
    {
        GameMaster.Instance.GoToScene(arcadeGame);
    }
}
