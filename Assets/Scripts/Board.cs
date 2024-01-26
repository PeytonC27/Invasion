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
    [SerializeField] int length;
    [SerializeField] int height;
    [SerializeField] float yOffset = 2;
    [SerializeField] Vector2 startingPosition = new(-10.5f, -4.5f); // the top left

    Slot[,] board;

    public static int boardLength;
    public static int boardHeight;

    private void Start()
    {
        boardLength = length;
        boardHeight = height;

        board = new Slot[length, height];

        float xPos = startingPosition.x;
        float yPos = startingPosition.y;
        float xJump = (Math.Abs(startingPosition.x) * 2) / (length-1);
        float yJump = (Math.Abs(startingPosition.y) * 2) / (height-1);

        // spawn the slots
        for (int y = 0;  y < height; y++)
        {
            for (int x = 0; x < length; x++)
            {
                Slot slot = Instantiate(clickableSlot, new Vector2(xPos, yPos + yOffset), Quaternion.identity, transform);
                slot.row = y;
                slot.position = x;
                slot.GetComponent<SpriteRenderer>().sortingOrder = 1;
                slot.name = "Slot " + x + " " + y;
                board[x, y] = slot;
                xPos += xJump;
            }
            xPos = startingPosition.x;
            yPos += yJump;
        }
    }

    public Card GetCardAt(int row, int column)
    {
        return board[column, row].Card;
    }

    /// <summary>
    /// Moves all the enemies forwards, returning a list of all the hero cards
    /// that were trampled in the process
    /// </summary>
    /// <returns></returns>
    public List<HeroCard> MoveAllEnemies()
    {
        List<HeroCard> retval = new();
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
                        return new();
                    }

                    // trample over every card in the enemy's path
                    for (int k = i - e.MoveSpeed; k < i; k++)
                    {
                        if (board[k, j].HasCard)
                        {
                            Debug.Log(e.Name + " trampled a " + board[k, j].Card.Name);
                            Card trampled = board[k, j].RemoveCard();
                            if (trampled is HeroCard)
                                retval.Add(trampled as HeroCard);
                        }
                    }
                    
                    // remove the original placement and update the new slot
                    board[i - e.MoveSpeed, j].AddCard(e);
                    board[i, j].RemoveCard();
                }
            }
        }

        return retval;
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

    public Slot GetSlotFromCard(Card card)
    {
        for (int i = 0; i < board.GetLength(0); i++)
        {
            for (int j = 0; j < board.GetLength(1); j++)
            {
                if (board[i, j].Card == card)
                    return board[i, j];
            }
        }
        return null;
    }

    /// <summary>
    /// Clears the damage buff for every hero on the board
    /// </summary>
    public void ResetDamageBuffs()
    {
        for (int i = 0; i < board.GetLength(0); i++)
        {
            for (int j = 0; j < board.GetLength(1); j++)
            {
                Card card = board[i, j].Card;
                if (card is HeroCard)
                {
                    HeroCard hero = card as HeroCard;
                    hero.Empower = false;
                    hero.Protected = false;
                    hero.OnCooldown = false;
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
