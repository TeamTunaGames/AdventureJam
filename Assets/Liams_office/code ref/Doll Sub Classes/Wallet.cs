using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;

public class Wallet : Doll
{
    private void Start()
    {
        if (DialogueLua.GetVariable("HasWallet").asBool)
        {
            Destroy(gameObject);
        }
    }

}
