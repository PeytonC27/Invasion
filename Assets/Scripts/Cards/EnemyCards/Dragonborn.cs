using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragonborn : EnemyCard
{
    public Dragonborn() : base(Resources.Load<Sprite>("sprites/dragon"), 2, 4, 1) { }

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
