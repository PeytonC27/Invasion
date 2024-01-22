using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cleric : HeroCard
{
    public Cleric() : base(Resources.Load<Sprite>("sprites/cleric"), 0, 1) { }

    public override void Action(Board board, int row, int column)
    {
        Debug.Log("The cleric said hi");
    }
}
