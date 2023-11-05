using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : ObjectWrapper
{
    public abstract void OnInteract(State s);

}
