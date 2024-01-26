using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyCard : Card
{
    public EnemyCard(string name, Sprite sprite, int health, int damage, int speed) : base(name, sprite, damage) 
    {
        Health = health;
        MoveSpeed = speed;
    }

    public int Health { get; set; }
    public int MoveSpeed { get; set; }
}
