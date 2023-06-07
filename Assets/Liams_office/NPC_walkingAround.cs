using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class NPC_walkingAround : MonoBehaviour
{

    public float moveSpeed;
    public Vector2 AddRandMoveSpeed;
    public float AllowMovingRadius;



    private Vector3 start_location;
    
    private Vector3 TargetLocation;


    private float finalMoveSpeed;
    private bool IsMoving;
    private bool IsEnable = false;


    private List<Action> ListRandomTasks = new List<Action>();



    void Start()
    {
        //init
        ListRandomTasks.Add(StartTimeWait);
        ListRandomTasks.Add(SetRandomTargetLocation);
        start_location = gameObject.transform.position;
        SetRandomTargetLocation();

        OnEnable();
    }

    // Update is called once per frame
    void Update()
    {
        if(!IsEnable)return;
        Physics.gravity = new Vector3(0, -1.0F, 0);
        // Check if the position of the NPC and Target Location are approximately equal.
        if (Vector3.Distance(transform.position, TargetLocation) > 0.001f)
        {
            IsMoving = true;
            var step =  finalMoveSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, TargetLocation, step);
            if(!CheckIsAtEdge())
            {
                TargetLocation = transform.position;
            }

        }
        else
        {
            if(IsMoving)
            {
                StartCoroutine(WaitTimeAndSetRandomNewTask(3.0f));
            }
            IsMoving = false;
        }
    }



    private void SetNewTask()
    {
        if(ListRandomTasks.Count == 0){OnDisable(); return;}
        int randomIndexOfTask = Random.Range(0, ListRandomTasks.Count);
        ListRandomTasks[randomIndexOfTask].Invoke();
    }

    //don't forget to add new tasks to ListRandomTasks
    #region NPC_Tasks
        
        void StartTimeWait()
        {
            StartCoroutine(WaitTimeAndSetRandomNewTask());
        }
        void SetRandomTargetLocation()
        {
            finalMoveSpeed = moveSpeed + Random.Range(AddRandMoveSpeed.x, AddRandMoveSpeed.y);

            Vector2 Pos2 = Random.insideUnitCircle *  AllowMovingRadius * 0.5f;
            Vector3 Pos3 = new Vector3(Pos2.x, 0 ,Pos2.y);
            TargetLocation = start_location + Pos3;

            transform.LookAt(TargetLocation);
        }

    #endregion

    #region Timer
        IEnumerator WaitTimeAndSetRandomNewTask(float waitTime = 1.0f)
        {
            yield return new WaitForSeconds(waitTime);
            SetNewTask();
        }

    #endregion

    bool CheckIsAtEdge()
    {

        Vector3 startPos = transform.position + transform.forward * 0.25f;
        Vector3 Floor = -Vector3.up * 3;
        RaycastHit hit;
        if (Physics.Raycast( startPos, Floor, out hit))
        {
            return true;
        }


        return false;
    }

    void OnEnable()
    {
        IsEnable = true;
        SetNewTask();
    }

    

    void OnDisable()
    {   
        IsEnable = false;
        StopAllCoroutines();
    }
}
