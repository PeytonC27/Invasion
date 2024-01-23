using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trickster : HeroCard
{
    public Trickster() : base(Resources.Load<Sprite>("sprites/trickster"), 0, 1) { }

    public override void Action(Board board, int row, int column)
    {
        for (int i = column; i < 6; i++)
        {
            if (board.GetCardAt(row, i) is HeroCard && board.GetCardAt(row, i) is not Trickster)
            {
                HeroCard hero = board.GetCardAt(row, i) as HeroCard;
                hero.DamageBuff++;
                Debug.Log("The trickster empowered the " + hero.GetType().Name);
            }
        }
    }
}
