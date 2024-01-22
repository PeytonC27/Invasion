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

            mana = 3;

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
        if (playerCardPhase && Input.GetKeyDown(KeyCode.Escape) || cardOptions <= 0)
        {
            playerCardPhase = false;
            cardPhaseActive = false;
            playerActionPhase = true;
            cardOptions = 2;

            // count extra mana from clerics
            Slot[,] grid = board.GetGrid();

            for (int i = 0; i < grid.GetLength(0); i++)
                for (int j = 0; j < grid.GetLength(1); j++)
                    if (grid[i, j].HasCard && grid[i, j].Card is Cleric)
                        mana++;

            Debug.Log("Mana: " + mana);
        }

        // ==================== ENEMY TURN ==================== //
        if (mana == 0 || Input.GetKeyDown(KeyCode.Escape))
        {
            // fill rightmost column
            board.FillRightmostColumn(enemyDeck.Pop(), enemyDeck.Pop(), enemyDeck.Pop());


            // move active enemies
            Slot[,] grid = board.GetGrid();
            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0;j < grid.GetLength(1); j++)
                {
                    if (grid[i, j].HasCard && grid[i,j].Card is EnemyCard)
                    {
                        EnemyCard e = grid[i, j].Card as EnemyCard;
                        grid[i - e.MoveSpeed, j].AddCard(e);
                        grid[i, j].RemoveCard();
                    }
                }
            }

            // perform enemy actions
            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    if (grid[i, j].HasCard && grid[i, j].Card is EnemyCard)
                    {
                        grid[i, j].Card.Action(grid, j, i);
                    }
                }
            }

            board.RefreshHealthDisplay();

            playerCardPhase = true;
            playerActionPhase = false;
        }
    }

    public void OnSlotClicked(Slot slot)
    {
        // ==================== CARD MANIPULATION ==================== //
        if (cardOptions > 0 && cardPhaseActive)
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

        // ==================== CARD ACTIONS ==================== //
        else
        {
            if (slot.HasCard)
            {
                slot.Card.Action(board.GetGrid(), slot.row, slot.position);
                mana--;
                Debug.Log("Action triggered at " + slot.Card.GetType().Name);
                board.RefreshHealthDisplay();
            }
        }
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
