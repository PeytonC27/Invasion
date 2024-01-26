using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Farmer : HeroCard
{
    public Farmer() : base("Farmer", Resources.Load<Sprite>("sprites/farmer"), 0, 1) { }

    public override void Action(Board board, int row, int column)
    {
        Debug.Log("The farmer says hi");
    }
}
