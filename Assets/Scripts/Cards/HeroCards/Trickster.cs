using UnityEngine;

public class Trickster : HeroCard
{
    public Trickster() : base("Trickster", Resources.Load<Sprite>("sprites/trickster"), 0, 1) { }

    public override void Action(Board board, int row, int column)
    {
        //
        for (int i = column; i < 6; i++)
        {
            Card card = board.GetCardAt(row, i);
            if (card is HeroCard)
            {
                (card as HeroCard).Empower = true;
                Debug.Log(card.GetType().Name + " was empowered!");
            }
        }
    }
}
