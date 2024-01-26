using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
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

    // switching/altering turns
    DrawSlotManager drawSlotManager;
    Board board;
    bool playerCardPhase = true, playerActionPhase = false, reset = true;
    int mana = 3;
    bool manaSet = false;
    int cardOptions;

    // these are used when a card uses an ability that needs a second input
    Card awaitingCard;
    bool awaitingSelection;

    // display
    DisplayManager displayManager;

    private void Start()
    {
        drawSlotManager = GetComponentInChildren<DrawSlotManager>();

        deck = new Stack<HeroCard>();
        discardPile = new List<HeroCard>();

        enemyDeck = new Stack<EnemyCard>();

        SetupDeck();
        ShuffleEnemyDeck();

        board = GetComponentInChildren<Board>();

        displayManager = GetComponentInChildren<DisplayManager>();


        // -8, -1, 5, -5
        
    }

    private void Update()
    {
        // ==================== PLAYER CARD TURN ==================== //
        if (playerCardPhase && drawSlotManager.IsEmpty() && !playerActionPhase && reset)
        {
            reset = false;
            EnterCardPhase(2, drawSlotManager.cardsToDraw);
        }


        // ==================== ENDING CARD PHASE ==================== //
        if (playerCardPhase && Input.GetKeyDown(KeyCode.Space))
        {
            playerCardPhase = false;
            playerActionPhase = true;

            // count extra mana from clerics
            if (!manaSet)
                mana = 3 + board.Count((Card c) => c is Cleric);
            manaSet = true;

            // discard hand
            List<HeroCard> discarded = drawSlotManager.ClearSlots();
            for (int i = 0; i < discarded.Count; i++)
                discardPile.Add(discarded[i]);
            

            Debug.Log("Mana: " + mana);
        }

        // ==================== ENEMY TURN ==================== //
        if (Input.GetKeyDown(KeyCode.Escape) && playerActionPhase)
        {
            playerActionPhase = false;
            reset = true;
            manaSet = false;
            StartCoroutine(EnemyTurn());
            playerCardPhase = true;
        }

        // updating UI
        displayManager.UpdatePhaseDisplay(playerCardPhase, playerActionPhase);
        displayManager.UpdateManaDisplay(mana, playerActionPhase);
    }

    public void OnSlotClicked(Slot slot)
    {
        // ==================== CARD MANIPULATION ==================== //
        if (cardOptions > 0 && playerCardPhase && !slot.IsEnemySlot())
        {
            if (Input.GetKey(replace) && slot.HasCard && slot.Card is not EnemyCard)
            {
                // extract the slot and its card
                Slot drawSlot = drawSlotManager.ExtractSlotFromSelection();
                if (drawSlot.Card == null)
                    return;

                // replace the cards
                Card toReplace = slot.Card;
                slot.AddCard(drawSlot.Card);
                drawSlotManager.ReplaceCard(toReplace);
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

        // ==================== AWAITING ACTION ==================== //
        else if (awaitingSelection)
        {
            // if the mage/paladin is waiting
            if (slot.HasCard && (awaitingCard is Mage || awaitingCard is Paladin))
            {
                HeroCard hero = awaitingCard as HeroCard;
                hero.SpecialAction(board, slot.Card);
                board.RefreshHealthDisplay();
                awaitingSelection = false;
                awaitingCard = null;
            }
        }

        // ==================== CARD ACTIONS ==================== //
        else if (playerActionPhase && !slot.IsEnemySlot())
        {
            if (!slot.HasCard)
                return;


            Card card = slot.Card;

            // every card slot except the farmer
            if (mana > 0 && card is HeroCard && !(card as HeroCard).OnCooldown)
            {
                HeroCard hero = card as HeroCard;

                // all other cards have simple actions
                
                if (hero is Mage || hero is Paladin)
                {
                    awaitingSelection = true;
                    awaitingCard = hero;
                }
                // the farmer essentially brings you back to the card stage
                // where you can place/replace a card
                else if (hero is Farmer)
                {
                    playerCardPhase = true;
                    playerActionPhase = false;
                    EnterCardPhase(1, 2);
                }

                hero.Action(board, slot.row, slot.position);
                hero.OnCooldown = true;
                mana--;
                board.RefreshHealthDisplay();
            }
            else if ((card as HeroCard).OnCooldown) 
            {
                Debug.Log(card.Name + " is on cooldown");
            }
        }
    }

    IEnumerator EnemyTurn()
    {
        // fill rightmost column
        if (enemyDeck.Count < 3)
            ShuffleEnemyDeck();
        board.FillRightmostColumn(enemyDeck.Pop(), enemyDeck.Pop(), enemyDeck.Pop());
        board.RefreshHealthDisplay();
        yield return new WaitForSeconds(0.5f);

        // move active enemies
        discardPile.AddRange(board.MoveAllEnemies());
        board.RefreshHealthDisplay();
        yield return new WaitForSeconds(0.5f);

        // perform enemy actions
        board.PerformEnemyActions();
        board.RefreshHealthDisplay();
        board.ResetDamageBuffs();
    }

    void EnterCardPhase(int options, int cardsToDraw)
    {
        cardOptions = options;

        // clear cards to discard
        if (deck.Count < cardsToDraw)
            ShuffleInDeck(discardPile);

        // get three most recent cards
        drawSlotManager.DrawCards(deck, cardsToDraw);
    }

    public void DisplayCardInfo(Card card)
    {
        displayManager.UpdateInfoDisplay(card, playerActionPhase);
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
            allCards.Add(new Mage());
            allCards.Add(new Warrior());
            allCards.Add(new Paladin());
            allCards.Add(new Farmer());
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

        discardPile.Clear();
    }
}
