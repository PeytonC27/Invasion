using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cleric : Card
{
    public Cleric() : base(Resources.Load<Sprite>("sprites/cleric"), 0) { }

    public override void Action(Slot[,] board, int row, int column)
    {
        Debug.Log("The cleric said hi");
    }
}
