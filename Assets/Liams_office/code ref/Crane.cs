using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ClawState
{
    Waiting,
    Dropping,
    Clawing,
    Returning
}

public class Crane : MonoBehaviour
{
    [SerializeField] private float speed = 2.0f;
    [SerializeField] private float descentSpeed = 5.0f;

    private PlayerInput.MainGameActions actions;

    public Action<Vector2> OnCraneGrab;

    private Vector2 ClawIdlePos;

    public ClawState currentClawState = ClawState.Waiting;
    private void Awake()
    {
        ClawIdlePos = transform.position;
        actions = ControllerManager.Instance.Actions;
    }

    private void Update()
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
        
        //when user press interacting buttom
        if (actions.Interact.triggered & currentClawState == ClawState.Waiting)
        {
            currentClawState = ClawState.Dropping;
        }
    }

    // called when the cube hits the floor
    void OnCollisionEnter2D(Collision2D col)
    {
        if(currentClawState == ClawState.Dropping)
        {
            SetClawState(ClawState.Clawing);
            OnClawing(col.gameObject);
        }
        //Debug.Log("123" + col.gameObject.ToString());
    }

    #region  claw state
        void OnWaitingTick()
        {
            Vector2 move = actions.Move.ReadValue<Vector2>();

            transform.position += new Vector3(move.x, 0, 0) * (speed * Time.deltaTime);
            float clampX = Mathf.Clamp(transform.position.x, -8.0f, 8.0f);
            transform.position = new Vector3(clampX, transform.position.y, 0);

            //waiting for user interaction input Check
        }

        void OnDroppingTick()
        {
            transform.position += Vector3.down * (descentSpeed * Time.deltaTime);

            if(transform.position.y <= -1.0f)
            {
                transform.position = new Vector3(transform.position.x, -1.0f, 0);
                OnCraneGrab?.Invoke(transform.position);
            }

            //OnCollisionEnter2D will handle change claw state
        }

        void OnClawing(GameObject doll)
        {
            if(doll.GetComponent<Doll>())
            {
                doll.GetComponent<Rigidbody2D>().isKinematic = true;
                doll.transform.SetParent(gameObject.transform);
            }

            SetClawState(ClawState.Returning);
        }

        void OnReturningTick()
        {
            if(transform.position.y < ClawIdlePos.y)
            {
                transform.position += Vector3.up * (descentSpeed * Time.deltaTime);
                return;
            }
            
            if(transform.childCount > 0)
            {
                transform.position += Vector3.right * (speed * Time.deltaTime);
                if(transform.position.x >= 8.0f)
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
                    return;
                }
            }
            
            if(transform.childCount == 0)
            {
                transform.position -= Vector3.right * (speed * Time.deltaTime);
                if(transform.position.x <= ClawIdlePos.x)
                {
                    SetClawState(ClawState.Waiting);
                    return;
                }
            }
        }
    #endregion
    

    void SetClawState(ClawState clawState)
    {
        currentClawState = clawState;
    }
}
