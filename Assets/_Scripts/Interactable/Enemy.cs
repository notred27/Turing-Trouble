using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Interactable
{

    public override void OnInteract(State s){
        Player.Instance.isRunning = false;
    }
}
