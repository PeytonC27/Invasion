using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin : EnemyCard
{
    public Goblin() : base("Goblin", Resources.Load<Sprite>("sprites/goblin"), 2, 1, 1) { }

    public override void Action(Board board, int row, int column)
    {
        if (board.GetCardAt(row, column - 1) is HeroCard)
        {
            HeroCard hero = board.GetCardAt(row, column - 1) as HeroCard;
            UpdateGobDamage(board);
            DamageHero(this, hero, board, row, column - 1);
        }
    }

    public void UpdateGobDamage(Board board)
    {
        Damage = board.Count(c => c is Goblin);
    }
}
