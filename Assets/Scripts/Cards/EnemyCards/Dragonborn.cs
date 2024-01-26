using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragonborn : EnemyCard
{
    public Dragonborn() : base("Dragonborn", Resources.Load<Sprite>("sprites/dragon"), 2, 4, 1) { }

    public override void Action(Board board, int row, int column)
    {
        if (board.GetCardAt(row, column - 1) is HeroCard)
        {
            HeroCard hero = board.GetCardAt(row, column - 1) as HeroCard;
            DamageHero(this, hero, board, row, column - 1);
        }
    }
}
