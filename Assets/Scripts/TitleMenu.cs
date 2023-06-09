using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleMenu : MonoBehaviour
{
    [SerializeField] private GameObject guideMenu;
    [SerializeField] private SceneReference scene;

    private void Start()
    {
        if(guideMenu != null)
            guideMenu.SetActive(false);
    }

    public void PlayClick()
    {
        guideMenu.SetActive(true);
    }

    public void QuitClicked()
    {
        Application.Quit();
    }

    public void ConfirmClick()
    {
        AudioManager.Instance.ChangeBGM(0);
        SceneManager.LoadScene(scene);
    }
}
