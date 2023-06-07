using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntermediaryAnimScript : MonoBehaviour
{
    [SerializeField] private Player player;

    public void SendAnimEnded()
    {
        player.AnimEndedThisFrame = true;
    }
}
