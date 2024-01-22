using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin : EnemyCard
{
    public Goblin() : base(Resources.Load<Sprite>("sprites/goblin"), 2, 1, 1) { }

    public override void Action(Board board, int row, int column)
    {
        int tempDamage = board.Count(c => c is Goblin);
        Card heroCard = board.GetCardAt(row, column - 1);

        if (heroCard is Card && heroCard is not EnemyCard)
        {
            if (heroCard.Defense < tempDamage)
                Debug.Log(heroCard.GetType().Name + " was killed by Ogre");
        }
    }
}
