using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerManager : Singleton<ControllerManager>
{
    public PlayerInput Controller { get; private set; }
    public PlayerInput.MainGameActions Actions { get; private set; }

    

    private void Start()
    {
        if (Controller == null)
        {
            CreateController();
        }
    }

    private void OnEnable()
    {
        if(Controller != null)
        {
            Controller.Enable();
        }
        else
        {
            CreateController();
            Controller.Enable();
        }
    }

    private void OnDisable()
    {
        if(Controller != null)
        {
            Controller.Disable();
        }
    }

    private void OnDestroy()
    {
        if(Controller != null)
        {
            Controller.Dispose();
        }
    }

    private void CreateController()
    {
        Controller = new();
        Actions = Controller.MainGame;
    }

}
