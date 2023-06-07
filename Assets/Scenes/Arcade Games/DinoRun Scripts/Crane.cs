using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crane : MonoBehaviour
{
    [SerializeField] private float speed = 2.0f;
    [SerializeField] private float descentSpeed = 5.0f;

    private PlayerInput.MainGameActions actions;
    private bool descending = false;

    public Action<Vector2> OnCraneGrab;

    private void Awake()
    {
        actions = ControllerManager.Instance.Actions;
    }

    private void Update()
    {
        if (!descending)
        {
            Vector2 move = actions.Move.ReadValue<Vector2>();

            transform.position += new Vector3(move.x, 0, 0) * (speed * Time.deltaTime);
            float clampX = Mathf.Clamp(transform.position.x, -8.0f, 8.0f);
            transform.position = new Vector3(clampX, transform.position.y, 0);
        }
        else
        {
            transform.position += Vector3.down * (descentSpeed * Time.deltaTime);

            if(transform.position.y <= -1.0f)
            {
                transform.position = new Vector3(transform.position.x, -1.0f, 0);
                OnCraneGrab?.Invoke(transform.position);
            }
        }

        if (actions.Interact.triggered)
        {
            descending = true;
        }
    }
}
