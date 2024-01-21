using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Card
{
    public Sprite Sprite { get; protected set; }

    public bool Discarded {  get; private set; } = false;
    public abstract void Action();
}
