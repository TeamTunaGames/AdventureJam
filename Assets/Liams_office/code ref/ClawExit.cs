using PixelCrushers.DialogueSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClawExit : MonoBehaviour
{
    
    public Action<Doll> OnDollEnterClawMachineExit;

    void OnTriggerEnter2D(Collider2D col)
    {
        Doll doll = col.gameObject.GetComponent<Doll>();
        if(doll.GetType() == typeof(Wallet))
        {
            DialogueLua.SetVariable("HasWallet", true);
        }
        OnDollEnterClawMachineExit?.Invoke(doll);
        Destroy(col.gameObject);
    }
}
