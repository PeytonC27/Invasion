using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
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
                slot.position = x;
                slot.GetComponent<SpriteRenderer>().sortingOrder = 1;
                slot.name = "Slot " + x + " " + y;
                board[x, y] = slot;
            }
        }
    }

    public Card GetCardAt(int row, int column)
    {
        return board[column, row].Card;
    }

    public void MoveAllEnemies()
    {
        for (int i = 0; i < board.GetLength(0); i++)
        {
            for (int j = 0; j < board.GetLength(1); j++)
            {
                if (board[i, j].HasCard && board[i, j].Card is EnemyCard)
                {
                    EnemyCard e = board[i, j].Card as EnemyCard;

                    // check if the player lost
                    if (i - e.MoveSpeed < 0)
                    {
                        Debug.Log("The player lost.");
                        Time.timeScale = 0;
                    }

                    // trample over every card in the enemy's path
                    for (int k = i - e.MoveSpeed; k > i; k--)
                    {
                        if (board[k, j].HasCard)
                        {
                            Debug.Log(e.GetType().Name + " trampled a " + board[i - e.MoveSpeed, j].Card.GetType().Name);
                            board[k, j].RemoveCard();
                        }
                    }
                    
                    // remove the original placement and update the new slot
                    board[i - e.MoveSpeed, j].AddCard(e);
                    board[i, j].RemoveCard();
                }
            }
        }
    }

    public void PerformEnemyActions()
    {
        for (int i = 0; i < board.GetLength(0); i++)
        {
            for (int j = 0; j < board.GetLength(1); j++)
            {
                if (board[i, j].HasCard && board[i, j].Card is EnemyCard)
                {
                    board[i, j].Card.Action(this, j, i);
                }
            }
        }
    }

    /// <summary>
    /// A special method used to count instances in the board based on a passed in function
    /// </summary>
    /// <param name="counterFunction"></param>
    /// <returns></returns>
    public int Count(Func<Card, bool> counterFunction)
    {
        int count = 0;
        for (int i = 0; i < board.GetLength(0); i++)
            for (int j = 0; j < board.GetLength(1); j++)
                if (counterFunction.Invoke(board[i, j].Card))
                    count++;
        return count;
    }

    public void RemoveCardAt(int row, int column)
    {
        board[column, row].RemoveCard();
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
                    if (e.Health <= 0)
                        RemoveCardAt(j, i);
                    else
                        board[i, j].SetLabel(e.Health.ToString());
                }
                else
                {
                    board[i,j].DisableLabel();
                }
            }
        }
    }
}
