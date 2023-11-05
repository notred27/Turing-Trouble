using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : Interactable
{
    public override void OnInteract(State s){
        GameManager.Instance.AddToCoins(1);
    }
}