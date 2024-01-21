using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trickster : Card
{
    public Trickster()
    {
        this.Sprite = Resources.Load<Sprite>("sprites/trickster");
    }
    public override void Action()
    {
        throw new System.NotImplementedException();
    }
}
