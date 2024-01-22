using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ogre : EnemyCard
{
    public Ogre() : base(Resources.Load<Sprite>("sprites/orge"), 4, 2, 1) { }

    public override void Action(Board board, int row, int column)
    {
        Card hero = board.GetCardAt(row, column - 1);
        if (hero is Card && hero is not EnemyCard)
        {
            if (hero.Defense < Damage)
            {
                Debug.Log(hero.GetType().Name + " was killed by Ogre");
            }
        }
    }
}
