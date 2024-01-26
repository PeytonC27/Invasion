using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mage : HeroCard
{
    public Mage() : base("Mage", Resources.Load<Sprite>("sprites/mage"), 1, 1) { }

    public override void Action(Board board, int row, int column)
    {
        Debug.Log("The mage is looking for a unit to protect or attack...");
    }

    public override void SpecialAction(Board board,Card card)
    {
        if (card is EnemyCard)
        {
            EnemyCard enemy = card as EnemyCard;
            DamageEnemy(this, enemy);
        }
        else if (card is HeroCard)
        {
            HeroCard hero = card as HeroCard;
            Debug.Log(hero.Name + " is protected from damage!");
            hero.Protected = true;
        }
    }
}
