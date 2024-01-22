using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hellhound : EnemyCard
{
    public Hellhound() : base(Resources.Load<Sprite>("sprites/hellhound"), 2, 0, 2) { }

    public override void Action(Slot[,] board, int row, int column)
    {
        Debug.Log("The hellhound says hi");
    }
}
