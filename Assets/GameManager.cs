using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private Stack<Card> deck;
    private List<Card> playerHand;


    private void Start()
    {
        deck = new Stack<Card>();
        playerHand = new List<Card>();

        ShuffleDeck();
    }

    private void Update()
    {
        
    }

    void ShuffleDeck()
    {
        List<Card> allCards = new List<Card>();

        // add nine of each basic card
        for (int i = 0; i < 9; i++) 
        {
            allCards.Add(new ShieldBearer());
            allCards.Add(new Cleric());
            allCards.Add(new Squire());
            allCards.Add(new Trickster());
        }

        // shuffle all the cards
        int n = allCards.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n);
            Card value = allCards[k];
            allCards[k] = allCards[n];
            allCards[n] = value;
        }

        // add the cards to the deck
        foreach (Card card in allCards)
        {
            deck.Push(card);
            Debug.Log(card.GetType().Name);
        }
    }
}
