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
    private Stack<HeroCard> deck;
    private List<HeroCard> discardPile;

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

        deck = new Stack<HeroCard>();
        discardPile = new List<HeroCard>();

        enemyDeck = new Stack<EnemyCard>();

        SetupDeck();
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
            discardPile.AddRange(drawSlotManager.ClearSlots());

            // get three most recent cards
            HeroCard card1 = DrawNextCard();
            HeroCard card2 = DrawNextCard();
            HeroCard card3 = DrawNextCard();
            drawSlotManager.FillSlots(card1, card2, card3);

            // add them to the discard pile
            discardPile.Add(card1);
            discardPile.Add(card2);
            discardPile.Add(card3);
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
        if (enemyDeck.Count < 3)
            ShuffleEnemyDeck();
        board.FillRightmostColumn(enemyDeck.Pop(), enemyDeck.Pop(), enemyDeck.Pop());
        yield return new WaitForSeconds(0.5f);

        // move active enemies
        discardPile.AddRange(board.MoveAllEnemies());
        yield return new WaitForSeconds(0.5f);

        // perform enemy actions
        board.PerformEnemyActions();

        board.RefreshHealthDisplay();

        playerCardPhase = true;
        playerActionPhase = false;
    }

    public HeroCard DrawNextCard()
    {
        if (deck.Count == 0)
            ShuffleInDeck(discardPile);
        return deck.Pop();
    }

    void SetupDeck()
    {
        List<HeroCard> allCards = new List<HeroCard>();

        // add nine of each basic card
        for (int i = 0; i < 9; i++) 
        {
            allCards.Add(new ShieldBearer());
            allCards.Add(new Cleric());
            allCards.Add(new Squire());
            allCards.Add(new Trickster());
        }

        ShuffleInDeck(allCards);
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
            enemyDeck.Push(card);
    }

    void ShuffleInDeck(List<HeroCard> allCards)
    {
        // shuffle all the cards
        int n = allCards.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n);
            HeroCard value = allCards[k];
            allCards[k] = allCards[n];
            allCards[n] = value;
        }

        foreach (HeroCard card in allCards)
            deck.Push(card);
    }
}
