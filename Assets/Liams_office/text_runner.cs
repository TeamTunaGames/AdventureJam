using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class text_runner : MonoBehaviour
{
   
    public string dialogue = "";
    public float Interval_in_sec = 0.5f;

    public event Action<string> onSpeaking;
    public event Action onFinishedSpeaking;
    

    private int currentTextIndex = -1;



    private void Speaking(string input_text)
    {

        string result= "";
        currentTextIndex += 1; //text iteration happens here

        result = input_text.Substring(0, currentTextIndex);
        onSpeaking?.Invoke(result);
        Debug.Log(result);

        return;
    }

    private IEnumerator Timer()
    {
        while(currentTextIndex < dialogue.Length - 1)
        {
            yield return new WaitForSeconds(Interval_in_sec);
            Speaking(dialogue);
        }
        onFinishedSpeaking?.Invoke();
        Debug.Log("finished!");
    }

    public void StartSpeaking()
    {
        if(dialogue != null)
        {
            StartCoroutine(Timer());
        }
        
    }

    private void OnEnable()
    {
        
    }
    void Start()
    {
        StartSpeaking();
    }
}
