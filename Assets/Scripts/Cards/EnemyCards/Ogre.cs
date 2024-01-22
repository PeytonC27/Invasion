using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ogre : EnemyCard
{
    public Ogre() : base(Resources.Load<Sprite>("sprites/orge"), 4, 2, 1) { }

    public override void Action(Slot[,] board, int row, int column)
    {
        if (board[column - 1, row].Card is Card && board[column - 1, row].Card is not EnemyCard)
        {
            Card hero = board[column - 1, row].Card;
            if (hero.Defense < Damage)
                Debug.Log(hero.GetType().Name + " was killed by Ogre");
        }
    }
}
