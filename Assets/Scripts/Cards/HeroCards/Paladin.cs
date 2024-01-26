using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paladin : HeroCard
{
    public bool Countering { get; set; }
    public Paladin() : base("Paladin", Resources.Load<Sprite>("sprites/paladin"), 1, 3) { }

    public override void Action(Board board, int row, int column)
    {
        Debug.Log("The paladin is making his move...");
    }

    public override void SpecialAction(Board board, Card card)
    {
        if (card == this)
        {
            Countering = true;
            Debug.Log("The paladin enters a counter stance");
        }
        else
        {
            Slot slot = board.GetSlotFromCard(this);
            Card otherCard = board.GetCardAt(slot.row, slot.position + 1);

            // if the card in front is null or a hero, the paladin counters
            if (otherCard == null || otherCard is HeroCard)
            {
                Countering = true;
                Debug.Log("There is no threat in front, so the paladin counters");
            }
            else
            {
                DamageEnemy(this, otherCard as EnemyCard);
                Debug.Log("The paladin attacks " + otherCard.GetType().Name);
            }
        }
    }
}
