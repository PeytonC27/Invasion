using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cleric : Card
{
    public Cleric()
    {
        this.Sprite = Resources.Load<Sprite>("sprites/cleric");
    }

    public override void Action()
    {
        throw new System.NotImplementedException();
    }
}
