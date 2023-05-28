using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private SpriteRenderer rend;

    private Player player;
    private PlayerState currentState;

    private readonly int IsWalkingHash = Animator.StringToHash("IsWalking");

    private void Awake()
    {
        player = GetComponent<Player>();
        currentState = player.State;
    }

    private void Update()
    {
        if(player.State != currentState)
        {
            currentState = player.State;
            switch (currentState)
            {
                case PlayerState.Idle:
                    anim.SetBool(IsWalkingHash, false);
                    break;
                case PlayerState.Walking:
                    anim.SetBool(IsWalkingHash, true);
                    break;
                default:
                    break;
            }
        }
        if(player.Velocity.x != 0.0f)
        {
            float sign = Mathf.Sign(player.Velocity.x);
            rend.flipX = (sign == 1);
        }
    }
}