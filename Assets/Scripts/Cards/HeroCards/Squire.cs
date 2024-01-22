using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Squire : Card
{
    public Squire() : base(Resources.Load<Sprite>("sprites/squire"), 1) { }

    public override void Action(Slot[,] board, int row, int column)
    {
        int nearest = -1;
        // find nearest enemy
        for (int i = column; i < 6; i++)
        {
            if (board[i, row].Card is EnemyCard)
            {
                nearest = i;
                break;
            }
        }

        if (nearest == -1)
        {
            Debug.Log("Squire could not find any enemy");
            return;
        }

        EnemyCard enem = board[nearest, row].Card as EnemyCard;
        enem.Health -= Damage;
        Debug.Log("Squire hit " + board[nearest, row].Card.GetType().Name);
    }
}
