using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Win : Interactable
{
    public override void OnInteract(State s)
    {
        Player.Instance.isRunning = false;
    }
}
