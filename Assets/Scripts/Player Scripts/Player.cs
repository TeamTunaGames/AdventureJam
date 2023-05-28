using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed = 1.0f;

    private PlayerState state = PlayerState.Idle;
    public PlayerState State { get { return state; } }

    private Vector2 velocity = Vector2.zero;
    private float velocityMagnitude;

    public Vector3 GetVelAsVector3 { get { return new(velocity.x, 0, velocity.y); } }
    public Vector2 Velocity { get { return velocity; } }

    private PlayerInput controller;
    private PlayerInput.MainGameActions actions;


    private Camera cam;
    private CharacterController cc;
    private PlayerAnimator anim;

    private void Awake()
    {
        cc = GetComponent<CharacterController>();
        anim = GetComponent<PlayerAnimator>();

    }

    private void Start()
    {
        cam = Camera.main;

        controller = ControllerManager.Instance.Controller;
        actions = controller.MainGame;
    }

    private void Update()
    {
        Vector2 input = actions.Move.ReadValue<Vector2>();
        UpdateInput(input);

        switch (state)
        {
            case PlayerState.Idle:
                IdleAction();
                break;
            case PlayerState.Walking:
                WalkingAction();
                break;
        }

        cc.Move(GetVelAsVector3 * Time.deltaTime);
    }

    private void SwitchState(PlayerState state)
    {
        //Will add more code in the future, but for now just leave it as is
        this.state = state;
    }

    private void UpdateInput(Vector2 input)
    {
        velocityMagnitude = input.sqrMagnitude;

        Vector3 output = new(input.x, 0, input.y);
        output = cam.transform.TransformDirection(output) * speed;
        velocity = new(output.x, output.z);
    }

    private void IdleAction()
    {
        if (velocityMagnitude != 0.0f)
        {
            SwitchState(PlayerState.Walking);
            return;
        }
    }

    private void WalkingAction()
    {
        if(velocityMagnitude == 0.0f)
        {
            SwitchState(PlayerState.Idle);
            return;
        }
    }
}

public enum PlayerState : byte
{
    Idle,
    Walking,
}