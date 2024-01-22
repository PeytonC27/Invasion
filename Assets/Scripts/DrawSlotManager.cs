using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;

public class DrawSlotManager : MonoBehaviour
{
    Slot slot1;
    Slot slot2;
    Slot slot3;
    GameObject selectHighlight;
    int selected = 1;

    // Start is called before the first frame update
    void Start()
    {
        slot1 = transform.GetChild(0).GetComponent<Slot>();
        slot2 = transform.GetChild(1).GetComponent<Slot>();
        slot3 = transform.GetChild(2).GetComponent<Slot>();
        selectHighlight = transform.GetChild(3).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.mouseScrollDelta.y < 0)
        {
            selected++;
            if (selected > 3)
                selected = 0;
            selectHighlight.transform.position = GetSlotFromSelection().transform.position;
        }
        else if (Input.mouseScrollDelta.y > 0)
        {
            selected--;
            if (selected < 1)
                selected = 3;
            selectHighlight.transform.position = GetSlotFromSelection().transform.position;
        }
    }

    /// <summary>
    /// Clears the currently highlighted slot
    /// </summary>
    public void ClearCurrentSlot()
    {
        GetSlotFromSelection().RemoveCard();
    }

    /// <summary>
    /// Returns the currently highlighted slot
    /// </summary>
    /// <returns></returns>
    public Slot ExtractSlotFromSelection()
    {
        return GetSlotFromSelection();
    }

    public void FillSlots(HeroCard card1, HeroCard card2, HeroCard card3)
    {
        selectHighlight.SetActive(true);
        slot1.AddCard(card1);
        slot2.AddCard(card2);
        slot3.AddCard(card3);
        selected = 1;
        selectHighlight.transform.position = slot1.transform.position;
    }

    /// <summary>
    /// Clears every slot, returning the leftover cards
    /// </summary>
    /// <returns></returns>
    public List<HeroCard> ClearSlots()
    {
        List<HeroCard> retval = new();
        selectHighlight.SetActive(false);
        if (slot1.HasCard)
            retval.Add(slot1.Card as HeroCard);
        if (slot2.HasCard)
            retval.Add(slot2.Card as HeroCard);
        if (slot3.HasCard)
            retval.Add(slot3.Card as HeroCard);

        slot1.RemoveCard();
        slot2.RemoveCard();
        slot3.RemoveCard();

        return retval;
    }

    Slot GetSlotFromSelection()
    {
        switch (selected)
        {
            case 1: return slot1;
            case 2: return slot2;
            case 3: return slot3;
            default: return slot1;
        }
    }
}
