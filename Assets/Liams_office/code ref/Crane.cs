using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum ClawState : byte
{
    Waiting,
    Dropping,
    Clawing,
    Returning,
}

public enum ReturingState : byte
{
    None,
    MovingUp,
    MovingRight,
    MovingBackToStartPos
}

public class Crane : MonoBehaviour
{
    [SerializeField] private SceneReference arcadeScene;

    [SerializeField] private float speed = 2.0f;
    [SerializeField] private float descentSpeed = 5.0f;

    private readonly int IsOpening = Animator.StringToHash("IsOpening");

    [SerializeField] private Animator anim;

    [SerializeField] private Vector2 GrabingArea = new Vector2(1.0f, 4.0f);

    [SerializeField] private List<GameObject> IgnoreColls;

    public float droppingExpirationTimer = 4.0f;

    public Vector2 chanceToDropRandomTimer = new Vector2(0.25f, 0.75f);
    public Vector2 chaceToDropThreshhold = new Vector2(0.5f, 0.5f);
    private bool isTimerRunning = false;
    private bool isValidTimer = true;
    RaycastHit2D[] hits;
    private PlayerInput.MainGameActions actions;

    private bool isColliding;
    public Action<Vector2> OnCraneGrab;

    private Vector2 ClawIdlePos;

    [SerializeField] private ClawState currentClawState = ClawState.Waiting;
    [SerializeField] private ReturingState currentReturingState = ReturingState.None;

