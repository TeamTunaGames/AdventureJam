using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DinoCanvas : MonoBehaviour
{
    [SerializeField] private GameObject PlayerObject;
    private DinoController dinoPlayerController;
    [SerializeField] private TextMeshProUGUI ScoreBoard;
    [SerializeField] private TextMeshProUGUI EndPlayDisply;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Awake()
    {
        dinoPlayerController = PlayerObject.GetComponent<DinoController>();
        dinoPlayerController.NotifyScore += UpdateScore;
        dinoPlayerController.NotifyWin += WinResult;
        dinoPlayerController.NotifyLoss += LossResult;
        EndPlayDisply.enabled = false;
    }

    void UpdateScore(int score)
    {
        ScoreBoard.SetText(score.ToString());
    }

    void WinResult()
    {
        EndPlayDisply.SetText("Win!");
        EndPlayDisply.enabled = true;
    }

    void LossResult()
    {
        EndPlayDisply.SetText("Better luck next time!");
        EndPlayDisply.enabled = true;
    }

    void EndPlatResult(string text)
    {
        EndPlayDisply.SetText(text);
        EndPlayDisply.enabled = true;
    }
    void Update()
    {
        
    }
}
