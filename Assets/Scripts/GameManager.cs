using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Player Turn Options")]
    [SerializeField] KeyCode replace = KeyCode.R;
    [SerializeField] KeyCode discard = KeyCode.D;

    private Stack<Card> deck;
    private List<Card> playerHand;
    private List<Card> drawHand;
    private List<Card> discardPile;

    DrawSlotManager drawSlotManager;
    bool playerCardPhase = true, playerActionPhase = false;
    bool turnStarted = false;
    int mana = 2;
    int cardOptions;


    private void Start()
    {
        drawSlotManager = GetComponentInChildren<DrawSlotManager>();

        deck = new Stack<Card>();
        playerHand = new List<Card>();
        drawHand = new List<Card>();
        discardPile = new List<Card>();

        ShuffleDeck();

        // -8, -1, 5, -5
        
    }

    private void Update()
    {
        // start the player's turn
        if (playerCardPhase && !turnStarted)
        {
            turnStarted = true;
            cardOptions = 2;

            // clear cards to discard
            if (drawHand.Count > 0)
            {
                foreach (Card card in drawHand)
                {
                    discardPile.Add(card);
                }
            }

            mana = 2;

            // get three most recent cards
            Card card1 = deck.Pop();
            Card card2 = deck.Pop();
            Card card3 = deck.Pop();
            drawSlotManager.FillSlots(card1, card2, card3);

            // add them to the discard pile
            discardPile.Add(card1);
            discardPile.Add(card2);
            discardPile.Add(card3);
        }
        // start the action turn
        else if (!playerCardPhase && playerActionPhase)
        {
            drawSlotManager.ClearSlots();
        }


        // this ends the card phase
        if (playerCardPhase && Input.GetKeyDown(KeyCode.Escape) || cardOptions <= 0)
        {
            playerCardPhase = false;
            playerActionPhase = true;
        }
    }

    public void OnSlotClicked(Slot slot)
    {
        if (cardOptions > 0)
        {
            if (Input.GetKey(replace) && slot.HasCard)
            {
                Slot drawSlot = drawSlotManager.ExtractSlotFromSelection();
                if (drawSlot.Card == null)
                    return;

                slot.AddCard(drawSlot.Card);
                drawSlotManager.ClearCurrentSlot();
                cardOptions--;
            }
            else if (Input.GetKey(discard) && slot.HasCard)
            {
                slot.RemoveCard();
                cardOptions--;
            }
            else if (!slot.HasCard)
            {
                Slot drawSlot = drawSlotManager.ExtractSlotFromSelection();
                if (drawSlot.Card == null)
                    return;

                slot.AddCard(drawSlot.Card);
                drawSlotManager.ClearCurrentSlot();
                cardOptions--;
            }
        }
    }

    void HandleEnemyTurn()
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
