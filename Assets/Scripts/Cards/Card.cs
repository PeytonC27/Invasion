using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Card
{
    public string Name { get; protected set; }
    public Sprite Sprite { get; protected set; }
    public int Damage { get; protected set; }

    public Card(string name, Sprite sprite, int damage)
    {
        Name = name;
        Sprite = sprite;
        Damage = damage;
    }

    public abstract void Action(Board board, int row, int column);

    protected void DamageHero(EnemyCard enemy, HeroCard hero, Board board, int row, int column)
    {
        // paladin interaction
        if (hero is Paladin && (hero as Paladin).Countering)
        {
            Debug.Log(hero.Name + " countered the " + enemy.Name + "'s attack! It dealt " + enemy.Damage + " damage!");
            CounterAttack(hero, enemy);
        }

        if (hero.Protected && hero.Defense < enemy.Damage)
        {
            Debug.Log(hero.Name + " was protected from the attack!");
        }
        else if (hero.Defense < enemy.Damage)
        {
            Debug.Log(hero.Name + " was killed by " + enemy.Name);
            board.RemoveCardAt(row, column);
        }
        else
        {
            Debug.Log(hero.Name + " withstood the " + enemy.Name + "'s attack!");
        }
    }

    protected void DamageEnemy(HeroCard hero, EnemyCard enemy)
    {
        enemy.Health -= hero.Damage + (hero.Empower ? 1 : 0);
        Debug.Log("Hit " + enemy.Name);

        if (enemy.Health <= 0)
            Debug.Log(hero.Name + " killed a(n)" + enemy.Name + "!");
    }

    protected void CounterAttack(HeroCard hero, EnemyCard enemy)
    {
        enemy.Health -= enemy.Damage;

        if (enemy.Health <= 0)
            Debug.Log(hero.Name + " killed a(n)" + enemy.Name + "!");
    }
}
