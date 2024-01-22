using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragonborn : EnemyCard
{
    public Dragonborn() : base(Resources.Load<Sprite>("sprites/dragon"), 2, 4, 1) { }

    public override void Action(Board board, int row, int column)
    {
        Card otherCard = board.GetCardAt(row, column - 1);
        if (otherCard is Card && otherCard is not EnemyCard)
        {
            if (otherCard.Defense < Damage)
                Debug.Log(otherCard.GetType().Name + " was killed by Ogre");
        }
    }
}