    void start()
    {

    }
    private void Awake()
    {
        foreach(GameObject IgnoreCol in IgnoreColls)
        {
            Physics2D.IgnoreCollision(IgnoreCol.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }
        ClawIdlePos = transform.position;
        actions = ControllerManager.Instance.Actions;
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        //when user press interacting buttom
        if (actions.Interact.triggered && currentClawState == ClawState.Waiting)
        {
            currentClawState = ClawState.Dropping;
        }
        if(actions.Dig.triggered && currentClawState == ClawState.Waiting)
        {
            GameMaster.Instance.GoToScene(arcadeScene, 1);
        }
    }

    private void FixedUpdate()
    {
        switch(currentClawState)
        {
            case ClawState.Waiting:

                OnWaitingTick();
                break;

            case ClawState.Dropping:

                OnDroppingTick();
                break;

            case ClawState.Clawing:

                //OnCllisionEnter2D() will handle this
                break;

            case ClawState.Returning:
                OnReturningTick();
                break;
        }
        
        
        
        isColliding = false;
    }

    // called when the cube hits the floor
    void OnCollisionEnter2D(Collision2D col)
    {
        isColliding = true;
        //Debug.Log("hit");

    }


    #region  claw state
        void OnWaitingTick()
        {   
            isValidTimer = true;
            ResetRidgidbodyForce();
            Vector2 move = actions.Move.ReadValue<Vector2>();

            transform.position += new Vector3(move.x, 0, 0) * (speed * Time.deltaTime);

            //waiting for user interaction input Check
        }

        void OnDroppingTick()
        {
            if(isValidTimer) StartTimer(droppingExpirationTimer);
            transform.position += Vector3.down * (descentSpeed * Time.deltaTime);

            Vector2 startPos = transform.position + new Vector3(0.0f, -GrabingArea.y - 0.01f);
            Vector2 Box = GrabingArea;
            hits = Physics2D.BoxCastAll(startPos, Box, 0, Vector2.down, 0);
            if(hits.Length > 0)
            {
                foreach(RaycastHit2D hit in hits)
                {   
                    Component crane = hit.collider.gameObject.GetComponent<Crane>();
                    Component doll = hit.collider.gameObject.GetComponent<Doll>();
                    if((!crane && !doll ) || !isTimerRunning) 
                    {
                        isValidTimer = true;
                        SetClawState(ClawState.Clawing);
                        anim.SetBool(IsOpening, false);
                    }
                }
            }
            if(transform.position.y <= -1.0f)
            {
                transform.position = new Vector3(transform.position.x, -1.0f, 0);
                OnCraneGrab?.Invoke(transform.position);
            }

            //OnCollisionEnter2D will handle change claw state
        }

        void OnClawing()
        {
            foreach(RaycastHit2D hit in hits)
            {
                Component doll = hit.collider.gameObject.GetComponent<Doll>();
                if(doll)
                {
                    //nasty code don't look at it
                    TroubleMaker aaa = hit.collider.gameObject.GetComponent<TroubleMaker>();
                    if(aaa) aaa.enabled = false;
                    //
                    doll.GetComponent<Rigidbody2D>().isKinematic = true;
                    doll.transform.SetParent(gameObject.transform);
                }
            }

            SetClawState(ClawState.Returning);
            ResetRidgidbodyForce();
        }

        void OnReturningTick()
        {
            if(isValidTimer) 
            {
                SetReturningState(ReturingState.MovingUp);
                StartTimer(UnityEngine.Random.Range(chanceToDropRandomTimer.x , chanceToDropRandomTimer.y));
            }
            
            if(!isTimerRunning) ChanceToDrop();

            switch(currentReturingState)
            {
                case ReturingState.None:
                    //tick should never reach this state
                    anim.SetBool(IsOpening, true);
                break;

                case ReturingState.MovingUp:
                    if(transform.position.y < ClawIdlePos.y)
                    {   
                        transform.position += Vector3.up * (descentSpeed * Time.deltaTime);
                    }
                    else
                    {
                        SetReturningState(ReturingState.MovingRight);
                    }
                break;

                case ReturingState.MovingRight:
                    transform.position += Vector3.right * (speed * Time.deltaTime);
                    if(isColliding && transform.position.x >= 5.5f)
                    {
                        //anim notify fire RealsingItems()
                        //RealsingItems() handles SetReturningState(ReturingState.MovingBackToStartPos);
                        anim.SetBool(IsOpening, true);
                    }
                break;

                case ReturingState.MovingBackToStartPos:
                    transform.position -= Vector3.right * (speed * Time.deltaTime);
                    if(transform.position.x <= ClawIdlePos.x)
                    {
                        SetClawState(ClawState.Waiting);
                        SetReturningState(ReturingState.None);
                        ResetRidgidbodyForce();
                        return;
                    }
                break;
            }
        }
    #endregion
    
    void ChanceToDrop()
    {
        foreach(Transform child in transform)
        {
            Rigidbody2D child_Ridgidbody2D = child.GetComponent<Rigidbody2D>();

            float DistanceFromX_Center = MathF.Abs(child.localPosition.x);
            //Debug.Log(DistanceFromX_Center);
            float DropingThreshhold = UnityEngine.Random.Range(chaceToDropThreshhold.x, chaceToDropThreshhold.y);
            if(DistanceFromX_Center <= DropingThreshhold) continue;
            if(child_Ridgidbody2D)
            {
                child_Ridgidbody2D.isKinematic = false;
            }
            //nasty code don't look at it
            TroubleMaker aaa = child.gameObject.GetComponent<TroubleMaker>();
            if(aaa) aaa.enabled = true;
            //
            child.parent = null;
            
        }
    }

    void RealsingItems()
    {
        foreach(Transform child in transform)
        {
            Rigidbody2D child_Ridgidbody2D = child.GetComponent<Rigidbody2D>();
            if(child_Ridgidbody2D)
            {
                child_Ridgidbody2D.isKinematic = false;
            }
        }
        transform.DetachChildren();
        SetReturningState(ReturingState.MovingBackToStartPos);
        ResetRidgidbodyForce();
    }

    void SetClawState(ClawState clawState)
    {
        currentClawState = clawState;
    }

    void SetReturningState(ReturingState returingState)
    {
        currentReturingState = returingState;
    }

    void StartTimer(float waitTime)
    {
        if(isTimerRunning || !isValidTimer)return;
        //you have to reset isValidTimer at some point manually
        isValidTimer = false;
        StartCoroutine(Timer(waitTime));
    }

    IEnumerator Timer(float waitTime = 1.0f)
    {
        isTimerRunning = true;
        yield return new WaitForSeconds(waitTime);
        isTimerRunning = false;
    }

    private void ResetRidgidbodyForce()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(0f,0f); 
        GetComponent<Rigidbody2D>().angularVelocity = 0.0f;
        transform.rotation = Quaternion.Euler(new Vector3(0f,0f,0f));
    }

    #if UNITY_EDITOR
    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, new Vector3(2, 2, 1));
    }
    #endif
    
}
