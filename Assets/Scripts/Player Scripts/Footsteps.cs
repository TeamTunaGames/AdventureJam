using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footsteps : MonoBehaviour
{
    [SerializeField] private float stepFrequency = .5f;
    [SerializeField] private AudioClip footstepSFX;

    private Player player;
    private AudioSource aSource;

    private float stepCycle = 0.0f;

    private void Awake()
    {
        player = GetComponent<Player>();
        aSource = GetComponent<AudioSource>();

        aSource.clip = footstepSFX;
    }

    private void Update()
    {
        if(player.State == PlayerState.Walking)
        {
            stepCycle += player.Velocity.magnitude * Time.deltaTime;
            if(stepCycle > stepFrequency)
            {
                aSource.PlayOneShot(footstepSFX);
                stepCycle = 0.0f;
            }
        }
        else
        {
            stepCycle = 0.0f;
        }
    }
}
