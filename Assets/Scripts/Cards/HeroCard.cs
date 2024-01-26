using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HeroCard : Card
{
    public int Defense { get; protected set; }
    public bool Empower { get; set; } = false;
    public bool Protected { get; set;} = false;
    public bool OnCooldown { get; set; } = false;
    protected HeroCard(String name, Sprite sprite, int damage, int defense) : base(name, sprite, damage) 
    {
        Defense = defense;
    }

    /// <summary>
    /// Special action to be performed against a card
    /// </summary>
    /// <param name="board">The board in play</param>
    /// <param name="card">The card taking the action</param>
    public virtual void SpecialAction(Board board, Card card) { }
}
