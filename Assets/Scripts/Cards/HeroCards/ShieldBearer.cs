using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ShieldBearer : HeroCard
{
    public ShieldBearer() : base("Shield Bearer", Resources.Load<Sprite>("sprites/shield_bearer"), 1, 1) { }

    public override void Action(Board board, int row, int column)
    {
        Card otherCard = board.GetCardAt(row, column + 1);
        if (otherCard is EnemyCard)
        {
            EnemyCard enem = otherCard as EnemyCard;
            DamageEnemy(this, enem);
        }
    }
}
