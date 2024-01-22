using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin : EnemyCard
{
    public Goblin() : base(Resources.Load<Sprite>("sprites/goblin"), 2, 1, 1) { }

    public override void Action(Slot[,] board, int row, int column)
    {
        int tempDamage = 1;

        for (int i = 0; i < board.GetLength(0); i++)
            for (int j = 0; j < board.GetLength(1); j++)
                if (board[i, j].Card is Goblin)
                    tempDamage++;

        if (board[column - 1, row].Card is Card && board[column - 1, row].Card is not EnemyCard)
        {
            Card hero = board[column - 1, row].Card;
            if (hero.Defense < tempDamage)
                Debug.Log(hero.GetType().Name + " was killed by Ogre");
        }
    }
}
