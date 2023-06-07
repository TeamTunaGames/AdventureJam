
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TroubleMaker : MonoBehaviour
{
    [SerializeField] private GameObject target_ToMoveAwayFrom;
    [SerializeField] private GameObject wallet;
    [SerializeField] private List<GameObject> walls;
    [SerializeField] private float lineTraceLength = 1.0f;
    [SerializeField] private Vector2 thorwingPower = new Vector2(10.0f, 50.0f);

    [SerializeField] private float walletDangerThreshold;
    [SerializeField] private float troublemakerDangerThreshold;
    public float moveSpeed = 1.0f;
    private float finalMoveSpeed;

    private int MoveDirection;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Awake()
    {
        finalMoveSpeed = moveSpeed;
        MoveDirection = (transform.position.x >= target_ToMoveAwayFrom.transform.position.x) ? 1 : -1;
        currentTroublemakerState = TroublemakerState.RandomAction;
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        
        //Debug.Log("hit");

    }
    
    void FixedUpdate()
    {
        //reset
        finalMoveSpeed = moveSpeed;
        //MoveDirection = (transform.position.x >= target_ToMoveAwayFrom.transform.position.x) ? 1.0f : -1.0f;

        AI_decision();

        switch(currentTroublemakerState)
        {
            case TroublemakerState.DefendingWallet:
                OnTickDefendingWallet();
            break;

            case TroublemakerState.RunningAway:
                OnTickRunningAway();
            break;

            case TroublemakerState.RandomAction:
                OnTickRandomAction();
            break;

            case TroublemakerState.HitWall:
                OnTickHitWall();
            break;
        }

        OnTickTossingDollAround();

        transform.position += new Vector3(MoveDirection * Mathf.Abs(finalMoveSpeed), 0.0f, 0.0f);
    }

    #region TroublemakerState
    enum TroublemakerState : byte
    {
        DefendingWallet,
        RunningAway,
        RandomAction,
        HitWall
    }
    private bool IsStateLocked = false;
    TroublemakerState currentTroublemakerState;

    void SetTroublemakerState(TroublemakerState state)
    {
        if(state != currentTroublemakerState)
        {
            //refresh timer
            StopCoroutine(Timer());
            isValidTimer = true;
            isTimerRunning = false;

            
            IsStateLocked = false;
            if(state == TroublemakerState.HitWall) IsStateLocked = true;
            currentTroublemakerState = state;

        }
        
    }

    #endregion

    #region Timer

    private bool isTimerRunning = false;

    //you have to reset isValidTimer after used a timer
    private bool isValidTimer = true;

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
        IsStateLocked = false;
        isTimerRunning = false;
    }
    #endregion

    #region OnTick

    void AI_decision()
    {
        float temp_dist;

        //Top priority
        temp_dist = Mathf.Abs(target_ToMoveAwayFrom.transform.position.x - wallet.transform.position.x);
        if(temp_dist <= walletDangerThreshold)
        {
            SetTroublemakerState(TroublemakerState.DefendingWallet);
            return;
        }
        
        if(IsStateLocked)return;
        //
        temp_dist = Mathf.Abs(target_ToMoveAwayFrom.transform.position.x - transform.position.x);
        if(temp_dist <= troublemakerDangerThreshold)
        {
            SetTroublemakerState(TroublemakerState.RunningAway);
            return;
        } 

        SetTroublemakerState(TroublemakerState.RandomAction);
    }
    void OnTickDefendingWallet()
    {
        finalMoveSpeed = moveSpeed * 2;
        if(!isValidTimer)
        {
            MoveDirection = wallet.transform.position.x >= gameObject.transform.position.x ? 1 : -1;
            isValidTimer = true;
        }
        
        if(!isTimerRunning)
        {
            StartTimer(1.0f);
            
        }
        
    }

    void OnTickRunningAway()
    {
        if(!isTimerRunning)
        {
            finalMoveSpeed = moveSpeed;
            MoveDirection = (transform.position.x >= target_ToMoveAwayFrom.transform.position.x) ? 1 : -1;
            CheckWall();
        }
    }

    void OnTickRandomAction()
    {
        if(!isTimerRunning)
        {
        float direction = UnityEngine.Random.Range(-1.0f, 1.0f);
        MoveDirection = direction >=0? 1 : -1;
        float rand = UnityEngine.Random.Range(1.0f , 3.0f);
        StartTimer(rand);
        isValidTimer = true;
        }
        CheckWall();
    }

    void OnTickHitWall()
    {
        finalMoveSpeed = moveSpeed * 2.5f;
        if(!isTimerRunning)
        {
            IsStateLocked = true;
            MoveDirection = -MoveDirection;
            StartTimer(1.5f);
            isValidTimer = true;
        }
    }

    void OnTickTossingDollAround()
    {
        Vector2 box_start = transform.position;
        Vector2 box_size = GetComponent<BoxCollider2D>().size;
        Vector2 box_direction = new Vector2(MoveDirection, 0.0f);
        float box_Length = box_size.x * 0.5f;
        RaycastHit2D[]hits = Physics2D.BoxCastAll(box_start, box_size + new Vector2(0.0f, 1.0f), 0.0f, box_direction, box_Length);
        foreach(RaycastHit2D hit in hits)
        {
            TossingDollAround(hit.collider);
        }
    }
    #endregion

    void CheckWall()
    {
            Vector2 start = transform.position;
            Vector2 end = start + new Vector2(MoveDirection * lineTraceLength, 0.0f);
            RaycastHit2D[] hits = Physics2D.LinecastAll(start, end);
            foreach(RaycastHit2D hit in hits)
            {
                if(walls.Contains(hit.collider.gameObject))
                {
                    SetTroublemakerState(TroublemakerState.HitWall);
                }
            }
    }
    void TossingDollAround(Collider2D col)
    {
        Doll doll = col.gameObject.GetComponent<Doll>();
        Rigidbody2D Rig2D = col.gameObject.GetComponent<Rigidbody2D>();
        if(doll && Rig2D)
        {
            float angle = UnityEngine.Random.Range(0.0f, 90.0f);
            Vector2 angle_V2 = new Vector2(Mathf.Cos(angle) * -MoveDirection,Mathf.Sin(angle));
            float Temp_throwingPower = UnityEngine.Random.Range(thorwingPower.x, thorwingPower.y);
            Rig2D.AddForce(angle_V2 * Temp_throwingPower, ForceMode2D.Impulse);
        }
    }
}
