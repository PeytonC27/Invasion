using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trickster : Card
{
    public Trickster() : base(Resources.Load<Sprite>("sprites/trickster"), 0) { }

    public override void Action(Slot[,] board, int row, int column)
    {
        Debug.Log("Trickster says hi");
    }
}
