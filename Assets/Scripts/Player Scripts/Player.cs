using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player stats")]
    [SerializeField] private float speed = 1.0f;
    [SerializeField] private float jumpPower = 15.0f;
    [SerializeField] private float jumpAccel = 12.0f;
    [SerializeField] private float fallAccel = 15.0f;
    [SerializeField] private float terminalVelocity = -20.0f;

    private PlayerState state = PlayerState.Idle;
    public PlayerState State { get { return state; } }

    private Vector2 velocity = Vector2.zero;
    private float yVelocity = 0.0f;
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

        AffectedGravity();

        switch (state)
        {
            case PlayerState.Idle:
                IdleAction();
                break;
            case PlayerState.Walking:
                WalkingAction();
                break;
            case PlayerState.Jumping:
                JumpingAction();
                break;
            case PlayerState.Freefall:
                FreeFallAction();
                break;
            default:
                break;
        }

        cc.Move(CalculateVelocity() * Time.deltaTime);
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

    private Vector3 CalculateVelocity()
    {
        return new Vector3(velocity.x, yVelocity, velocity.y);
    }

    private void AffectedGravity()
    {
        if (cc.isGrounded)
        {
            yVelocity = -1.0f;
            return;
        }
        else
        {
            switch (state)
            {
                case PlayerState.Jumping:
                    yVelocity = Mathf.MoveTowards(yVelocity, terminalVelocity, jumpAccel * Time.deltaTime);
                    break;
                case PlayerState.Freefall:
                    yVelocity = Mathf.MoveTowards(yVelocity, terminalVelocity, fallAccel * Time.deltaTime);
                    break;
                default:
                    break;
            }
        }
    }

    private bool CheckCommonGroundCancels()
    {
        if (actions.Jump.triggered)
        {
            PlayerJump();
            return true;
        }
        if (!cc.isGrounded)
        {
            SwitchState(PlayerState.Freefall);
            return true;
        }

        return false;
    }

    private void IdleAction()
    {
        if(CheckCommonGroundCancels())
            return;

        if (velocityMagnitude != 0.0f)
        {
            SwitchState(PlayerState.Walking);
            return;
        }
    }

    private void WalkingAction()
    {
        if (CheckCommonGroundCancels())
            return;

        if(velocityMagnitude == 0.0f)
        {
            SwitchState(PlayerState.Idle);
            return;
        }
    }

    private void PlayerJump()
    {
        yVelocity = jumpPower;
        SwitchState(PlayerState.Jumping);
    }

    private bool CheckCommonAirCancel()
    {
        if (cc.isGrounded)
        {
            if (velocity != Vector2.zero)
                SwitchState(PlayerState.Walking);
            else
                SwitchState(PlayerState.Idle);

            return true;
        }

        return false;
    }

    private void FreeFallAction()
    {
        if (CheckCommonAirCancel())
            return;
    }

    private void JumpingAction()
    {
        if (CheckCommonAirCancel())
            return;

        if(yVelocity < 0.0f)
        {
            SwitchState(PlayerState.Freefall);
            return;
        }
    }
}

public enum PlayerState : byte
{
    Idle,
    Walking,
    
    Jumping,
    Freefall,
}