using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed = 1.0f;

    private PlayerInput controller;
    private PlayerInput.MainGameActions actions;
    private CharacterController cc;

    private void Awake()
    {
        cc = GetComponent<CharacterController>();
    }

    private void Start()
    {
        controller = ControllerManager.Instance.Controller;
        actions = ControllerManager.Instance.Actions;
    }

    private void Update()
    {
        Vector2 input = actions.Move.ReadValue<Vector2>();

        cc.Move(new Vector3(input.x, 0, input.y) * (speed * Time.deltaTime));
    }

    
}

public enum PlayerState : byte
{
    Idle,
    Walking,
}