using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] private int bgmID = 0;
    [SerializeField] private AudioClip[] bgmTracks;

    public int BGMID { get { return bgmID; } }

    private AudioSource bgmSource;

    private new void Awake()
    {
        base.Awake();
        bgmSource = GetComponent<AudioSource>();
    }

    public void ChangeBGM(int trackID)
    {
        if(trackID == bgmID)
        {
            return;
        }
        bgmSource.Stop();
        bgmID = trackID;
        bgmSource.clip = bgmTracks[bgmID];
        bgmSource.Play();
    }
}
