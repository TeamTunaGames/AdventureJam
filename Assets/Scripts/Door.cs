using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour, IInteractable
{
    [field:SerializeField] public bool IsInteractable { get; private set; }
    public SceneReference scene;

    public void Interact()
    {
        SceneManager.LoadScene(scene);
    }
}
