using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ShieldBearer : HeroCard
{
    public ShieldBearer() : base(Resources.Load<Sprite>("sprites/shield_bearer"), 1, 2) { }

    public override void Action(Board board, int row, int column)
    {
        Card otherCard = board.GetCardAt(row, column + 1);
        if (otherCard is EnemyCard)
        {
            EnemyCard enem = otherCard as EnemyCard;
            enem.Health -= (Damage + DamageBuff);
            Debug.Log("Hit " + otherCard.GetType().Name);

            if (enem.Health <= 0)
                Debug.Log("Shield Bearer killed " + board.GetCardAt(row, column + 1).GetType().Name + "!");
        }
    }
}
