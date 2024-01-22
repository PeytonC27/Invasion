using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Player Turn Options")]
    [SerializeField] KeyCode replace = KeyCode.R;
    [SerializeField] KeyCode discard = KeyCode.D;

    // player deck
    private Stack<Card> deck;
    private List<Card> playerHand;
    private List<Card> drawHand;
    private List<Card> discardPile;

    // enemy deck
    private Stack<EnemyCard> enemyDeck;
    private List<EnemyCard> enemyDiscard;

    DrawSlotManager drawSlotManager;
    Board board;
    bool playerCardPhase = true, playerActionPhase = false;
    bool cardPhaseActive = false;
    int mana = 3;
    int cardOptions;

    private void Start()
    {
        drawSlotManager = GetComponentInChildren<DrawSlotManager>();

        deck = new Stack<Card>();
        playerHand = new List<Card>();
        drawHand = new List<Card>();
        discardPile = new List<Card>();

        enemyDeck = new Stack<EnemyCard>();

        ShuffleDeck();
        ShuffleEnemyDeck();

        board = GetComponentInChildren<Board>();

        // -8, -1, 5, -5
        
    }

    private void Update()
    {
        // ==================== PLAYER CARD TURN ==================== //
        if (playerCardPhase && !cardPhaseActive)
        {
            cardPhaseActive = true;
            cardOptions = 2;

            // clear cards to discard
            if (drawHand.Count > 0)
            {
                foreach (Card card in drawHand)
                {
                    discardPile.Add(card);
                }
            }

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
        // ==================== PLAYER ACTION TURN ==================== //
        else if (!playerCardPhase && playerActionPhase)
        {
            drawSlotManager.ClearSlots();
        }


        // ==================== ENDING CARD PHASE ==================== //
        if (playerCardPhase && Input.GetKeyDown(KeyCode.Space))
        {
            playerCardPhase = false;
            cardPhaseActive = false;
            playerActionPhase = true;
            cardOptions = 2;

            // count extra mana from clerics
            mana = 3 + board.Count((Card c) => c is Cleric);

            Debug.Log("Mana: " + mana);
        }

        // ==================== ENEMY TURN ==================== //
        if (Input.GetKeyDown(KeyCode.Escape) && playerActionPhase)
        {
            playerActionPhase = false;
            mana = 0;
            StartCoroutine(EnemyTurn());
        }
    }

    public void OnSlotClicked(Slot slot)
    {
        // ==================== CARD MANIPULATION ==================== //
        if (cardOptions > 0 && cardPhaseActive)
        {
            if (Input.GetKey(replace) && slot.HasCard && slot.Card is not EnemyCard)
            {
                Slot drawSlot = drawSlotManager.ExtractSlotFromSelection();
                if (drawSlot.Card == null)
                    return;

                slot.AddCard(drawSlot.Card);
                drawSlotManager.ClearCurrentSlot();
                cardOptions--;
            }
            else if (Input.GetKey(discard) && slot.HasCard && slot.Card is not EnemyCard)
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

        // ==================== CARD ACTIONS ==================== //
        else
        {
            if (slot.HasCard && mana > 0)
            {
                slot.Card.Action(board, slot.row, slot.position);
                mana--;
                board.RefreshHealthDisplay();
            }
        }
    }

    IEnumerator EnemyTurn()
    {
        // fill rightmost column
        board.FillRightmostColumn(enemyDeck.Pop(), enemyDeck.Pop(), enemyDeck.Pop());
        yield return new WaitForSeconds(0.5f);

        // move active enemies
        board.MoveAllEnemies();
        yield return new WaitForSeconds(0.5f);

        // perform enemy actions
        board.PerformEnemyActions();

        board.RefreshHealthDisplay();

        playerCardPhase = true;
        playerActionPhase = false;
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
        }
    }

    void ShuffleEnemyDeck()
    {
        List<EnemyCard> allCards = new List<EnemyCard>();

        // add nine of each basic card
        for (int i = 0; i < 9; i++)
        {
            allCards.Add(new Ogre());
            allCards.Add(new Goblin());
            allCards.Add(new Hellhound());
            allCards.Add(new Dragonborn());
        }

        // shuffle all the cards
        int n = allCards.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n);
            EnemyCard value = allCards[k];
            allCards[k] = allCards[n];
            allCards[n] = value;
        }

        // add the cards to the deck
        foreach (EnemyCard card in allCards)
        {
            enemyDeck.Push(card);
        }
    }
}
