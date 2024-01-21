using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squire : Card
{
    public Squire()
    {
        this.Sprite = Resources.Load<Sprite>("sprites/squire");
    }
    public override void Action()
    {
        throw new System.NotImplementedException();
    }
}
