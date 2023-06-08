using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableIndicator : MonoBehaviour
{
    private Player player;
    private InteractableFinder getInteractable;
    private SpriteRenderer rend;

    private void Awake()
    {
        rend = GetComponent<SpriteRenderer>();
        getInteractable = GetComponentInParent<InteractableFinder>();
        player = GetComponentInParent<Player>();
    }

    private void Start()
    {
        rend.enabled = false;
    }

    private void Update()
    {
        if (player.State == PlayerState.Idle || player.State == PlayerState.Walking)
            rend.enabled = getInteractable.Interactable != null;
        else
            rend.enabled = false;
    }
}
