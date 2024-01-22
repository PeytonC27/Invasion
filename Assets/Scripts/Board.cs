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

    public Slot[,] GetGrid()
    {
        return board;
    }

    public void FillRightmostColumn(EnemyCard enemy1, EnemyCard enemy2, EnemyCard enemy3)
    {
        board[5, 0].AddCard(enemy1);
        board[5, 1].AddCard(enemy2);
        board[5, 2].AddCard(enemy3);
    }

    public void RefreshHealthDisplay()
    {
        for (int i = 0; i < board.GetLength(0); i++)
        {
            for (int j = 0; j < board.GetLength(1); j++)
            {
                if (board[i, j].HasCard && board[i,j].Card is EnemyCard) 
                {
                    EnemyCard e = board[i,j].Card as EnemyCard;
                    board[i, j].SetLabel(e.Health.ToString());
                }
            }
        }
    }
}
