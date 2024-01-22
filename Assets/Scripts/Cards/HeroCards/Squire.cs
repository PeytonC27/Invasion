using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Squire : Card
{
    public Squire() : base(Resources.Load<Sprite>("sprites/squire"), 1) { }

    public override void Action(Board board, int row, int column)
    {
        int nearest = -1;
        // find nearest enemy
        for (int i = column; i < 6; i++)
        {
            if (board.GetCardAt(row, i) is EnemyCard)
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

        EnemyCard enem = board.GetCardAt(row, nearest) as EnemyCard;
        enem.Health -= Damage;
        Debug.Log("Squire hit " + board.GetCardAt(row, nearest).GetType().Name);

        if (enem.Health <= 0)
            Debug.Log("Squire killed " + board.GetCardAt(row, nearest).GetType().Name + "!");
    }
}
