using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HeroCard : Card
{
    public int Defense { get; protected set; }
    public int DamageBuff { get; set; }
    protected HeroCard(Sprite sprite, int damage, int defense) : base(sprite, damage) 
    {
        Defense = defense;
    }
}
