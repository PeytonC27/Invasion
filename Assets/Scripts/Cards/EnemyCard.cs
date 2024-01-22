using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyCard : Card
{
    public EnemyCard(Sprite sprite, int health, int damage, int speed) : base(sprite, damage) 
    {
        Health = health;
        MoveSpeed = speed;
    }

    public int Health { get; set; }
    public int MoveSpeed { get; set; }
}
