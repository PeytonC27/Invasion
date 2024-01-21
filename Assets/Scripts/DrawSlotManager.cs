using System.Collections;
using System.Collections.Generic;
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

    public void ClearCurrentSlot()
    {
        GetSlotFromSelection().RemoveCard();
    }

    public Slot ExtractSlotFromSelection()
    {
        return GetSlotFromSelection();
    }

    public void FillSlots(Card card1, Card card2, Card card3)
    {
        selectHighlight.SetActive(true);
        slot1.AddCard(card1);
        slot2.AddCard(card2);
        slot3.AddCard(card3);
        selectHighlight.transform.position = slot1.transform.position;
    }

    public void ClearSlots()
    {
        selectHighlight.SetActive(false);
        slot1.RemoveCard();
        slot2.RemoveCard();
        slot3.RemoveCard();
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
