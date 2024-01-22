using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ShieldBearer : Card
{
    public ShieldBearer() : base(Resources.Load<Sprite>("sprites/shield_bearer"), 1) { }

    public override void Action(Slot[,] board, int row, int column)
    {
        if (board[column + 1, row].Card is EnemyCard)
        {
            EnemyCard enem = board[column + 1, row].Card as EnemyCard;
            enem.Health -= Damage;
            Debug.Log("Hit " + board[column + 1, row].Card.GetType().Name);
        }
    }
}
