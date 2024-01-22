using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin : EnemyCard
{
    public Goblin() : base(Resources.Load<Sprite>("sprites/goblin"), 2, 1, 1) { }

    public override void Action(Board board, int row, int column)
    {
        int tempDamage = board.Count(c => c is Goblin);

        if (board.GetCardAt(row, column - 1) is HeroCard)
        {
            HeroCard hero = board.GetCardAt(row, column - 1) as HeroCard;
            if (hero.Defense < tempDamage)
            {
                Debug.Log(hero.GetType().Name + " was killed by Goblin!");
                board.RemoveCardAt(row, column - 1);
            }
        }
    }
}
