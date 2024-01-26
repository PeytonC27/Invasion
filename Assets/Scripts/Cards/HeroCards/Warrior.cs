using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warrior : HeroCard
{
    public Warrior() : base("Warrior", Resources.Load<Sprite>("sprites/warrior"), 2, 1) { }

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
            Debug.Log("Warrior could not find any enemy");
            return;
        }

        // add damage if an enemy is close
        if (nearest == column + 1)
            Damage++;

        EnemyCard enem = board.GetCardAt(row, nearest) as EnemyCard;
        DamageEnemy(this, enem);

        // remove the temp extra damage
        if (nearest == column + 1)
            Damage--;
    }
}
