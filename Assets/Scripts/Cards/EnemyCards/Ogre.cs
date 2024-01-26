using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.AI;

public class Ogre : EnemyCard
{
    public Ogre() : base("Ogre", Resources.Load<Sprite>("sprites/orge"), 4, 2, 1) { }

    public override void Action(Board board, int row, int column)
    {
        if (board.GetCardAt(row, column - 1) is HeroCard)
        {
            HeroCard hero = board.GetCardAt(row, column - 1) as HeroCard;
            DamageHero(this, hero, board, row, column - 1);
        }
    }
}
