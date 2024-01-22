using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Card
{
    public Sprite Sprite { get; protected set; }
    public int Defense { get; protected set; }
    public int Damage { get; protected set; }

    public Card(Sprite sprite, int damage)
    {
        Sprite = sprite;
        Damage = damage;
    }

    public abstract void Action(Board board, int row, int column);
}
