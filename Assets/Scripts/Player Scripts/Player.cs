using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;

public class Player : MonoBehaviour
{
    [Header("Player stats")]
    [SerializeField] private float speed = 5.0f;
    [SerializeField] private float underGroundSpeedMod = .5f;
    [SerializeField] private float jumpPower = 15.0f;
    [SerializeField] private float jumpAccel = 12.0f;
    [SerializeField] private float fallAccel = 15.0f;
    [SerializeField] private float terminalVelocity = -20.0f;

    private PlayerState state = PlayerState.Idle;
    public PlayerState State => state;

    private Vector2 velocity = Vector2.zero;
    private float yVelocity = 0.0f;
    private float velocityMagnitude;

    public Vector3 GetVelAsVector3 { get { return new(velocity.x, 0, velocity.y); } }
    public Vector2 Velocity { get { return velocity; } }

    private PlayerInput controller;
    private PlayerInput.MainGameActions actions;


    private Camera cam;
    private CharacterController cc;
    private InteractableFinder getInteractable;

    private bool animEndedThisFrame = false;
    public bool AnimEndedThisFrame { set { animEndedThisFrame = value; } }

    private void Awake()
    {
        cc = GetComponent<CharacterController>();
        GameMaster.Instance.SetPlayer(this);
        getInteractable = GetComponent<InteractableFinder>();
    }

    private void Start()
    {
        cam = Camera.main;

        controller = ControllerManager.Instance.Controller;
        actions = controller.MainGame;
    }

    private void OnEnable()
    {
        DialogueManager.instance.conversationStarted += OnConversationStart;
        DialogueManager.instance.conversationEnded += OnCoversationEnd;
    }

    private void OnDisable()
    {
        if(DialogueManager.instance != null)
        {
            DialogueManager.instance.conversationStarted -= OnConversationStart;
            DialogueManager.instance.conversationEnded -= OnCoversationEnd;
        }
    }

    private void Update()
    {
        Vector2 input = (state != PlayerState.Cutscene) ? actions.Move.ReadValue<Vector2>() : Vector2.zero;
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
            case PlayerState.UnderGround:
                UnderGroundAction();
                break;

            case PlayerState.Jumping:
                JumpingAction();
                break;
            case PlayerState.Freefall:
                FreeFallAction();
                break;

            case PlayerState.EnteringHole:
                HoleTransitionAction();
                break;
            case PlayerState.ExitingHole:
                goto case PlayerState.EnteringHole;
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
        if (actions.Dig.triggered)
        {
            SwitchState(PlayerState.EnteringHole);
            return true;
        }
        if (actions.Interact.triggered)
        {
            if(getInteractable.Interactable != null)
            {
                getInteractable.Interactable.Interact();
            }
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

    private void UnderGroundAction()
    {
        velocity *= underGroundSpeedMod;

        if (actions.Dig.triggered)
        {
            SwitchState(PlayerState.ExitingHole);
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

    private void HoleTransitionAction()
    {
        velocity = Vector2.zero;
        if (animEndedThisFrame)
        {
            animEndedThisFrame = false;
            if (state == PlayerState.EnteringHole)
            {
                cc.height = .75f;
                cc.center = Vector3.up * .375f;
                SwitchState(PlayerState.UnderGround);
            }

            else
            {
                cc.height = 1.5f;
                cc.center = Vector3.up * .75f;
                SwitchState(PlayerState.Idle);
            }
                
        }
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

    private void OnConversationStart(Transform actor)
    {
        SwitchState(PlayerState.Cutscene);
    }

    private void OnCoversationEnd(Transform actor)
    {
        SwitchState(PlayerState.Idle);
    }
    
}

public enum PlayerState : byte
{
    Idle,
    Walking,
    UnderGround,
    
    Jumping,
    Freefall,

    EnteringHole,
    ExitingHole,

    Cutscene,
}