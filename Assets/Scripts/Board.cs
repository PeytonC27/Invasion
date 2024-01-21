using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Board : MonoBehaviour
{
    [SerializeField] Slot clickableSlot;
    [SerializeField] Slot nonClickableSlot;

    Slot[,] board;


    private float[] xCoords = { -7.5f, -4.5f, -1.5f, 1.5f, 4.5f, 7.5f };
    private float[] yCoords = { 4.5f, 0f, -4.5f };
    private void Start()
    {
        board = new Slot[6, 3];

        for (int y = 0;  y < yCoords.Length; y++)
        {
            for (int x = 0; x < xCoords.Length; x++)
            {
                Slot toInstantiate = (x < 3) ? clickableSlot : nonClickableSlot;

                Slot slot = Instantiate(toInstantiate, new Vector2(xCoords[x], yCoords[y]), Quaternion.identity, transform);
                slot.row = y;
                slot.GetComponent<SpriteRenderer>().sortingOrder = 1;
                slot.name = "Slot " + x + " " + y;
                board[x, y] = slot;
            }
        }
    }
}
