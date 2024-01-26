using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;

public class DrawSlotManager : MonoBehaviour
{
    [SerializeField] Slot nonClickableSlot;
    [SerializeField] float yOffset;
    [SerializeField] float length = 9;

    List<Slot> slots;
    GameObject selectHighlight;
    int selected = 0;
    public int cardsToDraw = 5;

    // Start is called before the first frame update
    void Start()
    {
        slots = new List<Slot>();
        selectHighlight = transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsEmpty())
        {
            if (Input.mouseScrollDelta.y < 0)
            {
                selected++;
                if (selected >= slots.Count)
                    selected = 0;
                selectHighlight.transform.position = GetSlotFromSelection().transform.position;
            }
            else if (Input.mouseScrollDelta.y > 0)
            {
                selected--;
                if (selected < 0)
                    selected = slots.Count-1;
                selectHighlight.transform.position = GetSlotFromSelection().transform.position;
            }
        }
    }

    /// <summary>
    /// Returns the currently highlighted slot
    /// </summary>
    /// <returns></returns>
    public Slot ExtractSlotFromSelection()
    {
        return GetSlotFromSelection();
    }

    /// <summary>
    /// This replaces the current selection with the passed
    /// in card
    /// </summary>
    /// <param name="card"></param>
    public void ReplaceCard(Card card)
    {
        GetSlotFromSelection().AddCard(card);
    }

    /// <summary>
    /// Clears the currently highlighted slot
    /// </summary>
    public void ClearCurrentSlot()
    {
        GetSlotFromSelection().RemoveCard();
    }


    /// <summary>
    /// Draws the default hand size into the player's hand
    /// </summary>
    /// <param name="cards"></param>
    public void DrawCards(Stack<HeroCard> cards)
    {
        selectHighlight.SetActive(true);
        float diff = length/(cardsToDraw-1);
        float posX = -length/2;
        for (int i = 0; i < cardsToDraw; i++)
        {
            Slot newSlot = Instantiate(nonClickableSlot, new Vector2(posX, -10 + yOffset), Quaternion.identity, transform);
            newSlot.AddCard(cards.Pop());
            slots.Add(newSlot);
            posX += diff;
        }
        selected = 0;
        selectHighlight.transform.position = slots[0].transform.position;
    }

    /// <summary>
    /// Draws a specfic amount of cards into the player's hand
    /// </summary>
    /// <param name="cards"></param>
    /// <param name="amount"></param>
    public void DrawCards(Stack<HeroCard> cards, int amount)
    {
        selectHighlight.SetActive(true);
        float diff = length / (amount - 1);
        float posX = -length / 2;
        for (int i = 0; i < amount; i++)
        {
            Slot newSlot = Instantiate(nonClickableSlot, new Vector2(posX, -10 + yOffset), Quaternion.identity, transform);
            newSlot.AddCard(cards.Pop());
            slots.Add(newSlot);
            posX += diff;
        }
        selected = 0;
        selectHighlight.transform.position = slots[0].transform.position;
    }

    /// <summary>
    /// Clears every slot, returning the leftover cards
    /// </summary>
    /// <returns></returns>
    public List<HeroCard> ClearSlots()
    {
        List<HeroCard> retval = new();
        for (int i = 0; i < slots.Count; i++)
        {
            retval.Add(slots[i].Card as HeroCard);
            Destroy(slots[i].gameObject);
        }

        slots.Clear();
        selectHighlight.SetActive(false);
        return retval;
    }

    /// <summary>
    /// Gets the slot based on the player's selection
    /// </summary>
    /// <returns></returns>
    Slot GetSlotFromSelection()
    {
        return slots[selected];
    }

    public bool IsEmpty()
    {
        return slots.Count == 0;
    }
}
