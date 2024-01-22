using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trickster : HeroCard
{
    public Trickster() : base(Resources.Load<Sprite>("sprites/trickster"), 0, 1) { }

    public override void Action(Board board, int row, int column)
    {
        Debug.Log("Trickster says hi");
    }
}
