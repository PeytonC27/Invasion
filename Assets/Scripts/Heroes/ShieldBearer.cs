using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldBearer : Card
{
    public ShieldBearer()
    {
        this.Sprite = Resources.Load<Sprite>("sprites/shield_bearer");
    }

    public override void Action()
    {
        throw new System.NotImplementedException();
    }
}
